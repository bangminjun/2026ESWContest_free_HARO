    using UnityEngine;

    public class OVRRotationFi : MonoBehaviour
    {
        private Vector3 fixedPosition;
        private Quaternion fixedRotation;

        void Start()
        {
            // 시작 시점의 원래 위치/회전 저장
            fixedPosition = transform.position;
            fixedRotation = transform.rotation;
        }

        void LateUpdate()
        {
            if (OVRManager.isHmdPresent)
            {
                transform.position = fixedPosition;
                transform.rotation = fixedRotation;
            }
            if (!OVRManager.isHmdPresent)
            {
                transform.position = fixedPosition;
                transform.rotation = fixedRotation;
            }
        }
    }
