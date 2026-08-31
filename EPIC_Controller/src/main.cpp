#include <Arduino.h>
#include <WiFi.h>
#include <PubSubClient.h>
       
// ULN2003 드라이버 보드의 핀 연결 설정
#define JOY1X 36
#define JOY1Y 39
#define JOY2X 34
#define JOY2Y 35
#define JOY3X 32
#define JOY3Y 33
#define BTN1 23
#define BTN2 22
#define BTN3 21
#define BTN5 18
#define BTN6 17

const char* ssid = "popcorn";
const char* password = "11213144";
const char* mqtt_server = "192.168.0.5";
// const char* ssid = "bssm_free";
// const char* password = "bssm_free";
// const char* mqtt_server = "10.150.1.8";

WiFiClient espClient;
PubSubClient client(espClient);
unsigned long lastMsg = 0;
#define MSG_BUFFER_SIZE   (50)
char msg[MSG_BUFFER_SIZE];
int value = 0;

void setup_wifi() {

  delay(10);
  // We start by connecting to a WiFi network
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);

  WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }

  randomSeed(micros());

  Serial.println("");
  Serial.println("WiFi connected");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());
}
//esp32 데이터 수신부
void callback(char* topic, byte* payload, unsigned int length) {
  Serial.print("Message arrived [");
  Serial.print(topic);
  Serial.print("] ");
  for (int i = 0; i < length; i++) {
    Serial.print((char)payload[i]);
  }
  Serial.println();
/*
  if(payload[0] == '1'){
    digitalWrite(BTN7, 1);
  }
  else{
    digitalWrite(BTN7, 0);
  }
*/
}

void reconnect() {
  // Loop until we're reconnected
  while (!client.connected()) {
    Serial.print("Attempting MQTT connection...");
    String clientId = "ESP8266Client-";
    clientId += String(random(0xffff), HEX);
    if (client.connect(clientId.c_str())) {
    } else {
      Serial.print("failed, rc=");
      Serial.print(client.state());
      Serial.println(" try again in 5 seconds");
      delay(5000);
    }
  }
}

unsigned long t = 0;

// AccelStepper 객체 생성
// AccelStepper(step mode, pin1, pin2, pin3, pin4)

void setup() {
  Serial.begin(115200);
  
  setup_wifi();
  client.setServer(mqtt_server, 1883);
  client.setCallback(callback);
  pinMode(BTN1, INPUT);
  pinMode(BTN2, INPUT);
  pinMode(BTN3, INPUT);
  pinMode(BTN5, INPUT);
  pinMode(BTN6, INPUT);

  // 최대 속도와 가속도 설정
}

void loop() {
    if (!client.connected()) {
    reconnect();
  }
  client.loop();

  unsigned long now = millis();
  if (now - lastMsg > 100) {
    lastMsg = now;
    
    int a = 0;
    int b = 0;
    int a2 = 0;
    int b2 = 0;
    int a3 = 0;
    int b3 = 0;
    int btn1 = digitalRead(BTN1) == LOW ? 1 : 0;
    int btn2 = digitalRead(BTN2) == LOW ? 1 : 0;
    int btn3 = digitalRead(BTN3) == LOW ? 1 : 0;
    int btn5 = digitalRead(BTN5) == LOW ? 0 : 1;
    int btn6 = digitalRead(BTN6) == LOW ? 0 : 1;

    int controlx = analogRead(JOY1X);
    int controly = analogRead(JOY1Y);
    int controlx2 = analogRead(JOY2X);
    int controly2 = analogRead(JOY2Y);
    int controlx3 = analogRead(JOY3X);
    int controly3 = analogRead(JOY3Y);

    if(controlx > 3000){
      a=1;
    }else if(controlx < 1000){
      a=2;
    }else{
      a=0;
    }
    if(controly > 3000){
      b=1;
    }else if(controly < 1000){
      b=2;
    }else{
      b=0;
    }
    if(controlx2 > 3000){
      a2=1;
    }else if(controlx2 < 1000){
      a2=2;
    }else{
      a2=0;
    }
    if(controly2 > 3000){
      b2=1;
    }else if(controly2 < 1000){
      b2=2;
    }else{
      b2=0;
    }
    if(controlx3 > 3000){
      a3=1;
    }else if(controlx3 < 1000){
      a3=2;
    }else{
      a3=0;
    }
    if(controly3 > 3000){
      b3=1;
    }else if(controly3 < 1000){
      b3=2;
    }else{
      b3=0;
    }
    
    String data = String(b) + String(a) + String(b2) + String(a2) + String(b3) + String(a3) + String(btn1) + String(btn2) + String(btn3) + String(btn5) + String(btn6);
    //String != char*

    Serial.print("Publish message: ");
    Serial.println(data);

    client.publish("haro/controller1/state", data.c_str());
  }
}