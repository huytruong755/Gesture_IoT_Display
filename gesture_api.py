"""
Flask API Server cho Gesture Recognition
Chạy gesture detection và stream qua HTTP
"""
import cv2
import numpy as np
import os
from tensorflow.keras.models import Sequential
from tensorflow.keras.layers import Conv3D, MaxPooling3D, Flatten, Dense, Dropout, Input
from tensorflow.keras.optimizers import Adam
import mediapipe
from mediapipe.python.solutions import hands as mp_hands, drawing_utils as mp_draw
from collections import deque
import serial
import time
import json
import base64
import threading
from flask import Flask, jsonify, request
from datetime import datetime
import pyodbc

app = Flask(__name__)

# ═══════════════════════════════════════════════════════════
# DATABASE CONNECTION
# ═══════════════════════════════════════════════════════════
DB_CONNECTION = r'Driver={ODBC Driver 17 for SQL Server};Server=localhost\SQLEXPRESS;Database=Database_Gesture;Trusted_Connection=yes;'

def get_db_connection():
    """Kết nối database"""
    try:
        conn = pyodbc.connect(DB_CONNECTION)
        return conn
    except Exception as e:
        print(f"DB Connection Error: {e}")
        return None

def execute_query(query, params=None):
    # Thực thi query
    conn = get_db_connection()
    if not conn:
        print("Database connection failed")
        return None
    try:
        cursor = conn.cursor()
        if params:
            print(f"Executing query with params: {params}")
            cursor.execute(query, params)
        else:
            print(f"Executing query")
            cursor.execute(query)
        
        # Fetch trước khi commit
        if 'SELECT' in query.upper():
            result = cursor.fetchall()
        else:
            result = cursor.rowcount
        
        conn.commit()
        cursor.close()
        conn.close()
        print(f"Query executed successfully, result: {result}")
        return result
    except Exception as e:
        print(f"Query Error: {e}")
        print(f"   Query: {query}")
        print(f"   Params: {params}")
        if conn:
            conn.close()
        return None

# ═══════════════════════════════════════════════════════════
# CONFIGURATION
# ═══════════════════════════════════════════════════════════
FRAMES_TO_CAPTURE = 20
CONFIDENCE_THRESHOLD = 0.4
MAX_RESULTS = 3
COOLDOWN_FRAMES = 5
HAND_ABSENT_THRESHOLD = 8

actions = np.array([
    'want_rice', 'want_drink', 'change_dish', 'dessert',
    'satisfied', 'not_satisfied', 'ask_time', 'clear_tray'
])

action_labels = {
    'want_rice': 'Muốn ăn cơm',
    'want_drink': 'Muốn uống nước',
    'change_dish': 'Muốn đổi món ăn',
    'dessert': 'Món tráng miệng',
    'satisfied': 'Hài lòng món ăn',
    'not_satisfied': 'Không hài lòng',
    'ask_time': 'Muốn hỏi mấy giờ',
    'clear_tray': 'Muốn phục vụ dọn khay'
}

gesture_codes = {
    'want_rice': 'R',
    'want_drink': 'D',
    'change_dish': 'C',
    'dessert': 'E',
    'satisfied': 'S',
    'not_satisfied': 'N',
    'ask_time': 'T',
    'clear_tray': 'L',
}

# ═══════════════════════════════════════════════════════════
# GLOBAL STATE
# ═══════════════════════════════════════════════════════════
class GestureEngine:
    def __init__(self):
        self.model = None
        self.hands = None
        self.mp_draw = None
        self.cap = None
        self.is_running = False
        self.current_frame = None
        self.current_gesture = None
        self.current_confidence = 0.0
        self.current_timestamp = None
        self.ser = None
        self.led_status = False
        
        # ⭐ Database
        self.com_port = None
        self.device_id = None
        self.session_id = None
        self.start_time = None
        self.gesture_count = 0
        self.confidence_sum = 0.0
        
        # Buffers
        self.frames_buffer = deque(maxlen=FRAMES_TO_CAPTURE)
        self.results_list = deque(maxlen=MAX_RESULTS)
        self.cooldown = 0
        self.last_sent_code = None
        self.hand_absent_frames = 0
        
        self.lock = threading.Lock()
        
    def initialize(self):
        """Khởi tạo model, camera, serial port"""
        try:
            # Build & load model
            self.model = self._build_model()
            try:
                self.model.load_weights('model3d.h5')
                print("✓ Model loaded successfully")
            except:
                print("⚠ Model weights not found, using untrained weights")
            
            # Initialize MediaPipe
            self.mp_draw = mp_draw
            self.hands = mp_hands.Hands(
                static_image_mode=False,
                max_num_hands=2,
                min_detection_confidence=0.5,
                min_tracking_confidence=0.5
            )
            
            # Open camera
            self.cap = cv2.VideoCapture(0)
            if not self.cap.isOpened():
                raise Exception("Cannot open camera")
            
            # Try to open serial port
            self._initialize_serial()
            
            print("✓ Engine initialized successfully")
            return True
            
        except Exception as e:
            print(f"✗ Initialization error: {e}")
            return False
    
    def _build_model(self):
        """Build neural network model"""
        model = Sequential([
            Input(shape=(FRAMES_TO_CAPTURE, 63, 1, 1)),
            Conv3D(32, (3, 3, 1), activation='relu', padding='same'),
            MaxPooling3D((1, 2, 1)),
            Conv3D(64, (3, 3, 1), activation='relu', padding='same'),
            MaxPooling3D((1, 2, 1)),
            Flatten(),
            Dense(128, activation='relu'),
            Dropout(0.5),
            Dense(64, activation='relu'),
            Dropout(0.3),
            Dense(len(actions), activation='softmax')
        ])
        model.compile(
            optimizer=Adam(learning_rate=0.0001),
            loss='categorical_crossentropy',
            metrics=['categorical_accuracy']
        )
        return model
    
    def _initialize_serial(self, com_port='COM3', baudrate=9600):
        """Initialize serial port for Arduino"""
        try:
            # ⭐ Đóng port cũ nếu còn mở
            if self.ser and self.ser.is_open:
                try:
                    self.ser.close()
                    time.sleep(0.2)
                except:
                    pass
            
            self.ser = serial.Serial(com_port, baudrate, timeout=1)
            self.com_port = com_port
            time.sleep(0.5)
            print(f"✓ Serial port {com_port} opened")
            
            # ⭐ Cập nhật Device status thành Online
            self.update_device_status('Online')
        except Exception as e:
            print(f"⚠ Cannot open serial port {com_port}: {e}")
            self.ser = None
    
    def update_device_status(self, status):
        """Cập nhật trạng thái Device trong database"""
        if not self.com_port:
            return
        try:
            query = """
            UPDATE dbo.Device 
            SET Status = ?, LastConnected = GETDATE()
            WHERE SerialPort = ?
            """
            execute_query(query, (status, self.com_port))
            print(f"✓ Device {self.com_port} status updated to {status}")
        except Exception as e:
            print(f"⚠ Update device status failed: {e}")
    
    def create_session(self, location=None):
        """Tạo GestureSession mới"""
        try:
            if not location:
                location = f'Auto_{datetime.now().strftime("%Y%m%d_%H%M%S")}'
            
            print(f"\n📍 Creating GestureSession with location: '{location}'")
            
            # Lấy DeviceId từ COM port
            query = "SELECT DeviceId FROM dbo.Device WHERE SerialPort = ?"
            result = execute_query(query, (self.com_port,))
            
            if not result:
                print("⚠ Device not found in database, creating new device...")
                # Tạo device mới nếu chưa tồn tại
                insert_query = """
                INSERT INTO dbo.Device (DeviceName, SerialPort, BaudRate, Status, LastConnected)
                VALUES (?, ?, 9600, 'Online', GETDATE())
                """
                execute_query(insert_query, (f"Arduino_{self.com_port}", self.com_port))
                result = execute_query(query, (self.com_port,))
            
            if result:
                self.device_id = result[0][0]
                print(f"✓ Found DeviceId: {self.device_id}")
                
                # Tạo session mới
                insert_query = """
                INSERT INTO dbo.GestureSession (DeviceId, Location, TotalGestures, AverageConfidence)
                VALUES (?, ?, 0, 0)
                """
                print(f"📝 Inserting GestureSession with DeviceId={self.device_id}, Location='{location}'")
                execute_query(insert_query, (self.device_id, location))
                
                # Lấy SessionId vừa tạo
                session_query = "SELECT TOP 1 SessionId FROM dbo.GestureSession WHERE DeviceId = ? ORDER BY SessionId DESC"
                session_result = execute_query(session_query, (self.device_id,))
                
                if session_result:
                    self.session_id = session_result[0][0]
                    self.start_time = datetime.now()
                    self.gesture_count = 0
                    self.confidence_sum = 0.0
                    print(f"✅ GestureSession created: SessionId={self.session_id}, Location='{location}'")
        except Exception as e:
            print(f"❌ Create session failed: {e}")
    
    def save_gesture_to_db(self, gesture_name, gesture_code, confidence, duration=0):
        """Lưu cử chỉ vào database"""
        if not self.session_id:
            return
        try:
            query = """
            INSERT INTO dbo.GestureHistory (SessionId, Gesture, Confidence, GestureCode, Duration, Quality)
            VALUES (?, ?, ?, ?, ?, ?)
            """
            quality = 'Good' if confidence >= 90 else ('Fair' if confidence >= 70 else 'Poor')
            execute_query(query, (self.session_id, gesture_name, int(confidence), gesture_code, duration, quality))
            
            # Cập nhật thống kê session
            self.gesture_count += 1
            self.confidence_sum += confidence
        except Exception as e:
            print(f"⚠ Save gesture failed: {e}")
    
    def end_session(self):
        """Kết thúc session và lưu thống kê"""
        if not self.session_id:
            return
        try:
            avg_confidence = self.confidence_sum / self.gesture_count if self.gesture_count > 0 else 0
            query = """
            UPDATE dbo.GestureSession 
            SET EndTime = GETDATE(), TotalGestures = ?, AverageConfidence = ?
            WHERE SessionId = ?
            """
            execute_query(query, (self.gesture_count, avg_confidence, self.session_id))
            print(f"✓ GestureSession ended: Total={self.gesture_count}, AvgConfidence={avg_confidence:.2f}%")
            
            # Reset session
            self.session_id = None
            self.gesture_count = 0
            self.confidence_sum = 0.0
        except Exception as e:
            print(f"⚠ End session failed: {e}")
    
    def _extract_keypoints(self, results):
        """Extract hand landmarks"""
        if results.multi_hand_landmarks:
            hand = results.multi_hand_landmarks[0]
            return np.array([[res.x, res.y, res.z] for res in hand.landmark]).flatten()
        return np.zeros(63)
    
    # def _send_serial(self, code):
    #     """Send code to Arduino"""
    #     if self.ser and self.ser.is_open:
    #         try:
    #             # Map short gesture codes to Arduino command names.
    #             # Adjust this mapping to match the strings your Arduino sketch expects.
    #             # Default mapping from single-letter gesture codes to Arduino command strings.
    #             # Edit these values to exactly match the strings your Arduino sketch checks for.
    #             # code_map = {
    #             #     'R': 'BN_GREEN',   # example: want_rice -> Bắc-Nam green
    #             #     'D': 'BN_GREEN',   # want_drink -> Bắc-Nam green
    #             #     'C': 'DT_GREEN',   # change_dish -> Đông-Tây green
    #             #     'E': 'DT_GREEN',   # dessert -> Đông-Tây green
    #             #     'S': 'ALL_RED',    # satisfied -> turn all red (example)
    #             #     'N': 'ALL_RED',    # not_satisfied -> all red (example)
    #             #     'T': 'ALL_RED',    # ask_time -> all red (example)
    #             #     'L': 'ALL_RED',    # clear_tray -> all red (example)
    #             #     '0': 'ALL_RED'     # no hand -> all red
    #             # }

    #             # If code is a single-character short code, translate it.
    #             cmd = None
    #             if isinstance(code, str) and len(code) == 1:
    #                 cmd = code_map.get(code, code)
    #             else:
    #                 cmd = code

    #             # Ensure we send newline-terminated commands so Arduino's readStringUntil('\n') works.
    #             if isinstance(cmd, str) and not cmd.endswith('\n'):
    #                 cmd = cmd + '\n'

    #             # Write to serial
    #             self.ser.write(cmd.encode('utf-8') if isinstance(cmd, str) else cmd)
    #             self.led_status = (code != '0')
    #             return True
    #         except Exception as e:
    #             print(f"Serial error: {e}")
    #     return False

    def _send_serial(self, code):
        """Send code to Arduino"""
        if self.ser and self.ser.is_open:
            try:
                cmd = code + '\n'  # Ensure command is newline-terminated
                print(f">>> Sending to Arduino: '{cmd.strip()}'")
                self.ser.write(cmd.encode('utf-8'))
                self.led_status = (code != '0')
                return True
            except Exception as e:
                print(f"Serial error: {e}")
        return False
    
    def _process_frame(self, frame):
        """Process single frame"""
        with self.lock:
            # Store frame for streaming
            _, jpeg = cv2.imencode('.jpg', frame)
            self.current_frame = base64.b64encode(jpeg).decode()
            self.current_timestamp = datetime.now().isoformat()
    
    def run(self):
        """Main processing loop"""
        if not self.initialize():
            return
        
        self.is_running = True
        
        while self.is_running and self.cap.isOpened():
            ret, frame = self.cap.read()
            if not ret:
                break
            
            # Flip for better UX
            frame = cv2.flip(frame, 1)
            rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
            results = self.hands.process(rgb_frame)
            
            # Draw black rectangle for results display
            cv2.rectangle(frame, (0, frame.shape[0]-150), (frame.shape[1], frame.shape[0]), (0,0,0), -1)
            
            if results.multi_hand_landmarks:
                self.hand_absent_frames = 0
                
                for hand_landmarks in results.multi_hand_landmarks:
                    self.mp_draw.draw_landmarks(frame, hand_landmarks, mp_hands.HAND_CONNECTIONS)
                
                keypoints = self._extract_keypoints(results)
                self.frames_buffer.append(keypoints)
                
                # Display frame count
                cv2.putText(frame, f"Frames: {len(self.frames_buffer)}/{FRAMES_TO_CAPTURE}", 
                           (10, 30), cv2.FONT_HERSHEY_SIMPLEX, 0.7, (0, 255, 0), 2)
                
                if len(self.frames_buffer) == FRAMES_TO_CAPTURE and self.cooldown == 0:
                    input_data = np.array(list(self.frames_buffer))
                    input_data = np.expand_dims(input_data, axis=0)
                    input_data = input_data.reshape((1, FRAMES_TO_CAPTURE, 63, 1, 1))
                    
                    prediction = self.model.predict(input_data, verbose=0)
                    confidence = float(np.max(prediction))
                    predicted_action = actions[np.argmax(prediction)]
                    
                    if confidence >= CONFIDENCE_THRESHOLD:
                        gesture_name = action_labels[predicted_action]
                        code = gesture_codes.get(predicted_action, '?')
                        
                        with self.lock:
                            self.current_gesture = gesture_name
                            self.current_confidence = confidence * 100
                        
                        self.save_gesture_to_db(gesture_name, code, confidence * 100)
                        
                        # Send to Arduino
                        if code != self.last_sent_code:
                            self._send_serial(code)
                            self.last_sent_code = code
                        
                        if not self.results_list or gesture_name != self.results_list[-1]:
                            self.results_list.append(gesture_name)
                            self.cooldown = COOLDOWN_FRAMES
                            self.frames_buffer.clear()
            
            else:
                # No hands detected
                self.hand_absent_frames += 1
                self.frames_buffer.clear()
                
                if self.hand_absent_frames == HAND_ABSENT_THRESHOLD:
                    if self.last_sent_code is not None:
                        self._send_serial('0')
                        self.last_sent_code = None
                
                cv2.putText(frame, "Khong tay!", (10, 30), 
                           cv2.FONT_HERSHEY_SIMPLEX, 0.7, (0, 0, 255), 2)
                
                with self.lock:
                    self.current_gesture = None
                    self.current_confidence = 0.0
            
            # Display results
            y_pos = frame.shape[0] - 120
            cv2.putText(frame, "Ket qua nhan dang:", (10, y_pos), 
                       cv2.FONT_HERSHEY_SIMPLEX, 0.8, (255, 255, 255), 2)
            
            for i, result in enumerate(self.results_list):
                y_pos += 40
                cv2.putText(frame, f"{i+1}. {result}", (30, y_pos), 
                           cv2.FONT_HERSHEY_SIMPLEX, 0.8, (255, 255, 255), 2)
            
            # Reduce cooldown
            if self.cooldown > 0:
                self.cooldown -= 1
            
            # Store frame for streaming
            self._process_frame(frame)
            
            # Small delay
            if cv2.waitKey(10) & 0xFF == ord('q'):
                break
        
        self.stop()
    
    def stop(self):
        """Stop processing"""
        self.is_running = False
        
        self.end_session()
        
        self.update_device_status('Offline')
        
        if self.cap:
            self.cap.release()
        if self.ser and self.ser.is_open:
            self.ser.close()
        cv2.destroyAllWindows()

# Initialize global engine
engine = GestureEngine()
engine_thread = None

# ═══════════════════════════════════════════════════════════
# API ENDPOINTS
# ═══════════════════════════════════════════════════════════

@app.route('/api/start', methods=['POST'])
def start_camera():
    """Start camera processing"""
    global engine_thread
    
    data = request.json or {}
    com_port = data.get('com_port', 'COM3')
    baudrate = data.get('baudrate', 9600)
    location = data.get('location', f'Auto_{datetime.now().strftime("%Y%m%d_%H%M%S")}')
    
    print(f"\n🚀 /api/start called")
    print(f"   COM Port: {com_port}")
    print(f"   BaudRate: {baudrate}")
    print(f"   Location: {location}")
    
    if engine.is_running:
        return jsonify({'status': 'error', 'message': 'Already running'})
    
    if engine_thread and engine_thread.is_alive():
        time.sleep(1)  # Chờ thread cũ kết thúc
    
    engine.is_running = False
    engine.frames_buffer.clear()
    engine.results_list.clear()
    engine.cooldown = 0
    engine.hand_absent_frames = 0
    engine.last_sent_code = None
    
    engine._initialize_serial(com_port, baudrate)
    
    engine.create_session(location)
    
    engine_thread = threading.Thread(target=engine.run, daemon=True)
    engine_thread.start()
    
    return jsonify({'status': 'success', 'message': 'Camera started'})

@app.route('/api/stop', methods=['POST'])
def stop_camera():
    """Stop camera processing"""
    engine.stop()
    return jsonify({'status': 'success', 'message': 'Camera stopped'})

@app.route('/api/frame', methods=['GET'])
def get_frame():
    """Get current frame and gesture info"""
    with engine.lock:
        return jsonify({
            'frame': engine.current_frame,
            'gesture': engine.current_gesture,
            'confidence': round(engine.current_confidence, 2),
            'led_status': engine.led_status,
            'timestamp': engine.current_timestamp
        })

@app.route('/api/status', methods=['GET'])
def get_status():
    """Get engine status"""
    return jsonify({
        'running': engine.is_running,
        'gesture': engine.current_gesture,
        'confidence': round(engine.current_confidence, 2),
        'led_status': engine.led_status
    })

@app.route('/health', methods=['GET'])
def health():
    """Health check"""
    return jsonify({'status': 'ok'})

if __name__ == '__main__':
    print("Starting Gesture Recognition API Server...")
    app.run(host='127.0.0.1', port=5000, debug=False, threaded=True)
