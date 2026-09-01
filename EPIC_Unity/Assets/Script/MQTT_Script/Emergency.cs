using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Text;
using System.Collections;
/*
public class Emergency : MonoBehaviour
{
     MqttClient client;
    string clientId;


    void Start()
    {
        try
        {
            // string BrokerAddress = "192.168.0.5";
            string BrokerAddress = "broker.emqx.io";
            int BrokerPort = 1883;

            client = new MqttClient(BrokerAddress, BrokerPort, false, null, null, MqttSslProtocols.None);
            clientId = System.Guid.NewGuid().ToString();
            client.Connect(clientId);

            string[] mytopic = { "haro/controller2/state"};
            byte[] myqos = {
                MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE,
                MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE,
                MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE
            };
            client.Subscribe(mytopic, myqos);

            Debug.Log("[MQTT] Connected and Subscribed to Topics.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("[MQTT] 연결 실패: " + ex.Message);
        }
    }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
*/