#include <WiFi.h>
#include <PubSubClient.h>
#include <AccelStepper.h>
// Update these with values suitable for your network.
// ULN2003 드라이버 보드의 핀 연결 설정
// 모터 1 핀 설정
#define M1_IN1  13
#define M1_IN2  12
#define M1_IN3  14
#define M1_IN4  27

// 모터 2 핀 설정
#define M2_IN1  26
#define M2_IN2  25
#define M2_IN3  33
#define M2_IN4  32

// 모터 3 핀 설정
#define M3_IN1  16
#define M3_IN2  17
#define M3_IN3  5
#define M3_IN4  18

// 모터 4 핀 설정
#define M4_IN1  19
#define M4_IN2  21
#define M4_IN3  22
#define M4_IN4  23

#define pump 4

unsigned long t = 0;
AccelStepper stepper1(AccelStepper::HALF4WIRE, M1_IN1, M1_IN3, M1_IN2, M1_IN4);
AccelStepper stepper2(AccelStepper::HALF4WIRE, M2_IN1, M2_IN3, M2_IN2, M2_IN4);
AccelStepper stepper3(AccelStepper::HALF4WIRE, M3_IN1, M3_IN3, M3_IN2, M3_IN4);
AccelStepper stepper4(AccelStepper::HALF4WIRE, M4_IN1, M4_IN3, M4_IN2, M4_IN4);
const char* ssid = "";
const char* password = "";
const char* mqtt_server = "";

WiFiClient espClient;
PubSubClient client(espClient);
unsigned long lastMsg = 0;
#define MSG_BUFFER_SIZE  (50)
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

void callback(char* topic, byte* payload, unsigned int length) {
  Serial.print("Message arrived [");
  Serial.print(topic);
  Serial.print("] ");
  String mypayload = "";
  for (int i = 0; i < length; i++) {
    mypayload += (char)payload[i];
    Serial.print((char)payload[i]);
  }
  Serial.println();

  String mytopic = topic;

  if(mytopic == "haro/controller2/pump"){
    Serial.print("수신1=");
    Serial.println("mytopic");
    int com1 = mypayload.indexOf(",");
    int com2 = mypayload.indexOf(",",com1+1);
    int com3 = mypayload.indexOf(",",com2+1);
  
    int pos1 = mypayload.substring(0,com1).toInt();
    int pos2 = mypayload.substring(com1+1,com2).toInt();
    int pos3 = mypayload.substring(com2+1,com3).toInt();
    int pos4 = mypayload.substring(com3+1,mypayload.length()).toInt();
  
    stepper1.moveTo(pos1);
    stepper2.moveTo(pos2);
    stepper3.moveTo(pos3);
    stepper4.moveTo(pos4);
  }else if(mytopic == "haro/controller2/pump2"){
    Serial.print("수신2=");
    Serial.println("mytopic");
    if(payload[0] == '0'){
      digitalWrite(pump,LOW);
    }else if(payload[0] == '1'){
      digitalWrite(pump,HIGH);
    }
  }
}

void reconnect() {
  // Loop until we're reconnected
  while (!client.connected()) {
    Serial.print("Attempting MQTT connection...");
    // Create a random client ID
    String clientId = "ESP8266Client-";
    clientId += String(random(0xffff), HEX);
    // Attempt to connect
    if (client.connect(clientId.c_str())) {
      Serial.println("connected");
      // Once connected, publish an announcement...
      //client.publish("outTopic", "hello world");
      // ... and resubscribe
      client.subscribe("haro/controller2/pump");
      client.subscribe("haro/controller2/pump2");
    } else {
      Serial.print("failed, rc=");
      Serial.print(client.state());
      Serial.println(" try again in 5 seconds");
      // Wait 5 seconds before retrying
      delay(5000);
    }
  }
}

void setup() {
  Serial.begin(115200);
  pinMode(pump,OUTPUT);
  setup_wifi();
  client.setServer(mqtt_server, 1883);
  client.setCallback(callback);
  stepper1.setMaxSpeed(800.0);   // 최대 속도 (steps/second)
  stepper1.setAcceleration(300.0); // 가속도 (steps/second^2)
  stepper2.setMaxSpeed(800.0);   // 최대 속도 (steps/second)
  stepper2.setAcceleration(300.0); // 가속도 (steps/second^2)
  stepper3.setMaxSpeed(800.0);   // 최대 속도 (steps/second)
  stepper3.setAcceleration(300.0); // 가속도 (steps/second^2)
  stepper4.setMaxSpeed(800.0);   // 최대 속도 (steps/second)
  stepper4.setAcceleration(300.0); // 가속도 (steps/second^2)
}

void loop() {
  if (!client.connected()) {
    reconnect();
  }
  client.loop();

  unsigned long now = millis();
  if (now - lastMsg > 100) {
    lastMsg = now;
    String data = String(stepper1.currentPosition())+","
                  +String(stepper2.currentPosition())+","
                  +String(stepper3.currentPosition())+","
                  +String(stepper4.currentPosition());
    client.publish("haro/controller2/unity", data.c_str());
  }
  
  stepper1.run();
  stepper2.run();
  stepper3.run();
  stepper4.run();
}