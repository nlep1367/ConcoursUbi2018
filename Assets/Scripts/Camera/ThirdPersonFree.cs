using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonFree : ThirdPerson {

    public float DistanceOffset = 5;
    public Vector3 PositionOffset;

    public float UpAndDownSpeed;
    public float CameraAngleDegreeMinMax;

    public float CameranAngleDegree;
    public float CameraLateralDegree;

    public float CameraReturnSpeed = 1;

    private Vector3 ActualTarget;
    private float StartDegree;

    // Use this for initialization
    void Start()
    {
        StartDegree = CameranAngleDegree;
    }

    // Update is called once per frame
    void Update()
    {
        float VerticalAxis = Input.GetAxis("Vertical_Move");

        if (VerticalAxis >= float.Epsilon || VerticalAxis <= -float.Epsilon)
        {
            ReturnToOriginal(VerticalAxis);
        }

        float VerticalRotation = Input.GetAxis("Vertical_Rotation");

        if (VerticalRotation >= float.Epsilon || VerticalRotation <= -float.Epsilon)
        {
            CameranAngleDegree += VerticalRotation * Time.deltaTime * UpAndDownSpeed;

            CameranAngleDegree = Mathf.Clamp(CameranAngleDegree, StartDegree - CameraAngleDegreeMinMax, StartDegree + CameraAngleDegreeMinMax);
        }

        float HorizontalRotation = Input.GetAxis("Horizontal_Rotation");

        if (HorizontalRotation >= float.Epsilon || HorizontalRotation <= -float.Epsilon)
        {
            CameraLateralDegree += HorizontalRotation * Time.deltaTime * UpAndDownSpeed;
        }

        ActualTarget = Player.transform.position + Player.transform.right * PositionOffset.x;
        ActualTarget += Player.transform.up * PositionOffset.y;
        ActualTarget += Player.transform.forward * PositionOffset.z;

        transform.position = ActualTarget;

        float Radian = Mathf.Deg2Rad * CameranAngleDegree;

        // CameraOffset
        Vector3 lateralOffset = Quaternion.Euler(0, CameraLateralDegree, 0) * -Player.forward;
        Vector3 CalculatedOffset = lateralOffset * Mathf.Cos(Radian);
        CalculatedOffset += Player.up * Mathf.Sin(Radian);
        CalculatedOffset.Normalize();
        CalculatedOffset *= DistanceOffset;

        transform.position += CalculatedOffset;

        transform.LookAt(ActualTarget);
    }

    public void ReturnToOriginal(float emphasis)
    {
        if(Mathf.Abs(CameraLateralDegree) < CameraReturnSpeed)
        {
            CameraLateralDegree = 0;
        }
        else
        {
            CameraLateralDegree -= Mathf.Abs(CameraReturnSpeed * emphasis) * Mathf.Sign(CameraLateralDegree);
        }
    }
}
