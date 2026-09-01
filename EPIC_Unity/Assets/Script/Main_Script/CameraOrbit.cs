using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform center;
    public Transform trackingSpace;
    public float orbitSpeed = 50f;
    public float direction = 0f;

    public float heightOffset = 5f;
    public float orbitRadius = 10f;

    private float angle = 0f;

    void LateUpdate()
    {
        if (center == null || trackingSpace == null) return;
        if (direction == 0f) return;

        angle += orbitSpeed * direction * Time.deltaTime;

        // 위치만 궤도 따라 이동
        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(
            Mathf.Sin(rad) * orbitRadius,
            heightOffset,
            Mathf.Cos(rad) * orbitRadius
        );

        trackingSpace.position = center.position + offset;
        
        Vector3 lookDirection = center.position - trackingSpace.position;
        lookDirection.y = 0;  
        if (lookDirection != Vector3.zero)
            trackingSpace.rotation = Quaternion.LookRotation(lookDirection);
    }
}
