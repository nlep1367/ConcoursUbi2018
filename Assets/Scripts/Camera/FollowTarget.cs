using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : BaseCamera {

    public float CameraOffsetDistance = 15.0f;
    public float CameraOffsetAngle = 70;


    public float MinFollowSpeed = 1f;
    public float MaxFollowSpeed = 10f;

    private float CurrentFollowSpeed;

    public Transform TargetToFollow;


    private Vector3 ActualTarget;
    private Vector3 CameraOffset;

    // Use this for initialization
    void Start() {
        CameraOffset = new Vector3(0.0f, Mathf.Sin(Mathf.Deg2Rad * CameraOffsetAngle), -Mathf.Cos(Mathf.Deg2Rad * CameraOffsetAngle)) * CameraOffsetDistance;

        ActualTarget = TargetToFollow.position;
        CurrentFollowSpeed = MinFollowSpeed;
    }

    // Update is called once per frame
    void LateUpdate() {
        float Distance = (ActualTarget - TargetToFollow.position).magnitude;

        if (Distance > float.Epsilon)
        {

            Vector3 Dir = TargetToFollow.position - ActualTarget;
            Vector3 ImpendingMovement = Dir * Time.deltaTime * CurrentFollowSpeed;

            if (Dir.magnitude < ImpendingMovement.magnitude)
            {
                ActualTarget = TargetToFollow.position;
            }
            else
            {
                ActualTarget += ImpendingMovement;
                transform.position = ActualTarget + CameraOffset;
            }

            transform.LookAt(ActualTarget);
            CurrentFollowSpeed += (MaxFollowSpeed - MinFollowSpeed) * Time.deltaTime / 5;

        }
        else
        {
            CurrentFollowSpeed -= (MaxFollowSpeed - MinFollowSpeed) * Time.deltaTime;
        }

        CurrentFollowSpeed = Mathf.Clamp(CurrentFollowSpeed, MinFollowSpeed, MaxFollowSpeed);
    }

    void ResetCamera()
    {
        ActualTarget = TargetToFollow.position;
    }
}
