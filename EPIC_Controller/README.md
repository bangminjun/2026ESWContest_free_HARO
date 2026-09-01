# Haro EPIC Controller

ESP32 기반 하드웨어 컨트롤러 펌웨어입니다. 조이스틱 3개와 버튼 4개의 입력을 읽어, 상태값을 10자리 문자열로 인코딩한 뒤 Wi-Fi를 통해 MQTT 브로커로 발행(publish)합니다.

## 주요 기능

- Wi-Fi(STA 모드)로 지정한 AP에 연결
- MQTT 브로커에 연결 후 100ms 주기로 컨트롤러 상태 발행
- 조이스틱 3개(X/Y 아날로그 입력 6채널) 방향 판별 (중립 / +방향 / -방향)
- 버튼 4개 디지털 입력 상태 읽기
- 연결이 끊기면 5초 간격으로 MQTT 재연결 시도

## 하드웨어 구성

| 입력 | 핀 (ESP32 GPIO) |
|---|---|
| 조이스틱1 X | 39 |
| 조이스틱1 Y | 36 |
| 조이스틱2 X | 35 |
| 조이스틱2 Y | 34 |
| 조이스틱3 X | 33 |
| 조이스틱3 Y | 32 |
| 버튼4 (BTN4) | 19 |
| 버튼5 (BTN5) | 18 |
| 버튼6 (BTN6) | 17 |
| 버튼7 (BTN7) | 16 |

- 보드: `esp32dev` (Espressif32)
- 프레임워크: Arduino

## 설치 및 준비물

### 필요한 것
- ESP32 개발 보드
- 아날로그 조이스틱 모듈 3개, 택트 버튼 4개
- [PlatformIO](https://platformio.org/) (VS Code 확장 또는 CLI)
- MQTT 브로커 (예: Mosquitto) 및 접속 가능한 Wi-Fi 네트워크

### 1. 프로젝트 열기
```bash
git clone <repo-url>
cd Haro_EPIC_Controller
```
VS Code에서 폴더를 열면 `.vscode/extensions.json`에 명시된 **PlatformIO IDE** 확장 설치를 권장받습니다. 설치 후 PlatformIO가 프로젝트를 자동 인식합니다.

### 2. Wi-Fi / MQTT 접속 정보 설정
`src/main.cpp` 상단의 값을 실제 환경에 맞게 수정합니다.

```cpp
const char* ssid = "ssid";           // Wi-Fi SSID
const char* password = "password";   // Wi-Fi 비밀번호
const char* mqtt_server = "mqtt_server"; // MQTT 브로커 주소 (IP 또는 호스트명)
```

> 현재 MQTT 포트는 코드에 `1883`(기본 포트, 비TLS)으로 고정되어 있습니다.

### 3. 의존 라이브러리
`platformio.ini`에 아래 라이브러리가 선언되어 있으며, 빌드 시 PlatformIO가 자동으로 설치합니다.

```ini
lib_deps =
    waspinator/AccelStepper@^1.64
    knolleary/PubSubClient@^2.8
```

- `PubSubClient` : MQTT 통신에 사용
- `AccelStepper` : 의존성에는 포함되어 있으나 현재 `src/main.cpp`에서는 사용되지 않음 (스테퍼 모터 제어 등 추후 확장을 위한 것으로 보임)

### 4. 빌드 및 업로드
PlatformIO CLI 사용 시:
```bash
pio run                 # 빌드
pio run --target upload # ESP32에 업로드
pio device monitor      # 시리얼 모니터 (115200 baud)
```
또는 VS Code의 PlatformIO 사이드바에서 **Build** / **Upload** / **Monitor** 버튼을 사용합니다.

## 동작 방식 (사용법)

1. 전원이 켜지면 지정된 Wi-Fi에 연결하고, 연결되면 시리얼로 IP를 출력합니다.
2. MQTT 브로커에 연결합니다 (연결 실패 시 5초마다 재시도).
3. 100ms마다 조이스틱/버튼 값을 읽어 아래 규칙으로 인코딩한 뒤 발행합니다.

### 조이스틱 값 인코딩 규칙
아날로그 값(0~4095) 기준:
- `> 3000` → `1`
- `< 1000` → `2`
- 그 외(중립 구간) → `0`

### 버튼 값 인코딩 규칙
- BTN4, BTN6, BTN7: `LOW → 0`, `HIGH → 1`
- BTN5: `LOW → 1`, `HIGH → 0` (반전)

### 발행 데이터 형식
- **토픽**: `haro/controller2/state`
- **페이로드**: 10자리 숫자 문자열, 순서는 다음과 같습니다.

| 순서 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 |
|---|---|---|---|---|---|---|---|---|---|---|
| 값 | 조이1 Y | 조이1 X | 조이2 X | 조이2 Y | 조이3 Y | 조이3 X | BTN5 | BTN7 | BTN4 | BTN6 |

예: `0102010101` 형태의 문자열이 100ms마다 발행됩니다.

## 프로젝트 구조
```
.
├── include/          # 프로젝트 헤더 파일 (현재 비어있음)
├── lib/              # 프로젝트 전용 라이브러리 (현재 비어있음)
├── src/
│   └── main.cpp      # 메인 펌웨어 코드
├── test/             # PlatformIO 테스트 (현재 비어있음)
├── platformio.ini    # 빌드/보드/의존성 설정
└── LICENSE           # MIT License
```

## 라이선스
MIT License. 자세한 내용은 [LICENSE](./LICENSE) 파일을 참고하세요.
