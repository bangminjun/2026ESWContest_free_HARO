using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pump_body1 : MonoBehaviour
{
    ArticulationBody rot;

    void Start()
    {
        rot = GetComponent<ArticulationBody>();
 
        var drive = rot.xDrive;
        drive.stiffness = 10000;
        drive.damping = 1000;
        drive.forceLimit = 1000;
        rot.xDrive = drive;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || 
            Input.GetKeyDown(KeyCode.Alpha3) || 
            Input.GetKeyDown(KeyCode.Alpha5))
        {
            rot.SetDriveTarget(ArticulationDriveAxis.X, 180);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) || 
            Input.GetKeyDown(KeyCode.Alpha4) || 
            Input.GetKeyDown(KeyCode.Alpha6))
        {
            rot.SetDriveTarget(ArticulationDriveAxis.X, -180);
        }
        float x = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).x;
        if(x > 0.5){
            float nows = rot.xDrive.target;
            rot.SetDriveTarget(ArticulationDriveAxis.X, nows+1);
        }
        else if(x < -0.5){
            float nows = rot.xDrive.target;
            rot.SetDriveTarget(ArticulationDriveAxis.X, nows-1);
    }
    }
}
