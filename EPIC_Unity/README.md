# EPIC_Unity

VR 기반 디지털 트윈 안전제어(MR) 시뮬레이션 프로젝트입니다.

## 기술 스택

- **Unity**: 2022.3.22f1
- **언어**: C#
- **VR/XR**: Oculus / Meta Quest (Meta XR SDK, Oculus Interaction SDK)
- **통신**: MQTT (Mosquitto broker)
- **물리 시뮬레이션**: ArticulationBody 기반 3축 펌프카(Pumpcar) 제어
- **AI 연동**: 인물 감지(person detection) 결과를 MQTT로 실시간 수신

## 설치 방법

1. Unity Hub에서 **Unity 2022.3.22f1** 버전 설치
2. 레포지토리 클론
   ```bash
   git clone https://github.com/bangminjun/2026ESWContest_free_HARO.git
   ```
3. Unity Hub에서 `EPIC_Unity` 폴더를 프로젝트로 열기
4. 필요한 패키지 자동 설치 확인 (Package Manager)
   - Meta XR SDK
   - Oculus Interaction SDK

## 사용 방법

1. Mosquitto MQTT 브로커 실행 (로컬 또는 지정된 서버)
2. Unity에서 씬 실행 전 MQTT 브로커 주소/포트 설정 확인
3. `haro/controller2/state` 토픽을 구독하여 펌프카(ArticulationBody) 제어 상태 수신
4. AI 인물 감지 결과는 별도 MQTT 토픽을 통해 실시간으로 수신되어 시뮬레이션에 반영됨
5. VR 빌드 시 Meta Quest 기기와 연결 후 Build & Run

> Meta XR SDK v1.89.0 별도 설치 필요
