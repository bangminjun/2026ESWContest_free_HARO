using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pump_body : MonoBehaviour
{
    ArticulationBody rot;
    public GameObject rotw;

    void Start()
    {
        rot = GetComponent<ArticulationBody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            rot.SetDriveTarget(ArticulationDriveAxis.X, 90);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            rot.SetDriveTarget(ArticulationDriveAxis.X, 0);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            rot.SetDriveTarget(ArticulationDriveAxis.X, 180);
        }

        float y = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).y; 
        if (y > 0.5)
        {
            float now = rot.xDrive.target;
            rot.SetDriveTarget(ArticulationDriveAxis.X, now - 1);
        }
        else if (y < -0.5)
        {
            float now = rot.xDrive.target;
            rot.SetDriveTarget(ArticulationDriveAxis.X, now + 1);
        }
    }
}