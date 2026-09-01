using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pump_c : MonoBehaviour
{
    public ArticulationBody rot;
    public ArticulationBody arm1;
    public ArticulationBody arm2;
    public ArticulationBody arm3;

    public GameObject ball; // 공 오브젝트
    public Vector3 targetPosition = new Vector3(1f, 0.5f, -2f); 

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            // 공 위치 이동
            if (ball != null)
            {
                ball.transform.position = targetPosition;
                ball.GetComponent<Rigidbody>().linearVelocity = Vector3.zero; // 물리 영향 제거
                ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
        }
    }

    // targetPosition 설정 함수
    void SetTarget(ArticulationBody body, float target)
    {
        ArticulationDrive drive = body.xDrive;
        drive.target = target;
        body.xDrive = drive;
    }
}
