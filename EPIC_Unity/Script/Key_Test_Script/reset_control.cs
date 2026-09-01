using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reset_control : MonoBehaviour
{
    ArticulationBody body;
    public GameObject arm1;
    public GameObject arm2;
    public GameObject arm3;

    void Start()
    {
        body = GetComponent<ArticulationBody>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha9)){
            body.SetDriveTarget(ArticulationDriveAxis.X, 0);
        }
    }
}
