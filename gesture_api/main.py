from fastapi import FastAPI
from gesture_api.database import create_tables
from gesture_api.routers import auth, gesture, serial

app = FastAPI(title="Gesture API", version="1.0")

@app.on_event("startup")
def startup():
    create_tables()

@app.get("/")
def read_root():
    return {"message": "Gesture API is running", "docs": "/docs"}

app.include_router(auth.router)
app.include_router(gesture.router)
app.include_router(serial.router)