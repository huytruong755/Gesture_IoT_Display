import serial

for p in ['COM3', 'COM4']:
    try:
        ser = serial.Serial(p, 9600, timeout=1)
        print(f"✓ {p} mở được")
        ser.close()
    except Exception as e:
        print(f"✗ {p}: {e}")