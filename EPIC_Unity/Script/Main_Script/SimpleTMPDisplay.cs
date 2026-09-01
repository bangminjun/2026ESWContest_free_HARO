using TMPro;
using UnityEngine;

public class SimpleTMPDisplay : MonoBehaviour
{
    public ArticulationBody joint;
    public ArticulationBody arm1;
    public ArticulationBody arm2;
    public ArticulationBody arm3;

    public TMP_Text tmp;
    public TMP_Text tmp1;
    public TMP_Text tmp2;
    public TMP_Text tmp3;

    void Update()
    {
        int value = ClampTo180(Mathf.RoundToInt(NormalizeAngle(joint.xDrive.target)));
        tmp.text = $"Rot = {value}";

        int value1 = ClampTo180(Mathf.RoundToInt(NormalizeAngle(arm1.xDrive.target)));

        if (Mathf.Approximately(value1, 0f))
        {
            value1 = 0;
        }
        else
        {
            value1 = -value1;
        }

        tmp1.text = $"Arm1 = {value1}";

        int value2 = ClampTo180(Mathf.RoundToInt(NormalizeAngle(arm2.xDrive.target)));
        tmp2.text = $"Arm2 = {value2}";

        int value3 = ClampTo180(Mathf.RoundToInt(NormalizeAngle(arm3.xDrive.target)));
        tmp3.text = $"Arm3 = {value3}";
    }

    float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle > 180f) angle -= 360f;
        else if (angle < -180f) angle += 360f;
        return angle;
    }

    int ClampTo180(int angle)
    {
        return Mathf.Clamp(angle, -180, 180);
    }
}
