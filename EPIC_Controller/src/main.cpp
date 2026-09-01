#include <Arduino.h>
#include <WiFi.h>
#include <PubSubClient.h>

#define JOY1Y 36
#define JOY1X 39
#define JOY2Y 34
#define JOY2X 35
#define JOY3Y 32
#define JOY3X 33
#define BTN4 19
#define BTN5 18
#define BTN6 17
#define BTN7 16

const char* ssid = "ssid";
const char* password = "password";
const char* mqtt_server = "mqtt_server";

WiFiClient espClient;
PubSubClient client(espClient);
unsigned long lastMsg = 0;

void setup_wifi() {
  delay(10);
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

// esp32 데이터 수신부
void callback(char* topic, byte* payload, unsigned int length) {
  Serial.print("Message arrived [");
  Serial.print(topic);
  Serial.print("] ");
  for (int i = 0; i < length; i++) {
    Serial.print((char)payload[i]);
  }
  Serial.println();
}

void reconnect() {
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

void setup() {
  Serial.begin(115200);

  setup_wifi();
  client.setServer(mqtt_server, 1883);
  client.setCallback(callback);
  pinMode(BTN4, INPUT);
  pinMode(BTN5, INPUT);
  pinMode(BTN6, INPUT);
  pinMode(BTN7, INPUT);
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
    int btn4 = digitalRead(BTN4) == LOW ? 0 : 1;
    int btn5 = digitalRead(BTN5) == LOW ? 1 : 0;
    int btn6 = digitalRead(BTN6) == LOW ? 0 : 1;
    int btn7 = digitalRead(BTN7) == LOW ? 0 : 1;

    int controlx = analogRead(JOY1X);
    int controly = analogRead(JOY1Y);
    int controlx2 = analogRead(JOY2X);
    int controly2 = analogRead(JOY2Y);
    int controlx3 = analogRead(JOY3X);
    int controly3 = analogRead(JOY3Y);

    Serial.println(controlx);
    Serial.println(controly);

    if (controlx > 3000) {
      a = 1;
    } else if (controlx < 1000) {
      a = 2;
    } else {
      a = 0;
    }
    if (controly > 3000) {
      b = 1;
    } else if (controly < 1000) {
      b = 2;
    } else {
      b = 0;
    }
    if (controlx2 > 3000) {
      a2 = 1;
    } else if (controlx2 < 1000) {
      a2 = 2;
    } else {
      a2 = 0;
    }
    if (controly2 > 3000) {
      b2 = 1;
    } else if (controly2 < 1000) {
      b2 = 2;
    } else {
      b2 = 0;
    }
    if (controlx3 > 3000) {
      a3 = 1;
    } else if (controlx3 < 1000) {
      a3 = 2;
    } else {
      a3 = 0;
    }
    if (controly3 > 3000) {
      b3 = 1;
    } else if (controly3 < 1000) {
      b3 = 2;
    } else {
      b3 = 0;
    }

    String data = String(b) + String(a) + String(a2) + String(b2) + String(b3) + String(a3) + String(btn5) + String(btn7) + String(btn4) + String(btn6);

    Serial.print("Publish message: ");
    Serial.println(data);

    client.publish("haro/controller2/state", data.c_str());
  }
}