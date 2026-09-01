# HARO

VR 기반 펌프카 교육 시뮬레이션과 디지털 트윈 안전제어 시스템을 결합한 프로젝트입니다.

## 폴더 구조

```
2026ESWContest_free_HARO/
├── EPIC_Unity/          # VR 기반 디지털 트윈 안전제어(MR) Unity 프로젝트
├── EPIC_Controller/     # 컨트롤러 펌웨어 및 하드웨어 관련 코드
├── Pumpcar_firmware/    # 펌프카 펌웨어 (ESP32 / PlatformIO)
├── AI/                  # 인물 감지(person detection) AI 모듈 (YOLO 기반)
└── LICENSE              # GNU GPL v3.0
```

각 하위 폴더에는 개별 README가 포함되어 있어 세부 설치/사용 방법을 확인할 수 있습니다.

## 실행 방법

### 1. MQTT 브로커 준비
Mosquitto 등 MQTT 브로커를 로컬 또는 서버에 실행합니다.

### 2. Unity 시뮬레이션 (EPIC_Unity)
```bash
git clone https://github.com/bangminjun/2026ESWContest_free_HARO.git
```
Unity Hub에서 `EPIC_Unity` 폴더를 프로젝트로 열고, Meta Quest 기기 연결 후 Build & Run합니다.

### 3. 펌프카 펌웨어 (Pumpcar_firmware)
PlatformIO(VS Code 확장 또는 CLI)로 `Pumpcar_firmware` 폴더를 열고 ESP32 보드에 업로드합니다.
```bash
cd Pumpcar_firmware
pio run --target upload
```

### 4. 인물 감지 AI (AI)
```bash
cd AI/AI
pip install ultralytics opencv-python paho-mqtt
python TEST1.py
```
실행 전 스크립트 내 `MQTT_BROKER`, `MQTT_TOPIC_DETECT` 값을 환경에 맞게 설정해야 합니다.

### 5. 컨트롤러 (EPIC_Controller)
`EPIC_Controller/README.md` 참고.

## 오픈소스 라이선스

| 이름 | 버전 | 라이선스 | 사용처 |
|---|---|---|---|
| Unity XR Management | 4.5.4 | Unity Companion License | EPIC_Unity |
| Unity XR Oculus | 4.5.2 | Unity Companion License | EPIC_Unity |
| Unity Collab Proxy | 2.11.4 | Unity Companion License | EPIC_Unity |
| Unity Timeline | 1.8.11 | Unity Companion License | EPIC_Unity |
| Unity Visual Scripting | 1.9.10 | Unity Companion License | EPIC_Unity |
| PubSubClient (knolleary) | ^2.8 | MIT | Pumpcar_firmware |
| AccelStepper (waspinator) | ^1.64 | GPL v3.0 | Pumpcar_firmware |
| Ultralytics YOLO | latest | AGPL-3.0 | AI |
| OpenCV (opencv-python) | latest | Apache 2.0 | AI |
| paho-mqtt | latest | Eclipse Distribution License 1.0 | AI |

> ⚠️ **참고**: `AI` 모듈에서 사용하는 **Ultralytics YOLO는 AGPL-3.0 라이선스**입니다. AGPL-3.0은 이를 포함한 프로젝트 전체의 소스코드 공개를 요구할 수 있으므로, 상업적/비공개 배포 시에는 별도 확인이 필요합니다. (참고: [Ultralytics License](https://www.ultralytics.com/license))

## License

이 프로젝트 자체는 [GNU General Public License v3.0](./LICENSE) 하에 배포됩니다.
