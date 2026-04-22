from fastapi import APIRouter, Depends
from sqlalchemy.orm import Session
from gesture_api.database import get_db, GestureLog
from gesture_api.auth import decode_token
from gesture_api.serial_manager import send_text
from gesture_api.schemas import GestureSend

router = APIRouter(prefix="/gesture", tags=["gesture"])

@router.post("/send")
def send_gesture(gesture_data: GestureSend,
                 current_user: str = Depends(decode_token),
                 db: Session = Depends(get_db)):
    # Gửi lên LED
    send_text(gesture_data.label)
    # Lưu log
    log = GestureLog(session_id=gesture_data.session_id, gesture_label=gesture_data.label, confidence=gesture_data.confidence)
    db.add(log)
    db.commit()
    return {"status": "ok", "sent": gesture_data.label}

@router.get("/history/{session_id}")
def get_history(session_id: int, db: Session = Depends(get_db),
                current_user: str = Depends(decode_token)):
    logs = db.query(GestureLog).filter(GestureLog.session_id == session_id).all()
    return logs