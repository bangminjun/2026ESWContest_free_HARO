using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pump_rot : MonoBehaviour
{
     ArticulationBody rot;
     public GameObject rotw;
    void Start()
    {
        rot = GetComponent<ArticulationBody>();
    }
 
    void Update()
    {
        float x = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick,OVRInput.Controller.RTouch).x;
        if(x > 0.5){
            float now = rot.xDrive.target;
            rot.SetDriveTarget(ArticulationDriveAxis.X, now+1);
        }
        else if(x < -0.5){
            float now = rot.xDrive.target;
            rot.SetDriveTarget(ArticulationDriveAxis.X, now-1);
        }
    }
}
