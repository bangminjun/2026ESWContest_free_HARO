using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt;

public class test : MonoBehaviour
{
    public ArticulationBody rot;
    public ArticulationBody arm1;
    public ArticulationBody arm2;
    public ArticulationBody arm3;
    public ParticleSystem ps; 
    
    /*
    MqttClient client;
    string clientId;

    void Start()
    {

        string BrokerAddress = "192.168.0.5";
        client = new MqttClient(BrokerAddress);

        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

        // use a unique id as client id, each time we start the application
        clientId = System.Guid.NewGuid().ToString();
        client.Connect(clientId);

        arm1 = rot.GetComponent<ArticulationBody>();
        arm2 = ar2.GetComponent<ArticulationBody>();
        arm3 = ar3.GetComponent<ArticulationBody>();
        arm4 = ar4.GetComponent<ArticulationBody>();


        //유니티에서 브로커랑 접속이 완료된 지점!
        //구독신청하기!
        string[] mytopic = { "bssm/ljy" };
        byte[] myqos = { 0 };

        client.Subscribe(mytopic, myqos);

    }
    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        string ReceivedMessage = System.Text.Encoding.UTF8.GetString(e.Message);
        print(ReceivedMessage);

        if (ReceivedMessage[0] == '1')
        {
            UnityMainThreadDispatcher.Instance?.Enqueue(() =>
            {
                float now = arm1.xDrive.target;
                arm1.SetDriveTarget(ArticulationDriveAxis.X, now + 1);
            });
        }
        else if (ReceivedMessage[0] == '2')
        {
            UnityMainThreadDispatcher.Instance?.Enqueue(() =>
            {
                float now = arm1.xDrive.target;
                arm1.SetDriveTarget(ArticulationDriveAxis.X, now - 1);
            });
        }
        if (ReceivedMessage[1] == '1')
        {
            UnityMainThreadDispatcher.Instance?.Enqueue(() =>
            {
                float now = arm2.xDrive.target;
                arm2.SetDriveTarget(ArticulationDriveAxis.X, now + 1);
            });
        }
        else if (ReceivedMessage[1] == '2')
        {
            UnityMainThreadDispatcher.Instance?.Enqueue(() =>
            {
                float now = arm2.xDrive.target;
                arm2.SetDriveTarget(ArticulationDriveAxis.X, now - 1);
            });
        }
        if (ReceivedMessage[2] == '1')
        {
            UnityMainThreadDispatcher.Instance?.Enqueue(() =>
            {
                float now = arm3.xDrive.target;
                arm3.SetDriveTarget(ArticulationDriveAxis.X, now + 1);
            });
        }
        else if (ReceivedMessage[2] == '2')
        {
            UnityMainThreadDispatcher.Instance?.Enqueue(() =>
            {
                float now = arm3.xDrive.target;
                arm3.SetDriveTarget(ArticulationDriveAxis.X, now - 1);
            });
        }
        if (ReceivedMessage[3] == '1')
        {
            UnityMainThreadDispatcher.Instance?.Enqueue(() =>
            {
                float now = arm4.xDrive.target;
                arm4.SetDriveTarget(ArticulationDriveAxis.X, now + 1);
            });
        }
        else if (ReceivedMessage[3] == '2')
        {
            UnityMainThreadDispatcher.Instance?.Enqueue(() =>
            {
                float now = arm4.xDrive.target;
                arm4.SetDriveTarget(ArticulationDriveAxis.X, now - 1);
            });
        }
    }
    private void OnApplicationQuit()
    {
        //사용자가 유니티를 껏다!
        client.Disconnect();
    }
    */
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            ps.Play();
        }
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            ps.Stop();
        }
    }
}
