using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class body1 : MonoBehaviour
{
     ArticulationBody rot;
     public GameObject rotw;
    void Start()
    {
        rot = GetComponent<ArticulationBody>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha5)){
            rot.SetDriveTarget(ArticulationDriveAxis.X, 90);
        }
        if(Input.GetKeyDown(KeyCode.Alpha6)){
            rot.SetDriveTarget(ArticulationDriveAxis.X,0);
        }
        float y = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick,OVRInput.Controller.LTouch).x;
        if(y > 0.5){
            float now = rot.xDrive.target;
            rot.SetDriveTarget(ArticulationDriveAxis.X, now+1);
        }
        else if(y < -0.5){
            float now = rot.xDrive.target;
            rot.SetDriveTarget(ArticulationDriveAxis.X, now-1);
        }
    }
}
