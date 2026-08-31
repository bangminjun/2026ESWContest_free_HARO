# YOLOv8 실시간 인원 감지 & MQTT 전송 시스템

웹캠 영상을 실시간으로 분석해 사람(작업자)을 감지하고, 감지된 인원 수를 MQTT 브로커로 전송하는 파이썬 프로그램입니다.

---

## 📌 주요 기능

- **실시간 객체 감지**: YOLOv8 모델(`TEST4.pt`)을 사용해 웹캠 프레임에서 사람/작업자를 실시간으로 탐지
- **바운딩 박스 시각화**: 감지된 대상에 초록색 박스와 클래스 이름 라벨을 화면에 표시
- **인원 수 카운트**: 프레임마다 감지된 `people` / `worker` 클래스 개수를 집계
- **MQTT 전송**: 집계된 인원 수를 지정한 MQTT 브로커/토픽으로 실시간 발행(publish)
- **실시간 화면 출력**: OpenCV 창으로 감지 결과 확인 (`q` 키로 종료)

---

## 🛠 필요 라이브러리

| 라이브러리 | 용도 |
|---|---|
| `opencv-python` (cv2) | 카메라 입력 및 화면 출력, 박스/텍스트 그리기 |
| `ultralytics` | YOLOv8 모델 로드 및 예측(inference) |
| `paho-mqtt` | MQTT 브로커 연결 및 메시지 발행 |

### 설치 방법

```bash
pip install opencv-python ultralytics paho-mqtt
```

> ⚠️ `ultralytics`는 내부적으로 `torch`(PyTorch)를 필요로 하므로, GPU를 사용하려면 CUDA 지원 버전의 PyTorch를 별도로 설치해야 할 수 있습니다.

---

## 📁 필요 파일

- `TEST1.py` — 메인 실행 스크립트
- `TEST4.pt` — 학습된 YOLOv8 커스텀 모델 가중치 파일 (같은 폴더에 위치해야 함)

---

## ⚙️ 사용 전 설정

코드 상단의 MQTT 설정 값을 환경에 맞게 반드시 채워야 합니다. 현재는 비어 있는 상태입니다.

```python
MQTT_BROKER = ""       # 예: "192.168.0.10" 또는 "broker.hivemq.com"
MQTT_PORT = 1883
MQTT_TOPIC_DETECT = "" # 예: "factory/detect/count"
```

- `MQTT_BROKER`: MQTT 브로커의 IP 주소 또는 호스트명
- `MQTT_TOPIC_DETECT`: 감지된 인원 수를 발행할 토픽명

---

## ▶️ 사용법

1. 위 라이브러리 설치
2. `TEST1.py`와 `TEST4.pt`를 같은 디렉터리에 위치
3. `MQTT_BROKER`, `MQTT_TOPIC_DETECT` 값 입력
4. 웹캠이 PC에 연결되어 있는지 확인 (`cv2.VideoCapture(0)` → 기본 카메라 사용)
5. 아래 명령어로 실행

```bash
python TEST1.py
```

6. 실행되면:
   - MQTT 브로커 연결 시도 → 성공/실패 메시지 출력
   - 모델 로드 후 클래스 목록 출력
   - 웹캠 화면(1280x720)에 실시간 감지 결과 표시
   - 콘솔에 `Detected count: N` 형태로 감지 인원 수 출력
   - MQTT 토픽으로 인원 수(문자열) 전송
7. **`q` 키**를 누르면 프로그램 종료 (카메라 해제, MQTT 연결 해제까지 정상 처리)

---

## 🔍 동작 원리 요약

```
웹캠 프레임 캡처
   ↓
YOLOv8 모델 예측 (conf=0.25, imgsz=640)
   ↓
클래스가 "people" 또는 "worker"인 객체만 필터링
   ↓
바운딩 박스 + 라벨 그리기, 카운트 증가
   ↓
카운트 값을 MQTT로 publish
   ↓
화면 출력 (OpenCV 창)
```

---

## ⚠️ 참고 / 주의사항

- 모델(`TEST4.pt`)의 클래스 이름이 실제로 `people`, `worker`인지 확인이 필요합니다. 다른 이름이면(예: `person`) 감지가 되지 않을 수 있으니 실행 시 콘솔에 출력되는 `클래스 목록`을 확인하세요.
- MQTT 브로커 주소/포트/토픽이 비어 있으면 연결에 실패하고 프로그램이 즉시 종료됩니다(`exit(1)`).
- 카메라 인덱스는 `cv2.VideoCapture(0)`으로 고정되어 있으므로, 카메라가 여러 대이거나 외장 캠을 쓰는 경우 인덱스를 조정해야 할 수 있습니다.
- 별도의 예외 처리(`try/except`)가 프레임 읽기 실패에 대해서는 `continue`로만 처리되어 있어, 카메라 연결이 끊기면 무한 루프에 빠질 수 있습니다.