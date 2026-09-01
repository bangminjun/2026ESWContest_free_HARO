using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Text;
using System.Collections;

public class MqttSender : MonoBehaviour
{
    MqttClient client;
    string clientId;

    public string topicPump = "haro/controller2/pump";
    public string topicPcb = "haro/controller2/pump2";
    public string topicDetect = "Haro/detect";
    public GameObject WarningPanel;
    public GameObject warningPalnel2;
    public TMPro.TextMeshProUGUI pumpValueText;

    public ArticulationBody rot;
    public ArticulationBody arm1;
    public ArticulationBody arm2;
    public ArticulationBody arm3;
    public GameObject cameracontroll;
    public GameObject person;
    public ParticleSystem ps;

    void Start()
    {
        if (WarningPanel != null)
            WarningPanel.SetActive(false);
        if (warningPalnel2 != null)
            warningPalnel2.SetActive(false);
        if(person != null)
            person.SetActive(false);

        try
        {
            string BrokerAddress = "192.168.0.5";
            // string BrokerAddress = "broker.emqx.io";
            int BrokerPort = 1883;

            client = new MqttClient(BrokerAddress, BrokerPort, false, null, null, MqttSslProtocols.None);
            clientId = System.Guid.NewGuid().ToString();
            client.Connect(clientId);

            string[] mytopic = { topicPump, "haro/controller2/state", topicDetect };
            byte[] myqos = {
                MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE,
                MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE,
                MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE
            };

            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            client.Subscribe(mytopic, myqos);

            Debug.Log("[MQTT] Connected and Subscribed to Topics.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("[MQTT] 연결 실패: " + ex.Message);
        }
    }

    private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        string receivedMessage = Encoding.UTF8.GetString(e.Message);

        if (e.Topic == topicDetect)
        {
            Debug.Log("[MQTT] Haro/detect 토픽 메시지 수신됨");

            UnityMainThreadDispatcher.Instance?.Enqueue(() =>
            {
                try
                {
                    int value = int.Parse(receivedMessage.Trim());
                    if (pumpValueText != null)
                        pumpValueText.text = $"작업자 {value}명이 감지되었습니다";

                    if (value > 0)
                    {
                        if (WarningPanel != null) WarningPanel.SetActive(true);
                        if (person != null) person.SetActive(true);
                        Debug.LogWarning($"[MQTT] 감지 인원 {value}명 → WarningPanel 활성화");
                    }
                    else
                    {
                        if (WarningPanel != null) WarningPanel.SetActive(false);
                        if (person != null) person.SetActive(false);
                        Debug.Log($"WarningPanel 비활성화");
                    }
                }
                catch
                {
                    Debug.LogWarning($" 수신된 메시지가 숫자가 아닙니다. ({receivedMessage})");
                }
            });
            return;
        }

        if (e.Topic == "haro/controller2/state")
        {
            UnityMainThreadDispatcher.Instance?.Enqueue(() =>
            {
                try
                {
                    if (rot == null || arm1 == null || arm2 == null || arm3 == null || cameracontroll == null)
                    {
                        Debug.LogWarning("[MQTT] 로봇 부품 참조가 누락되어 있습니다.");
                        return;
                    }

                    float x = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).x;
                    float l = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).y;
                    float k = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch).x;
                    float y = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch).y;
               
                    if ((receivedMessage[0] == '1') || (x > 0.5f))
                        rot.SetDriveTarget(ArticulationDriveAxis.X, rot.xDrive.target - 1);
                    if ((receivedMessage[0] == '2') || (x < -0.5f))
                        rot.SetDriveTarget(ArticulationDriveAxis.X, rot.xDrive.target + 1);
                    if ((receivedMessage[1] == '1') || (l > 0.5f))
                        arm1.SetDriveTarget(ArticulationDriveAxis.X, arm1.xDrive.target - 1);
                    if ((receivedMessage[1] == '2') || (l < -0.5f))
                        arm1.SetDriveTarget(ArticulationDriveAxis.X, arm1.xDrive.target + 1);
                    if (receivedMessage[2] == '2')
                        cameracontroll.GetComponent<CameraOrbit>().direction = 1f;
                    if (receivedMessage[2] == '1')
                        cameracontroll.GetComponent<CameraOrbit>().direction = -1f;
                    if( receivedMessage[2] == '0')
                    {
                        cameracontroll.GetComponent<CameraOrbit>().direction = 0f;
                    }
                    if ((receivedMessage[4] == '1') || (k < -0.5f))
                        arm2.SetDriveTarget(ArticulationDriveAxis.X, arm2.xDrive.target - 1);
                    if ((receivedMessage[4] == '2') || (k > 0.5f))
                        arm2.SetDriveTarget(ArticulationDriveAxis.X, arm2.xDrive.target + 1);
                    if ((receivedMessage[5] == '1') || (y > 0.5f))
                        arm3.SetDriveTarget(ArticulationDriveAxis.X, arm3.xDrive.target - 1);
                    if ((receivedMessage[5] == '2') || (y < -0.5f))
                        arm3.SetDriveTarget(ArticulationDriveAxis.X, arm3.xDrive.target + 1);
                    if (receivedMessage[6] == '1' || OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
                    {
                        client.Publish(topicPcb, Encoding.UTF8.GetBytes("1"), 0, false);
                        ps.Play();
                    }
                    if (receivedMessage[6] == '0' || OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
                    {
                        client.Publish(topicPcb, Encoding.UTF8.GetBytes("0"), 0, false);
                        ps.Stop(); 
                    }
                    if(receivedMessage[8] == '1')
{
    StartCoroutine(MoveRobotArm());
}
string MakeMessage()
{
    int v1 = Mathf.RoundToInt((float)(arm1.xDrive.target * 5.68f) * 8);
    int v2 = Mathf.RoundToInt((float)(rot.xDrive.target * -5.68f) * 8);
    int v3 = Mathf.RoundToInt((float)(arm2.xDrive.target * 5.68f) * 8);
    int v4 = Mathf.RoundToInt((float)(arm3.xDrive.target * -5.68f) * 8);

    return $"{v1},{v2},{v4},{v3}";
}

IEnumerator MoveRobotArm()
{
    arm1.SetDriveTarget(ArticulationDriveAxis.X, 90);
    arm2.SetDriveTarget(ArticulationDriveAxis.X, -180);
    arm3.SetDriveTarget(ArticulationDriveAxis.X, 180);
    Send("step3");
    yield return new WaitForSeconds(7f);
    arm3.SetDriveTarget(ArticulationDriveAxis.X, 0);
    Send("step4");
    yield return new WaitForSeconds(2f);

    arm2.SetDriveTarget(ArticulationDriveAxis.X, 0);
    Send("step5");
    yield return new WaitForSeconds(3f);

    arm1.SetDriveTarget(ArticulationDriveAxis.X, 0);
    Send("step6");
    yield return new WaitForSeconds(4f);

    rot.SetDriveTarget(ArticulationDriveAxis.X, 0);
    Send("step7");

    Debug.LogWarning("로봇 팔 동작 완료");
}

void Send(string tag)
{
    string m = MakeMessage();
    client.Publish(topicPump, Encoding.UTF8.GetBytes(m));
    Debug.Log($"[{tag}] MQTT Sent: {m}");
}
                    
                    if (receivedMessage[7] == '1'|| OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch) || Input.GetKeyDown(KeyCode.M))
                    {
                            int value1 = Mathf.RoundToInt((float)(arm1.xDrive.target * 5.68f) * 8);
                            int value2 = Mathf.RoundToInt((float)(rot.xDrive.target * -5.68f) * 8);
                            int value3 = Mathf.RoundToInt((float)(arm2.xDrive.target * 5.68f) * 8);
                            int value4 = Mathf.RoundToInt((float)(arm3.xDrive.target * -5.68f) * 8);
                        string message = $"{value1},{value2},{value4},{value3}";
                        if (arm1.xDrive.target !=-100 )
                        {
                            warningPalnel2.SetActive(false);
                            client.Publish(topicPump, Encoding.UTF8.GetBytes(message));
                            Debug.Log($"[MQTT Sent to {topicPump}] {message}");
                        }
                        else
                        {
                            warningPalnel2.SetActive(true);
                            Debug.LogWarning("[MQTT] 팔 동작 범위 초과 경고 패널 활성화");
                        }
                    }
                 
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"[MQTT] State 메시지 처리 오류: {ex.Message}");
                }
            });
        }
    }

    void Update()
    {
        if (client == null || !client.IsConnected)
            return;
        /*if (Input.GetKeyDown(KeyCode.N))
        {
            cameracontroll.GetComponent<CameraOrbit>().direction = 1f;
            //Debug.Log("N키 누르는 중 - direction: " + cameracontroll.GetComponent<CameraOrbit>().direction);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            cameracontroll.GetComponent<CameraOrbit>().direction = -1f;
            //Debug.Log("L키 누르는 중 - direction: " + cameracontroll.GetComponent<CameraOrbit>().direction);
        }
        if(!Input.GetKey(KeyCode.N) && !Input.GetKey(KeyCode.L))
        {

            cameracontroll.GetComponent<CameraOrbit>().direction = 0f;
            //Debug.Log("키 누르는 중 - direction: " + cameracontroll.GetComponent<CameraOrbit>().direction);
        }*/
        if (Input.GetKeyDown(KeyCode.Alpha7) || OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            client.Publish(topicPcb, Encoding.UTF8.GetBytes("1"), 0, false);
            Debug.Log($"[MQTT Sent to {topicPcb}] ON");
        }
        if (Input.GetKeyDown(KeyCode.Alpha8) || OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            client.Publish(topicPcb, Encoding.UTF8.GetBytes("0"), 0, false);
            Debug.Log($"[MQTT Sent to {topicPcb}] OFF");
        }
       
    }

    private void OnApplicationQuit()
    {
        if (client != null && client.IsConnected)
        {
            client.Disconnect();
            Debug.Log("[MQTT] Disconnected.");
        }
    }
}
