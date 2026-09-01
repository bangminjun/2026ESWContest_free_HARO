using UnityEngine;

public class OVRRotationFix : MonoBehaviour
{
    void LateUpdate()
    {
        if (OVRManager.isHmdPresent)
        {
            transform.rotation = Quaternion.identity;
        }
         if (!OVRManager.isHmdPresent)
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
