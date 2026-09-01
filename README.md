# HARO

VR 기반 펌프카 교육 시뮬레이션과 디지털 트윈 안전제어 시스템을 결합한 프로젝트입니다.

## 폴더 구조

| 폴더 | 설명 |
|---|---|
| [`EPIC_Unity/`](./EPIC_Unity) | VR 기반 디지털 트윈 안전제어(MR) Unity 프로젝트. MQTT로 펌프카(ArticulationBody) 제어 상태를 수신하고, AI 인물 감지 결과를 실시간 반영 |
| [`EPIC_Controller/`](./EPIC_Controller) | 컨트롤러 펌웨어 및 하드웨어 관련 코드 |
| [`Pumpcar_firmware/`](./Pumpcar_firmware) | 펌프카 펌웨어 (PlatformIO 기반) |
| [`AI/`](./AI) | 인물 감지(person detection) AI 모듈 |

## 시스템 개요

- Unity(`EPIC_Unity`)와 펌프카(`Pumpcar_firmware`)는 MQTT 브로커(Mosquitto)를 통해 통신합니다.
- `haro/controller2/state` 토픽을 구독하여 3축 펌프카의 실시간 상태를 Unity 시뮬레이션에 반영합니다.
- `AI` 모듈은 인물 감지 결과를 별도 MQTT 토픽으로 전송하며, 이는 안전제어 로직에 사용됩니다.
- `EPIC_Controller`는 이 시스템의 컨트롤러 펌웨어를 담당합니다.

## 각 폴더별 상세 정보

각 하위 폴더에는 별도의 README가 포함되어 있어 개별 설치/사용 방법을 확인할 수 있습니다.

## License

이 프로젝트는 [GNU General Public License v3.0](./LICENSE) 하에 배포됩니다.
