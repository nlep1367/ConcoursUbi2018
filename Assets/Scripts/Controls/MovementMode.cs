using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MovementMode {
    public static void StrafeMode(Player player, float movementSpeed, float rotationSpeed)
    {
        float HorizontalAxis = Input.GetAxis("Horizontal_Move");

        if (HorizontalAxis >= float.Epsilon || HorizontalAxis <= -float.Epsilon)
        {
            player.RigidBody.AddForce(HorizontalAxis * player.transform.right * movementSpeed * Time.deltaTime, ForceMode.VelocityChange);
        }

        float VerticalAxis = Input.GetAxis("Vertical_Move");

        if (VerticalAxis >= float.Epsilon || VerticalAxis <= -float.Epsilon)
        {
            player.RigidBody.AddForce(VerticalAxis * player.transform.forward * movementSpeed * Time.deltaTime, ForceMode.VelocityChange);
        }

        float HorizontalRotation = Input.GetAxis("Horizontal_Rotation");

        if (HorizontalRotation != 0)
        {
            player.transform.eulerAngles += new Vector3(0, Time.deltaTime * rotationSpeed * HorizontalRotation, 0);
        }
    }

    public static void ForwardMode(Player player, float acceleration, float maxSpeed, float rotationSpeed)
    {
        float horizontalAxis = Input.GetAxis("Horizontal_Move");

        if (horizontalAxis >= float.Epsilon || horizontalAxis <= -float.Epsilon)
        {
            player.RigidBody.AddForce(horizontalAxis * player.transform.right * acceleration * Time.deltaTime, ForceMode.VelocityChange);
        }

        float verticalAxis = Input.GetAxis("Vertical_Move");

        if (verticalAxis >= float.Epsilon || verticalAxis <= -float.Epsilon)
        {
            player.RigidBody.AddForce(verticalAxis * player.transform.forward * acceleration * Time.deltaTime, ForceMode.VelocityChange);
        }

        Vector3 velocityProjection = player.RigidBody.velocity;
        float y = velocityProjection.y;
        velocityProjection.y = 0;
        if (velocityProjection.magnitude > maxSpeed)
        {
            player.RigidBody.velocity = velocityProjection.normalized * maxSpeed + new Vector3(0, y, 0);
        }

        velocityProjection.y = 0;

        if (velocityProjection != Vector3.zero)
        {
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, Quaternion.LookRotation(velocityProjection), Time.deltaTime * 10);
        }
    }

    public static void ForwardModeCamRelative(Player player, float acceleration, float maxSpeed, float rotationSpeed, Camera camera)
    {
        float horizontalAxis = Input.GetAxis("Horizontal_Move");

        Vector3 cameraForwardProjection = camera.transform.forward;
        cameraForwardProjection.y = 0;
        cameraForwardProjection.Normalize();

        Vector3 cameraRightProjection = camera.transform.right;
        cameraRightProjection.y = 0;
        cameraRightProjection.Normalize();

        Vector3 ySpeed = new Vector3(0, player.RigidBody.velocity.y,0);
        player.RigidBody.velocity = Vector3.zero;

        if (horizontalAxis >= float.Epsilon || horizontalAxis <= -float.Epsilon)
        {
            player.RigidBody.velocity += (horizontalAxis * cameraRightProjection * maxSpeed);
           // player.RigidBody.AddForce(horizontalAxis * cameraRightProjection * acceleration * Time.deltaTime, ForceMode.VelocityChange);
        }

        float verticalAxis = Input.GetAxis("Vertical_Move");

        if (verticalAxis >= float.Epsilon || verticalAxis <= -float.Epsilon)
        {
            player.RigidBody.velocity += (verticalAxis * cameraForwardProjection * maxSpeed);
           // player.RigidBody.AddForce(verticalAxis * cameraForwardProjection * acceleration * Time.deltaTime, ForceMode.VelocityChange);
        }

        Vector3 velocityProjection = player.RigidBody.velocity;
        if (velocityProjection.magnitude > maxSpeed)
        {
            player.RigidBody.velocity = velocityProjection.normalized * maxSpeed;
        }

        player.RigidBody.velocity += ySpeed;
        velocityProjection.y = 0;

        if (velocityProjection != Vector3.zero)
        {
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, Quaternion.LookRotation(velocityProjection), Time.deltaTime * 10);
        }
    }
}
