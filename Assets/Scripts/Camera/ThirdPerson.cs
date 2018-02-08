using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPerson : BaseCamera {

    public float DistanceOffset = 5;
    public Vector3 PositionOffset;

    public float LookUpSpeed;
    public float LookUpMax;

    public float Degree;
    public float RotationSpeed;

    public Transform Player;

    private Vector3 ActualTarget;
    private float StartDegree;
   
	// Use this for initialization
	void Start () {
        StartDegree = Degree;

    }
	
	// Update is called once per frame
	void Update () {

        float VerticalRotation = Input.GetAxis("Vertical_Rotation");

        if (VerticalRotation != 0)
        {
            Degree += VerticalRotation * Time.deltaTime * LookUpSpeed;
            Debug.Log("Horizontal Rotation: " + VerticalRotation);

            Degree = Mathf.Clamp(Degree, StartDegree - LookUpMax, StartDegree + LookUpMax);
        }



        ActualTarget = Player.transform.position + Player.transform.right * PositionOffset.x;
        ActualTarget += Player.transform.up * PositionOffset.y;
        ActualTarget += Player.transform.forward * PositionOffset.z;

        transform.position = ActualTarget;

        float Radian = Mathf.Deg2Rad * Degree;

        // CameraOffset
        Vector3 CalculatedOffset = -Player.forward * Mathf.Cos(Radian);
        CalculatedOffset += Player.up * Mathf.Sin(Radian);
        CalculatedOffset.Normalize();
        CalculatedOffset *= DistanceOffset;

        transform.position += CalculatedOffset;

        transform.LookAt(ActualTarget);


	}
}
