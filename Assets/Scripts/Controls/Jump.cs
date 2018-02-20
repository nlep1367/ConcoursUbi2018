using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour {
    private const float GROUNDED_SKIN = 0.1f;

    Rigidbody rigidBody;
    public float MaxHeight;

    private float jumpSpeed;

    private bool grounded;
    private bool isRequestingJump;

    private Vector3 objectGroundToPosition;
    private Vector3 groundedBox;

    public LayerMask mask;

    // Use this for initialization
    void Start()
    {
        rigidBody = this.GetComponent<Rigidbody>();
        jumpSpeed = Mathf.Sqrt(Mathf.Abs(2* MaxHeight * Physics2D.gravity.y));
        grounded = false;

        Vector3 playerSize = GetComponent<BoxCollider>().size;

        objectGroundToPosition = playerSize.y * Vector3.up /2f;

        groundedBox = new Vector3(playerSize.x, GROUNDED_SKIN, playerSize.z);
    }

    // Update is called once per frame
    void Update()
    {
        bool requestIsJumping = Input.GetButtonDown("Jump");

        if (grounded && requestIsJumping)
        {
            isRequestingJump = true;
        }
    }

    private void FixedUpdate()
    {
        //Look for jump command
        if (isRequestingJump)
        {
            rigidBody.AddForce(Vector3.up * jumpSpeed, ForceMode.VelocityChange);
            isRequestingJump = false;
            grounded = false;
        } 
        // Update grounded value
        grounded = CheckIfGrounded();
    }

    private bool CheckIfGrounded()
    {
        Vector3 boxCenter = (this.transform.position - objectGroundToPosition);
        return Physics.OverlapBox(boxCenter, groundedBox / 2.0f, Quaternion.identity, mask).Length > 0;
    }
}
