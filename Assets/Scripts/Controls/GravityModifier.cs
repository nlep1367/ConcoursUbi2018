using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Makes jump feel less floaty by accelerating descent and allow short jump
public class GravityModifier : MonoBehaviour {
    public float FallModifier;
    public float LowJumpModifier;
    public float NormalGravityModifier;
    private Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
        rigidBody = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (rigidBody.velocity.y < 0)
        {
            Vector3 force = Vector3.up * Physics.gravity.y * FallModifier * Time.deltaTime;
            rigidBody.AddForce(force, ForceMode.VelocityChange);
        }
        else if (rigidBody.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            Vector3 force = Vector3.up * Physics.gravity.y * LowJumpModifier * Time.deltaTime;
            rigidBody.AddForce(force, ForceMode.VelocityChange);
        }
        else
        {
            Vector3 force = Vector3.up * Physics.gravity.y * NormalGravityModifier * Time.deltaTime;
            rigidBody.AddForce(force, ForceMode.VelocityChange);
        }
    }
}
