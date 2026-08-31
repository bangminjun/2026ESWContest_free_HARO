# 펌프카 펌웨어 (ESP32 + MQTT + 스텝모터 4축)

MQTT로 수신한 명령에 따라 스텝모터 4개와 펌프를 제어하는 ESP32 펌웨어입니다.

---

## 📌 주요 기능

- **WiFi 연결**: 지정된 SSID로 자동 연결, 연결될 때까지 대기
- **MQTT 통신**: 지정된 브로커에 연결, 명령 수신 및 상태 발행
- **스텝모터 4개 제어**: `AccelStepper` 라이브러리로 4개의 28BYJ-48류 스테퍼 모터(ULN2003 드라이버)를 위치 제어
- **펌프 ON/OFF 제어**: GPIO 4번 핀으로 펌프 릴레이 제어
- **실시간 위치 피드백**: 100ms마다 4개 모터의 현재 위치를 MQTT로 전송 (Unity 등 시각화 프로그램 연동용으로 보임)

---

## 🛠 필요 라이브러리

Arduino IDE의 **라이브러리 매니저**(스케치 → 라이브러리 포함 → 라이브러리 관리)에서 설치:

| 라이브러리 | 용도 | 비고 |
|---|---|---|
| `WiFi.h` | ESP32 WiFi 연결 | ESP32 보드 패키지에 기본 포함 |
| `PubSubClient` (by Nick O'Leary) | MQTT 통신 | 라이브러리 매니저에서 검색 설치 |
| `AccelStepper` (by Mike McCauley) | 스테퍼 모터 가속/감속 제어 | 라이브러리 매니저에서 검색 설치 |

---

## 🔌 하드웨어 연결

**ULN2003 드라이버 보드 × 4개**, 각각 스텝모터 1개씩 연결

| 모터 | IN1 | IN2 | IN3 | IN4 |
|---|---|---|---|---|
| Motor 1 | 13 | 12 | 14 | 27 |
| Motor 2 | 26 | 25 | 33 | 32 |
| Motor 3 | 16 | 17 | 5 | 18 |
| Motor 4 | 19 | 21 | 22 | 23 |

- **펌프**: GPIO 4번 → 릴레이 모듈 IN

> ⚠️ `AccelStepper` 생성자 순서가 `(IN1, IN3, IN2, IN4)`인 건 `HALF4WIRE` 모드의 정상적인 배선 순서라 그대로 두면 됩니다.

---

## ⚙️ 빌드 전 반드시 채워야 할 값

현재 아래 세 값이 **빈 문자열**이라 이 상태로는 WiFi/MQTT 연결이 되지 않습니다.

```cpp
const char* ssid = "";         // → 사용할 WiFi 이름 입력
const char* password = "";     // → 사용할 WiFi 비밀번호 입력
const char* mqtt_server = "";  // → MQTT 브로커 IP 입력 (예: "192.168.0.5")
```

---

## 🏗 빌드 방법 (Arduino IDE 기준)

1. **ESP32 보드 패키지 설치**
   파일 → 환경설정 → 추가 보드 매니저 URL에 추가:
   `https://raw.githubusercontent.com/espressif/arduino-esp32/gh-pages/package_esp32_index.json`
   도구 → 보드 → 보드매니저 → `esp32` 검색 후 설치
2. **보드 선택**: 도구 → 보드 → ESP32 Arduino → 사용 중인 보드(예: `ESP32 Dev Module`) 선택
3. **라이브러리 설치**: 위 표의 `PubSubClient`, `AccelStepper` 설치
4. **포트 선택**: 도구 → 포트 → ESP32 연결된 COM 포트 선택
5. **빈 값(ssid/password/mqtt_server) 채우기**
6. **코드 업로드**: 스케치 → 업로드 (Ctrl+U)
7. **시리얼 모니터 확인**: 115200 baud로 WiFi/MQTT 연결 로그 확인

---

## 📡 MQTT 토픽 구조

**구독(수신)**

| 토픽 | 포맷 | 동작 |
|---|---|---|
| `haro/controller2/pump` | `"pos1,pos2,pos3,pos4"` (콤마 구분 정수) | 4개 모터 목표 위치를 `moveTo()`로 설정 |
| `haro/controller2/pump2` | `"1"` 또는 `"0"` | 펌프 ON / OFF |

**발행(송신)**

| 토픽 | 포맷 | 주기 |
|---|---|---|
| `haro/controller2/unity` | `"pos1,pos2,pos3,pos4"` (현재 위치) | 100ms마다 |

---

## 🔍 동작 흐름

```
전원 인가 → WiFi 연결 → MQTT 브로커 연결/구독
   ↓
loop():
  - MQTT 연결 끊기면 재연결 시도
  - "pump" 토픽 수신 시 → 4개 모터 목표 위치(moveTo) 설정
  - "pump2" 토픽 수신 시 → 펌프 ON/OFF
  - 100ms마다 현재 모터 위치를 "unity" 토픽으로 발행
  - 모터 4개 각각 run()으로 실제 이동 처리 (매 loop마다 반복 호출 필요)
```

---

## ⚠️ 참고 / 잠재적 이슈

- `stepperN.run()`은 매우 자주(가능한 빠르게) 호출되어야 부드러운 가속/감속이 이루어지는데, `reconnect()`의 `delay(5000)` 같은 블로킹 코드 실행 중에는 모터 움직임이 완전히 멈춥니다.
- `pos1~pos4`는 **목표 위치(절대 좌표, steps)**로 해석됩니다 (`moveTo` 사용, 상대 이동 아님).
- ssid/password/mqtt_server가 빈 채로 업로드하면 WiFi 연결 단계(`while (WiFi.status() != WL_CONNECTED)`)에서 무한 대기하니, 업로드 전 반드시 값을 채워야 합니다.
- 앞서 만든 Python(YOLO+MQTT) 스크립트와 연동하려면 같은 MQTT 브로커 IP를 사용하고, 토픽명(`haro/controller2/...`)도 서로 맞춰줘야 합니다.