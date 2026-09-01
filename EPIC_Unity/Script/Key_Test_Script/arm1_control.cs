using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arm1_control : MonoBehaviour
{
    ArticulationBody body;
    public GameObject arm1;
    void Start()
    {
        body = GetComponent<ArticulationBody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            body.SetDriveTarget(ArticulationDriveAxis.X, 45);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            body.SetDriveTarget(ArticulationDriveAxis.X,-45);
        }
        if(Input.GetKeyDown(KeyCode.Alpha8)){
            body.SetDriveTarget(ArticulationDriveAxis.X,0);
        }
    }
}
