using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public float MovementSpeed;

    public float RotationSpeed;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        if(!isLocalPlayer)
        {
            return;
        }

        float HorizontalAxis = Input.GetAxis("Horizontal_Move");

        if(HorizontalAxis >= float.Epsilon || HorizontalAxis <= -float.Epsilon)
        {
            rb.AddForce(HorizontalAxis * transform.right * MovementSpeed * Time.deltaTime, ForceMode.VelocityChange);
        }

        float VerticalAxis = Input.GetAxis("Vertical_Move");

        if (VerticalAxis >= float.Epsilon || VerticalAxis <= -float.Epsilon)
        {
            rb.AddForce(VerticalAxis * transform.forward * MovementSpeed * Time.deltaTime, ForceMode.VelocityChange);
        }

        float HorizontalRotation = Input.GetAxis("Horizontal_Rotation");

        if (HorizontalRotation != 0)
        {
            transform.eulerAngles += new Vector3(0, Time.deltaTime * RotationSpeed * HorizontalRotation, 0 );
        }


    }
}
