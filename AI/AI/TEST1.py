import cv2
from ultralytics import YOLO
import paho.mqtt.client as mqtt

# MQTT 설정
MQTT_BROKER = ""
MQTT_PORT = 1883
MQTT_TOPIC_DETECT = ""  # 감지된 사람 수 전송용

client = mqtt.Client()
try:
    client.connect(MQTT_BROKER, MQTT_PORT, 60)
    client.loop_start()
    print("MQTT 연결 성공")
except Exception as e:
    print("MQTT 연결 실패:", e)
    exit(1)

# 모델 불러오기
model = YOLO("TEST4.pt")
CLASS_NAMES = model.names
print("클래스 목록:", CLASS_NAMES)

# 카메라 설정
cap = cv2.VideoCapture(0)
cap.set(cv2.CAP_PROP_FRAME_WIDTH, 1280)
cap.set(cv2.CAP_PROP_FRAME_HEIGHT, 720)

try:
    while True:
        ret, frame = cap.read()
        if not ret:
            print("카메라 프레임 error")
            continue

        # YOLO 예측
        results = model.predict(source=frame.copy(), imgsz=640, conf=0.25, verbose=False)[0]

        count_detected = 0

        # 사람/작업자 감지
        for box in results.boxes:
            class_id = int(box.cls[0])
            class_name = CLASS_NAMES[class_id]

            # "people" 또는 "worker" 감지
            if class_name in ["people", "worker"]:
                count_detected += 1

                # 바운딩 박스 표시
                x1, y1, x2, y2 = map(int, box.xyxy[0])
                cv2.rectangle(frame, (x1, y1), (x2, y2), (0, 255, 0), 2)
                label = f"{class_name}"
                cv2.putText(frame, label, (x1, y1 - 10),
                            cv2.FONT_HERSHEY_SIMPLEX, 0.6, (0, 255, 0), 2)

        # MQTT 전송 (감지된 사람 수)
        client.publish(MQTT_TOPIC_DETECT, str(count_detected))
        print(f"Detected count: {count_detected}")

        # 화면 출력
        cv2.imshow("YOLOv8 Real-time Detection", frame)

        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

finally:
    cap.release()
    cv2.destroyAllWindows()
    client.loop_stop()
    client.disconnect()

