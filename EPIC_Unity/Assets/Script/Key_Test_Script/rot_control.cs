using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rot_control : MonoBehaviour
{
    ArticulationBody body;
    public GameObject rot;

    void Start()
    {
        body = GetComponent<ArticulationBody>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0)){
            body.SetDriveTarget(ArticulationDriveAxis.X, 180);
        }
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            body.SetDriveTarget(ArticulationDriveAxis.X,-180);
        }
        if(Input.GetKeyDown(KeyCode.Alpha8)){
            body.SetDriveTarget(ArticulationDriveAxis.X,0);
        }
    }
}
