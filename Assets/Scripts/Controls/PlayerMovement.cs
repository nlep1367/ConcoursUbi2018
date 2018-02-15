using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public float MovementSpeed;

    public float RotationSpeed;

	// Use this for initialization
	void Start () {
		
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
            transform.position += HorizontalAxis * transform.right * MovementSpeed * Time.deltaTime;
        }

        float VerticalAxis = Input.GetAxis("Vertical_Move");

        if (VerticalAxis >= float.Epsilon || VerticalAxis <= -float.Epsilon)
        {
            transform.position += VerticalAxis * transform.forward * MovementSpeed * Time.deltaTime;
        }

        float HorizontalRotation = Input.GetAxis("Horizontal_Rotation");

        if (HorizontalRotation != 0)
        {
            transform.eulerAngles += new Vector3(0, Time.deltaTime * RotationSpeed * HorizontalRotation, 0 );
        }


    }
}
