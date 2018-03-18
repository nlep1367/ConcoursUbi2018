using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class BlockerAI : MonoBehaviour {

    public Transform StartPosition;
    public Transform EndPosition;

    public AIVision Vision;
    public IdleBehaviour Idle;

    private Vector3 Destination;
    public float MinDistance = 1;

    public float speed;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update ()
    {
        Vector3 Target;
        
        if (Vision.SeeSomething(out Target))
        {
            Vector3 Path = EndPosition.position - StartPosition.position;
            float dot = Vector3.Dot(StartPosition.position - Target, Path);

            if (dot > 1)
                dot = 1;

            if (dot > 0)
            {
                Destination = StartPosition.position + (dot * Path);
            }
        }
        else
        {
            Destination = Idle.Process();
        }

        Vector3 Direction = transform.position - Destination;

        if((Direction).magnitude > MinDistance)
        {
            Direction.Normalize();

            transform.position += (Direction * speed * Time.deltaTime);
        }

        transform.LookAt(Target);
	}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(StartPosition.position, EndPosition.position);

        Gizmos.color = Color.blue;
        Gizmos.DrawCube(StartPosition.position, new Vector3(0.5f, 0.5f, 0.5f));
        Gizmos.DrawCube(EndPosition.position, new Vector3(0.5f, 0.5f, 0.5f));
    }
}
