using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonIndependant : ThirdPerson {
    public float DistanceOffset = 5;
    public Vector3 BaseOffsetVector = new Vector3(0,1,0);

    public float VerticalSpeed = 5f;
    public float LateralSpeed = 5f;

    public float ReturnSpeed = 0.0001f;

    public float MaxY = 0.8f;
    public float MinY = -0.2f;
    
    Vector3 _offsetVector;

	// Update is called once per frame
	void Update () {
        float verticalRotation = Input.GetAxis("Vertical_Rotation");

        if (verticalRotation != 0)
        {
            Vector3 tryOffset =  Quaternion.AngleAxis(verticalRotation * VerticalSpeed, this.transform.right) * _offsetVector;
            float tryOffsetY = Vector3.Dot(tryOffset, Vector3.up);

            if(tryOffsetY > MaxY)
            {
                float offsetY = Vector3.Dot(_offsetVector, Vector3.up);
                float target = (MaxY - offsetY) / (tryOffsetY - offsetY);

                tryOffset = Vector3.Slerp(_offsetVector, tryOffset, target);

            }

            if (tryOffsetY < MinY)
            {
                float offsetY = Vector3.Dot(_offsetVector, Vector3.up);
                float target = (MinY - offsetY) / (tryOffsetY - offsetY);

                tryOffset = Vector3.Slerp(_offsetVector, tryOffset, target);

            }

            _offsetVector = tryOffset;
            _offsetVector.Normalize();
        }

        float horizontalRotation = Input.GetAxis("Horizontal_Rotation");

        if (horizontalRotation != 0)
        {
            _offsetVector = Quaternion.AngleAxis(horizontalRotation * LateralSpeed, Vector3.up) * _offsetVector;
            _offsetVector.Normalize();
        }
        Vector3 offset = (_offsetVector + BaseOffsetVector).normalized*DistanceOffset;

        if(Input.GetButton("ReturnCamera"))
        {
            GoBackToFront();
        }

        this.transform.position = Player.transform.position + SpringCamera(offset);
        this.transform.LookAt(Player);
    }

    public override void SetPlayer(Transform Player)
    {
        base.SetPlayer(Player);
        _offsetVector = -Player.forward;
    }

    private void GoBackToFront()
    {
        _offsetVector = Vector3.RotateTowards(_offsetVector, -Player.forward, ReturnSpeed, 0.0f);
    }

    private Vector3 SpringCamera(Vector3 offset)
    {
        RaycastHit hit;
        if(Physics.Raycast(Player.transform.position, offset, out hit, DistanceOffset, LayerMask.GetMask("Blocker")))
        {
            return offset.normalized * hit.distance;
        }
        return offset;
    }
}
