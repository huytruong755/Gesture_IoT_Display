#include <MD_MAX72xx.h>
#include <SPI.h>

#define HARDWARE_TYPE MD_MAX72XX::PAROLA_HW
#define MAX_DEVICES 4
#define CLK_PIN 13
#define DATA_PIN 11
#define CS_PIN 10
#define MAX_REPEAT 3

MD_MAX72XX mx = MD_MAX72XX(HARDWARE_TYPE, CS_PIN, MAX_DEVICES);

// ===== Scroll state machine =====
const char* scrollText = nullptr;   // con trỏ text đang scroll
uint8_t     charIdx    = 0;         // ký tự hiện tại trong chuỗi
uint8_t     colIdx     = 0;         // cột hiện tại trong ký tự
uint8_t     charBuf[8];
uint8_t     charWidth  = 0;
bool        scrolling  = false;
uint32_t    lastTick   = 0;
uint8_t scrollRepeat = 0;

#define SCROLL_DELAY 80  // ms mỗi cột dịch — tăng để chậm hơn

// ===== Gesture table =====
struct GestureMap { char code; const char* text; };
GestureMap gestures[] = {
  {'R', "Muon an com"},
  {'D', "Muon uong nuoc"},
  {'C', "Muon doi mon an"},
  {'E', "Mon trang mieng"},
  {'S', "Hai long"},
  {'N', "Khong hai long"},
  {'T', "May gio roi?"},
  {'L', "Don khay"}
};
#define GESTURE_COUNT (sizeof(gestures)/sizeof(gestures[0]))

// ===== Khởi động scroll mới =====
void startScroll(const char* text) {
  mx.clear();
  scrollText = text;
  charIdx    = 0;
  colIdx     = 0;
  charWidth  = 0;
  scrolling  = true;
  scrollRepeat = 0;
  // Load ký tự đầu tiên ngay
  if (scrollText && scrollText[0] != '\0') {
    charWidth = mx.getChar(scrollText[0], 8, charBuf);
  }
}

// ===== Gọi mỗi loop — không blocking =====
void tickScroll() {
  if (!scrolling || !scrollText) return;
  if (millis() - lastTick < SCROLL_DELAY) return;
  lastTick = millis();

  // Dịch LED sang trái 1 cột
  mx.transform(MD_MAX72XX::TSL);

  // Đặt cột mới nhất
  if (charIdx < strlen(scrollText)) {
    if (colIdx < charWidth) {
      mx.setColumn(0, charBuf[colIdx]);
      colIdx++;
    } else {
      // Khoảng cách giữa ký tự (1 cột trắng)
      mx.setColumn(0, 0);
      charIdx++;
      colIdx = 0;
      if (charIdx < strlen(scrollText)) {
        charWidth = mx.getChar(scrollText[charIdx], 8, charBuf);
      }
    }
  } else {
    // Hết chuỗi — scroll thêm 32 cột trắng để text thoát ra
    static uint8_t blankCount = 0;
    mx.setColumn(0, 0);
    blankCount++;
    if (blankCount >= 32) {
  blankCount = 0;
  scrollRepeat++;

  if (scrollRepeat < MAX_REPEAT) {
    // Chạy lại từ đầu
    charIdx = 0;
    colIdx  = 0;
    if (scrollText && scrollText[0] != '\0') {
      charWidth = mx.getChar(scrollText[0], 8, charBuf);
    }
  } else {
    // Đủ 3 lần thì dừng
    scrolling  = false;
    mx.clear();
  }
}
  }
}

// ===== Xử lý mã nhận =====
void handleCode(char code) {
  if (code == 'X') {          // Lệnh dừng từ Python
    scrolling  = false;
    scrollText = nullptr;
    mx.clear();
    Serial.println("STOP: LED cleared");
    return;
  }
  for (uint8_t i = 0; i < GESTURE_COUNT; i++) {
    if (gestures[i].code == code) {
      Serial.print("START: "); Serial.println(gestures[i].text);
      startScroll(gestures[i].text);  // Ghi đè ngay lập tức
      return;
    }
  }
}

void setup() {
  mx.begin();
  mx.clear();
  Serial.begin(9600);
  Serial.println("Ready");
  startScroll("Ready!");
}

void loop() {
  // Đọc serial — luôn ưu tiên, không bị block bởi scroll
  if (Serial.available() > 0) {
    char c = Serial.read();
    if (c >= 32 && c <= 126) handleCode(c);
  }

  // Tick scroll không blocking
  tickScroll();
}
