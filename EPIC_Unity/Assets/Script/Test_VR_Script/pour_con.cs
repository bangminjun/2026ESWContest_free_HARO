using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pour_con : MonoBehaviour
{
    public ParticleSystem ps;
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            ps.Play();
        }
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            ps.Stop();
        }
        
    }
}
