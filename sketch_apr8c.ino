#include <MD_MAX72xx.h>
#include <SPI.h>

// Turn on debug statements to the serial output
#define DEBUG 1

#if DEBUG
#define PRINT(s, x) { Serial.print(F(s)); Serial.print(x); }
#define PRINTS(x) Serial.print(F(x))
#define PRINTD(x) Serial.println(x, DEC)
#else
#define PRINT(s, x)
#define PRINTS(x)
#define PRINTD(x)
#endif

// Define hardware interface for MAX7219
#define HARDWARE_TYPE MD_MAX72XX::FC16_HW
#define MAX_DEVICES 4
#define CLK_PIN 13  // or SCK
#define DATA_PIN 11 // or MOSI
#define CS_PIN 10   // or SS

// SPI hardware interface
MD_MAX72XX mx = MD_MAX72XX(HARDWARE_TYPE, CS_PIN, MAX_DEVICES);

// Display timing
#define DELAYTIME 100  // in milliseconds

// Gesture mapping: code -> Vietnamese text
struct GestureMap {
  char code;
  const char* text;
};

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

#define GESTURE_COUNT (sizeof(gestures) / sizeof(gestures[0]))

// Function to scroll text on LED Matrix
void scrollText(const char *p)
{
  uint8_t charWidth;
  uint8_t cBuf[8];  // Buffer for character bitmap

  mx.clear();

  while (*p != '\0')
  {
    charWidth = mx.getChar(*p++, sizeof(cBuf) / sizeof(cBuf[0]), cBuf);

    for (uint8_t i = 0; i <= charWidth; i++)  // Allow space between characters
    {
      mx.transform(MD_MAX72XX::TSL);  // Transform: shift left
      if (i < charWidth)
        mx.setColumn(0, cBuf[i]);
      delay(DELAYTIME);
    }
  }
  
  mx.clear();  // Clear display after scrolling
}

// Function to display static text (non-scrolling)
void displayStatic(const char *p)
{
  uint8_t charWidth;
  uint8_t cBuf[8];

  mx.clear();
  uint8_t col = mx.getColumnCount() - 1;  // Start from rightmost column

  while (*p != '\0' && col > 0)
  {
    charWidth = mx.getChar(*p++, sizeof(cBuf) / sizeof(cBuf[0]), cBuf);
    for (uint8_t i = 0; i < charWidth && col > 0; i++)
    {
      mx.setColumn(col--, cBuf[i]);
    }
  }
}

// Function to handle incoming gesture code
void handleGestureCode(char code)
{
  PRINTS("\n");
  PRINT("Received code: ", code);
  
  // Find matching gesture
  for (uint8_t i = 0; i < GESTURE_COUNT; i++)
  {
    if (gestures[i].code == code)
    {
      PRINT(" -> ", gestures[i].text);
      PRINTS("\n");
      scrollText((char*)gestures[i].text);
      return;
    }
  }
  
  // Unknown code
  PRINTS(" -> Unknown gesture\n");
  scrollText("No match?");
}

void setup()
{
  mx.begin();
  Serial.begin(9600);
  
  PRINTS("\n");
  PRINTS("MAX7219 LED Matrix Display Initialized\n");
  PRINTS("Waiting for gesture codes...\n");
  PRINTS("Expected codes: R,D,C,E,S,N,T,L\n");
  
  // Display welcome message
  scrollText("Ready!");
}

void loop()
{
  // Check if data is available on Serial port
  if (Serial.available() > 0)
  {
    char receivedChar = Serial.read();
    
    // Filter out non-printable characters
    if (receivedChar >= 32 && receivedChar <= 126)
    {
      handleGestureCode(receivedChar);
    }
  }
}
