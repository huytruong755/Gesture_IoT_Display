from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from gesture_api.database import get_db, User
from gesture_api.auth import hash_password, verify_password, create_token
from gesture_api.schemas import UserRegister, UserLogin

router = APIRouter(prefix="/auth", tags=["auth"])

@router.post("/register")
def register(user_data: UserRegister, db: Session = Depends(get_db)):
    if db.query(User).filter(User.username == user_data.username).first():
        raise HTTPException(400, "Username đã tồn tại")
    user = User(username=user_data.username, email=user_data.email, password_hash=hash_password(user_data.password))
    db.add(user)
    db.commit()
    return {"message": "Đăng ký thành công"}

@router.post("/login")
def login(user_data: UserLogin, db: Session = Depends(get_db)):
    user = db.query(User).filter(User.username == user_data.username).first()
    if not user or not verify_password(user_data.password, user.password_hash):
        raise HTTPException(401, "Sai tài khoản hoặc mật khẩu")
    token = create_token({"sub": user.username, "user_id": user.id})
    return {"access_token": token, "token_type": "bearer"}