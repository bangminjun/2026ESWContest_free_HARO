using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arm2_control : MonoBehaviour
{
    ArticulationBody body;
    public GameObject arm2;
    void Start()
    {
        body = GetComponent<ArticulationBody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha4)){
            body.SetDriveTarget(ArticulationDriveAxis.X, 45);
        }
        if(Input.GetKeyDown(KeyCode.Alpha5)){
            body.SetDriveTarget(ArticulationDriveAxis.X,-45);
        }
        if(Input.GetKeyDown(KeyCode.Alpha8)){
            body.SetDriveTarget(ArticulationDriveAxis.X,0);
        }
    }
}
