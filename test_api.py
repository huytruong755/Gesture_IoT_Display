"""
Test script để kiểm tra Gesture Recognition API
Chạy sau khi khởi động gesture_api.py
"""
import requests
import json
import time
import base64
import cv2

BASE_URL = "http://127.0.0.1:5000"
API_TIMEOUT = 5

def test_health():
    """Test health check"""
    print("\n=== Test Health Check ===")
    try:
        response = requests.get(f"{BASE_URL}/health", timeout=API_TIMEOUT)
        if response.status_code == 200:
            print("✓ API Server is healthy")
            return True
        else:
            print(f"✗ Server returned status {response.status_code}")
            return False
    except requests.exceptions.ConnectionError:
        print("✗ Cannot connect to API server")
        print("  Make sure gesture_api.py is running")
        return False
    except Exception as e:
        print(f"✗ Error: {e}")
        return False

def test_start():
    """Test start camera"""
    print("\n=== Test Start Camera ===")
    try:
        payload = {
            "com_port": "COM3",
            "baudrate": 9600
        }
        response = requests.post(
            f"{BASE_URL}/api/start",
            json=payload,
            timeout=API_TIMEOUT
        )
        result = response.json()
        print(f"Status: {result.get('status')}")
        print(f"Message: {result.get('message')}")
        return response.status_code == 200
    except Exception as e:
        print(f"✗ Error: {e}")
        return False

def test_get_frame(count=5):
    """Test get frame multiple times"""
    print(f"\n=== Test Get Frame ({count} times) ===")
    try:
        for i in range(count):
            response = requests.get(f"{BASE_URL}/api/frame", timeout=API_TIMEOUT)
            data = response.json()
            
            gesture = data.get('gesture') or 'None'
            confidence = data.get('confidence', 0)
            led_status = "ON" if data.get('led_status') else "OFF"
            
            print(f"[{i+1}] Gesture: {gesture:30s} | Confidence: {confidence:6.2f}% | LED: {led_status}")
            
            time.sleep(1)
        return True
    except Exception as e:
        print(f"✗ Error: {e}")
        return False

def test_get_status():
    """Test get status"""
    print("\n=== Test Get Status ===")
    try:
        response = requests.get(f"{BASE_URL}/api/status", timeout=API_TIMEOUT)
        data = response.json()
        
        print(f"Running: {data.get('running')}")
        print(f"Gesture: {data.get('gesture') or 'None'}")
        print(f"Confidence: {data.get('confidence')}%")
        print(f"LED Status: {data.get('led_status')}")
        
        return True
    except Exception as e:
        print(f"✗ Error: {e}")
        return False

def test_stop():
    """Test stop camera"""
    print("\n=== Test Stop Camera ===")
    try:
        response = requests.post(f"{BASE_URL}/api/stop", timeout=API_TIMEOUT)
        result = response.json()
        print(f"Status: {result.get('status')}")
        print(f"Message: {result.get('message')}")
        return response.status_code == 200
    except Exception as e:
        print(f"✗ Error: {e}")
        return False

def main():
    print("╔════════════════════════════════════════════════════════╗")
    print("║  Gesture Recognition API - Test Suite                ║")
    print("╚════════════════════════════════════════════════════════╝")
    
    print(f"API URL: {BASE_URL}")
    print(f"Timeout: {API_TIMEOUT}s")
    
    # Test 1: Health check
    if not test_health():
        print("\n✗ API server is not running!")
        print("  Please start gesture_api.py first:")
        print("  python gesture_api.py")
        return
    
    # Test 2: Start camera
    if not test_start():
        print("\n✗ Failed to start camera")
        return
    
    print("\n⏳ Waiting 2 seconds for camera to initialize...")
    time.sleep(2)
    
    # Test 3: Get status
    test_get_status()
    
    # Test 4: Get frames
    test_get_frame(count=5)
    
    # Test 5: Stop camera
    test_stop()
    
    print("\n" + "="*60)
    print("✓ All tests completed!")
    print("="*60)

if __name__ == "__main__":
    try:
        main()
    except KeyboardInterrupt:
        print("\n\n✗ Test interrupted by user")
    except Exception as e:
        print(f"\n✗ Unexpected error: {e}")
