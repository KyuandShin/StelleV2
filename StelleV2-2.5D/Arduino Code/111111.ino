#include "Wire.h"
#include "I2Cdev.h"
#include "MPU6050.h"

MPU6050 accelgyro;

int16_t ax, ay, az;
int16_t gx, gy, gz;

#define LED_PIN 13
bool blinkState = false;
int a=1;
int b=1;
int c=0;
int d=0;
void setup() {
    Wire.begin();
    Serial.begin(9600);
    accelgyro.initialize();
    pinMode(LED_PIN, OUTPUT);
}

void loop() {
    accelgyro.getMotion6(&ax, &ay, &az, &gx, &gy, &gz);
    //Serial.print("a/g:\t");
    //Serial.print(ax*2*9.8/32768); Serial.print("\t");
    //Serial.print(ay*2*9.8/32768); Serial.print("\t");
    //Serial.print(az*2*9.8/32768); Serial.print("\t");
    //Serial.print(gx*90/32768); Serial.print("\t");
    //Serial.print(gy*90/32768); Serial.print("\t");
    //Serial.println(gz*90/32768);
    if(ax*2*9.8/32768>5)
    a=0;
    if(ax*2*9.8/32768<-3)
    a=2;
    if(ax*2*9.8/32768>-3&&ax*2*9.8/32768<5)
    a=1;
    if(ay*2*9.8/32768>5)
    b=0;
    if(ay*2*9.8/32768<-5)
    b=2;
    if(ay*2*9.8/32768>-5&&ay*2*9.8/32768<5)
    b=1;
    if(analogRead(A0)>1000)
    c=1;
    else
    c=0;
    if(analogRead(A1)>990)
    d=1;
    else
    d=0;
    
    blinkState = !blinkState;
    digitalWrite(LED_PIN, blinkState);
    Serial.print(a);
    Serial.print(",");
    Serial.print(b);
    Serial.print(",");
    Serial.print(c);
    Serial.print(",");
    Serial.println(d);
 
    delay(200);
}
