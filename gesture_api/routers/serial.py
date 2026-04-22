from fastapi import APIRouter

router = APIRouter(prefix="/serial", tags=["serial"])

@router.get("/ports")
def get_available_ports():
    """Lấy danh sách cổng COM khả dụng"""
    from gesture_api.serial_manager import list_ports
    return {"ports": list_ports()}

@router.post("/connect")
def serial_connect(port: str, baudrate: int = 9600):
    """Kết nối đến cổng Com"""
    from gesture_api.serial_manager import connect
    ok = connect(port, baudrate)
    return {"connected": ok}
