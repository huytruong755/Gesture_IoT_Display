import serial
import serial.tools.list_ports

_connection: serial.Serial | None = None

def list_ports() -> list[str]:
    return [p.device for p in serial.tools.list_ports.comports()]

def connect(port: str, baudrate: int = 9600) -> bool:
    global _connection
    try:
        if _connection and _connection.is_open:
            _connection.close()
        _connection = serial.Serial(port, baudrate, timeout=1)
        return True
    except Exception as e:
        print(f"Serial error: {e}")
        return False

def send_text(text: str) -> bool:
    """Gửi chuỗi ký tự đến Arduino để hiển thị LED"""
    if _connection and _connection.is_open:
        _connection.write((text + "\n").encode("utf-8"))
        return True
    return False

def disconnect():
    global _connection
    if _connection and _connection.is_open:
        _connection.close()