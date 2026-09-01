using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class body2 : MonoBehaviour
{
     ArticulationBody rot;
     public GameObject rotw;
    void Start()
    {
        rot = GetComponent<ArticulationBody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            rot.SetDriveTarget(ArticulationDriveAxis.X, -90);
        }
        if(Input.GetKeyDown(KeyCode.Alpha4)){
            rot.SetDriveTarget(ArticulationDriveAxis.X,0);
        }
        float y = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick,OVRInput.Controller.LTouch).y;
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
