using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Text;

public class codered : MonoBehaviour
{
    MqttClient client;
    string clientId;

    void Start()
    {
        string BrokerAddress = "broker.emqx.io";
        client = new MqttClient(BrokerAddress);
        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

        // 클라이언트 ID 생성 후 연결
        clientId = System.Guid.NewGuid().ToString();
        client.Connect(clientId);

        // ✅ 토픽 구독
        client.Subscribe(new string[] { "bssm/ljy" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });

        Debug.Log("[MQTT] Connected & Subscribed to bssm/ljy");
    }

    // 메시지 수신 콜백 함수
    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        string message = Encoding.UTF8.GetString(e.Message);
        Debug.Log($"[MQTT] Topic: {e.Topic}, Message: {message}");
    }

    void Update()
    {
        // 필요시 메시지 Publish 코드 추가 가능
    }

    private void OnApplicationQuit()
    {
        if (client != null && client.IsConnected)
        {
            client.Disconnect();
            Debug.Log("[MQTT] Disconnected");
        }
    }
}
