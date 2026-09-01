# HARO

건설 현장의 펌프카를 원격으로 제어할 수 있는 시스템으로, VR 시뮬레이터를 통한 조작 훈련과 AI 기반 작업자 감지로 현장 안전성을 높이는 프로젝트입니다.

## 폴더 구조

```
2026ESWContest_free_HARO/
├── AI/                  # YOLO 작업자 감지 (TEST1.py, 학습 가중치 TEST4.pt)
├── EPIC_Controller/     # 조이스틱 컨트롤러 펌웨어 (ESP32)
├── EPIC_Unity/          # Unity VR 시뮬레이터 (Quest 2)
├── Pumpcar_firmware/    # 실물 펌프카 펌웨어 (ESP32)
└── LICENSE              # GNU GPL v3.0
```

각 하위 폴더에는 개별 README가 포함되어 있어 세부 설치/사용 방법을 확인할 수 있습니다.

## 실행 방법

### AI (작업자 감지)
```bash
cd AI/AI
pip install ultralytics opencv-python paho-mqtt
python TEST1.py
```

### EPIC_Controller (조이스틱 컨트롤러)
PlatformIO로 `EPIC_Controller` 폴더를 열고 ESP32 보드에 업로드합니다.

### EPIC_Unity (VR 시뮬레이터)
Unity Hub에서 `EPIC_Unity` 폴더를 열고, Quest 2 기기 연결 후 Build & Run합니다.
> Meta XR SDK는 별도 설치 필요

### Pumpcar_firmware (실물 펌프카)
PlatformIO로 `Pumpcar_firmware` 폴더를 열고 ESP32 보드에 업로드합니다.

## 오픈소스 라이선스

| 이름 | 버전 | 라이선스 |
|---|---|---|
| M2Mqtt | 4.3.0.0 | EPL 1.0 |
| Oculus XR Plugin | 4.5.2 | Unity Companion License |
| Meta XR SDK (OVRPlugin) | 1.89.0 | Meta Platform Technologies SDK License |

## License

이 프로젝트 자체는 [GNU General Public License v3.0](./LICENSE) 하에 배포됩니다.
