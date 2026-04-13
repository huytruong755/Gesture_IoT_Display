import cv2
import numpy as np
import os
from tensorflow.keras.models import Sequential
from tensorflow.keras.layers import Conv3D, MaxPooling3D, Flatten, Dense, Dropout, Input
from tensorflow.keras.optimizers import Adam
import mediapipe as mp
from collections import deque
import serial
import time

# Parameters
FRAMES_TO_CAPTURE = 20
CONFIDENCE_THRESHOLD = 0.4
MAX_RESULTS = 3

# Mở serial port - thay COM3 bằng cổng của bạn nếu cần
try:
    ser = serial.Serial('COM3', 9600, timeout=1)
    time.sleep(0.5)  # chờ port ổn định
    print(f"✓ Mở serial port thành công: {ser.name}")
except Exception as e:
    print(f"✗ Lỗi mở serial port COM3: {e}")
    print("  Hãy kiểm tra: 1) COM port đúng? 2) com0com đã chạy? 3) thiết bị khác dùng port?")
    ser = None
actions = np.array([
    'want_rice', 'want_drink', 'change_dish', 'dessert',
    'satisfied', 'not_satisfied', 'ask_time', 'clear_tray'
])

# Dictionary ánh xạ tên hành động sang tiếng Việt
action_labels = {
    'want_rice': 'Muon an com',
    'want_drink': 'Muon uong nuoc',
    'change_dish': 'Muon đoi mon an',
    'dessert': 'Mon trang mieng',
    'satisfied': 'Hai long mon an',
    'not_satisfied': 'Khong hai long',
    'ask_time': 'Muon hoi may gio',
    'clear_tray': 'Muon phuc vu don khay'
}

# Map action name → mã ký tự cho Arduino
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

# Rebuild model with compatible architecture
def build_model():
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

# Load model
model = build_model()
try:
    model.load_weights('model3d.h5')
except Exception as e:
    print(f"Warning: Could not load weights from model3d.h5: {e}")
    print("Using untrained model weights instead")

# Initialize MediaPipe
mp_hands = mp.solutions.hands
hands = mp_hands.Hands(
    static_image_mode=False,
    max_num_hands=2,
    min_detection_confidence=0.5,
    min_tracking_confidence=0.5
)
mp_draw = mp.solutions.drawing_utils

# Initialize camera
cap = cv2.VideoCapture(0)
if not cap.isOpened():
    print("Khong mo duoc camera")
    exit()

# Initialize variables
frames_buffer = deque(maxlen=FRAMES_TO_CAPTURE)
results_list = deque(maxlen=MAX_RESULTS)
cooldown = 0
COOLDOWN_FRAMES = 5

def extract_keypoints(results):
    if results.multi_hand_landmarks:
        hand = results.multi_hand_landmarks[0]
        return np.array([[res.x, res.y, res.z] for res in hand.landmark]).flatten()
    return np.zeros(63)

while cap.isOpened():
    ret, frame = cap.read()
    if not ret:
        break

    # Xử lý frame
    rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
    results = hands.process(rgb_frame)

    # Vẽ khung đen cho phần hiển thị kết quả
    cv2.rectangle(frame, (0, frame.shape[0]-150), (frame.shape[1], frame.shape[0]), (0,0,0), -1)
    
    if results.multi_hand_landmarks:
        for hand_landmarks in results.multi_hand_landmarks:
            mp_draw.draw_landmarks(frame, hand_landmarks, mp_hands.HAND_CONNECTIONS)
        
        keypoints = extract_keypoints(results)
        frames_buffer.append(keypoints)
        
        # Hiển thị số frame đã thu thập
        cv2.putText(frame, f"Frames: {len(frames_buffer)}/{FRAMES_TO_CAPTURE}", 
                    (10, 30), cv2.FONT_HERSHEY_SIMPLEX, 0.7, (0, 255, 0), 2)
        
        if len(frames_buffer) == FRAMES_TO_CAPTURE and cooldown == 0:
            input_data = np.array(list(frames_buffer))
            input_data = np.expand_dims(input_data, axis=0)
            input_data = input_data.reshape((1, FRAMES_TO_CAPTURE, 63, 1, 1))
            
            prediction = model.predict(input_data, verbose=0)
            confidence = np.max(prediction)
            predicted_action = actions[np.argmax(prediction)]
            
            if confidence >= CONFIDENCE_THRESHOLD:
                result = action_labels[predicted_action]
                code = gesture_codes.get(predicted_action, '?')
                # Gửi mã lên COM3 khi nhận diện được
                if ser and ser.is_open:
                    try:
                        ser.write(code.encode())
                        print(f"Gửi mã: {code} ({predicted_action}) - Confidence: {confidence:.2f}")
                    except Exception as e:
                        print(f"Lỗi gửi serial: {e}")
                else:
                    print(f"⚠ Serial port không mở. Mã không được gửi: {code}")
                
                if not results_list or result != results_list[-1]:
                    results_list.append(result)
                    cooldown = COOLDOWN_FRAMES
                    frames_buffer.clear()
    else:
        frames_buffer.clear()
        cv2.putText(frame, "Khong tay!", (10, 30), 
                    cv2.FONT_HERSHEY_SIMPLEX, 0.7, (0, 0, 255), 2)

    # Hiển thị kết quả
    y_pos = frame.shape[0] - 120  # Vị trí bắt đầu hiển thị
    cv2.putText(frame, "Ket qua nhan dang:", (10, y_pos), 
                cv2.FONT_HERSHEY_SIMPLEX, 0.8, (255, 255, 255), 2)
    
    for i, result in enumerate(results_list):
        y_pos += 40
        cv2.putText(frame, f"{i+1}. {result}", (30, y_pos), 
                    cv2.FONT_HERSHEY_SIMPLEX, 0.8, (255, 255, 255), 2)

    # Giảm cooldown
    if cooldown > 0:
        cooldown -= 1

    cv2.imshow('Nhan dang ky hieu', frame)
    
    # Xóa kết quả khi nhấn Space
    if cv2.waitKey(1) & 0xFF == ord(' '):
        results_list.clear()
        frames_buffer.clear()
        cooldown = 0
    
    # Thoát khi nhấn Q
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

cap.release()
cv2.destroyAllWindows()

# Đóng serial port
if ser and ser.is_open:
    ser.close()
    print("✓ Đã đóng serial port")

hands.close()