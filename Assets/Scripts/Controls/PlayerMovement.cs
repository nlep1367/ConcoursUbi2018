using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float HorizontalAxis = Input.GetAxis("Horizontal");

        if(HorizontalAxis >= float.Epsilon || HorizontalAxis <= -float.Epsilon)
        {
            transform.position += HorizontalAxis * Vector3.right * Speed * Time.deltaTime;
        }

        float VerticalAxis = Input.GetAxis("Vertical");

        if (VerticalAxis >= float.Epsilon || VerticalAxis <= -float.Epsilon)
        {
            transform.position += VerticalAxis * Vector3.forward * Speed * Time.deltaTime;
        }
    }
}
