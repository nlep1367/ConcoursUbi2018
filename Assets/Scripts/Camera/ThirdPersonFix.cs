using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonFix : ThirdPerson {
    public float DistanceOffset = 5;
    public Vector3 PositionOffset;

    public float UpAndDownSpeed;
    public float CameraAngleDegreeMinMax;

    public float CameranAngleDegree;

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

        float VerticalRotation = StaticInput.GetAxis("Vertical_Rotation");

        if (VerticalRotation != 0)
        {
            CameranAngleDegree += VerticalRotation * Time.deltaTime * UpAndDownSpeed;

            CameranAngleDegree = Mathf.Clamp(CameranAngleDegree, StartDegree - CameraAngleDegreeMinMax, StartDegree + CameraAngleDegreeMinMax);
        }


        ActualTarget = Player.transform.position + Player.transform.right * PositionOffset.x;
        ActualTarget += Player.transform.up * PositionOffset.y;
        ActualTarget += Player.transform.forward * PositionOffset.z;

        transform.position = ActualTarget;

        float Radian = Mathf.Deg2Rad * CameranAngleDegree;

        // CameraOffset
        Vector3 CalculatedOffset = -Player.forward * Mathf.Cos(Radian);
        CalculatedOffset += Player.up * Mathf.Sin(Radian);
        CalculatedOffset.Normalize();
        CalculatedOffset *= DistanceOffset;

        transform.position += CalculatedOffset;

        transform.LookAt(ActualTarget);
    }
}
