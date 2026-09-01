using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Transform xrRig;  
    public Transform[] cameraPositions; 
    private int currentIndex = 0;

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            currentIndex = (currentIndex + 1) % cameraPositions.Length;  
            xrRig.position = cameraPositions[currentIndex].position;
            xrRig.rotation = cameraPositions[currentIndex].rotation;
        
        }
    }
}
