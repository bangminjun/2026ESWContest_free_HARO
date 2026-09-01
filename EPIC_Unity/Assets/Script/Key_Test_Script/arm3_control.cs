using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arm3_control : MonoBehaviour
{
    ArticulationBody body;
    public GameObject arm3;
    void Start()
    {
        body = GetComponent<ArticulationBody>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha6)){
            body.SetDriveTarget(ArticulationDriveAxis.X, 45);
        }
        if(Input.GetKeyDown(KeyCode.Alpha7)){
            body.SetDriveTarget(ArticulationDriveAxis.X,-45);
        }
        if(Input.GetKeyDown(KeyCode.Alpha8)){
            body.SetDriveTarget(ArticulationDriveAxis.X,0);
        }
    }
}
