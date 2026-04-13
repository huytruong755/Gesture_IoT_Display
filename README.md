# Hệ Thống Nhận Diện Cử Chỉ Tay 3D

## 1. Giới Thiệu Tổng Quan

Hệ thống nhận diện cử chỉ tay AI điều khiển LED Matrix qua Arduino.

**Luồng dữ liệu:**
```
📷 Camera → 🤖 Python AI → COM3 → 🔌 VSPE (COM3↔COM4) → 💻 COM4 → 🖥️ Proteus → ⚙️ Arduino → 💡 LED Matrix
```

**8 cử chỉ hỗ trợ:** `want_rice` (R), `want_drink` (D), `change_dish` (C), `dessert` (E), `satisfied` (S), `not_satisfied` (N), `ask_time` (T), `clear_tray` (L)

---

## 2. Yêu Cầu Phần Cứng

- 📷 Webcam USB
- 💻 Máy tính RAM ≥ 8GB
- ⚡ Arduino UNO
- 💡 MAX7219 + LED Matrix 8x8
- 🔌 Cáp USB, breadboard, resistor, tụ điện

---

## 3. Yêu Cầu Phần Mềm

**Python packages:** tensorflow, opencv-python, mediapipe, numpy, pyserial

**Phần mềm:** Python 3.8+, Arduino IDE, Proteus 8.x, VSPE (Virtual Serial Port Emulator)

---

## 4. Cài Đặt Python

- Mở PowerShell tại thư mục dự án `d:\document\cuChiTay`
- Tạo Virtual Environment: `python -m venv venv`
- Kích hoạt (Windows): `.\venv\Scripts\Activate.ps1`
- Cài packages: `pip install tensorflow opencv-python mediapipe numpy pyserial`

---

## 5. Cài Đặt VSPE (Virtual Serial Port Emulator)

1. **Tải VSPE** từ: http://www.virtual-serial-port.org/ (hoặc từ trang chủ)
2. **Cài đặt** VSPE trên máy
3. **Mở VSPE** với quyền **Administrator**
4. Vào **Device → Create new → Pair**
5. **Nhập 2 cổng**: COM3 ↔ COM4
6. Nhấn **Finish**
7. Nhấn **Start (Play)** để kích hoạt cặp cổng ảo
8. **Lưu cấu hình**: File → Save (.vspe)
9. Kiểm tra Device Manager → Ports sẽ hiển thị COM3 & COM4

**Lưu ý:** Nếu lỗi, kiểm tra xem COM3/COM4 có bị trùng hoặc đang bị chiếm bởi thiết bị khác không

---

## 6. Cài Đặt Proteus COMPIM

1. **Mở Proteus** → **File** → **New Design**
2. **Thêm COMPIM component** (Device → COMPIM)
3. **Double-click COMPIM** → Configure:
   - **Port Configuration**: `COM4` (nhận từ Python qua VSPE)
   - **Baud Rate**: `9600`
   - Click **OK**
4. **Thêm Arduino UNO** component vào sơ đồ
5. **Thêm MAX7219** component
6. **Kết nối RX/TX**: COMPIM RX → Arduino RX, COMPIM TX → Arduino TX
7. **Kết nối MAX7219**: D10→CS, D11→DIN, D13→CLK (từ Arduino)

---

## 7. Cài Đặt Arduino IDE

1. **Tải Arduino IDE** từ: https://www.arduino.cc/en/software
2. **Cài đặt** Arduino IDE bình thường
3. **Mở Arduino IDE**
4. **Chọn Board**: Tools → Board → Arduino AVR Boards → **Arduino UNO**
5. **Viết code** xử lý nhận dữ liệu từ Serial và điều khiển MAX7219:
   - Sử dụng `Serial.begin(9600)` để lắng nghe COM port
   - Sử dụng `if(Serial.available())` để kiểm tra dữ liệu nhận
   - Sử dụng thư viện `LedControl.h` để điều khiển MAX7219
6. **Upload code**: Tools → Port → Chọn port Arduino (COM6+ - KHÔNG phải COM3/COM4) → Nhấn Upload

---

## 8. Kết Nối Phần Cứng MAX7219 & LED Matrix

**Sơ đồ kết nối:**
- Arduino 5V → MAX7219 VCC
- Arduino GND → MAX7219 GND
- Arduino D10 → MAX7219 CS (Chip Select)
- Arduino D11 → MAX7219 DIN (Data In)
- Arduino D13 → MAX7219 CLK (Clock)
- MAX7219 Output → LED Matrix 8x8

**Linh kiện cần thiết:**
- MAX7219 IC (1 cái)
- LED Matrix 8x8 (1 cái)
- Resistor 10kΩ (2 cái)
- Tụ 100nF, 10µF (để lọc nhiễu)
- Breadboard + dây cáp

---

## 9. Chạy Hệ Thống Hoàn Chỉnh

**Thứ tự khởi động:**

1. **Khởi động VSPE**: Mở VSPE → Nhấn **Play** → COM3↔COM4 đang chạy
2. **Khởi động Proteus**: Mở Project Proteus → Nhấn **Play** (simulation) → COMPIM lắng nghe trên COM4
3. **Chạy Python AI**: Kích hoạt venv rồi chạy `python RealTime3D.py`
4. **Kiểm tra luồng dữ liệu**:
   - Camera nhìn thấy cử chỉ → Python AI nhận diện
   - Python gửi mã via COM3 → VSPE chuyển sang COM4
   - Proteus COMPIM nhận dữ liệu → gửi cho Arduino
   - Arduino xử lý → điều khiển MAX7219
   - LED Matrix hiển thị pattern

**Port mapping quan trọng:**
- Python gửi dữ liệu: **COM3**
- Proteus COMPIM nhận: **COM4**
- Arduino upload code: **COM6+** (port khác, không phải COM3/COM4)
- **Baud Rate**: **9600** (tất cả phải cùng)

---
