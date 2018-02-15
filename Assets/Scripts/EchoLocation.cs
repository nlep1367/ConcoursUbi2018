using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoLocation : MonoBehaviour {

    public int maxPoints = 20;
    public float minDelayNextEcho = 0.1f;
    public float maxDelayNextEcho = 0.2f;
    public float radius = 3.5f;

    private Vector3 positionWithOffset;

    private Queue<Vector2> pointsEcho = new Queue<Vector2>();
    private float nextUpdate = 0.5f;

    
	// Use this for initialization
	void Start () {
       Vector3 size = GetComponent<MeshCollider>().bounds.size;
       positionWithOffset = transform.position + new Vector3(0, - size.y / 2.0f, 0);
	}
	
	// Update is called once per frame
	void Update () {
        if(Time.time >= nextUpdate)
        {
            if (pointsEcho.Count != maxPoints)
            {
                GeneratePoint();
            }

            nextUpdate += Random.Range(minDelayNextEcho, maxDelayNextEcho);
        }
    }

    private void GeneratePoint()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.z) + Random.insideUnitCircle * radius;
        pointsEcho.Enqueue(pos);

        if (pointsEcho.Count == maxPoints)
        {
            pointsEcho.Dequeue();
        }
    }

    /*
     * need to remove this function when MP's echolocation visual will be integrated and add another
     * function to instanciate them.
     */
    private void OnDrawGizmos()
    {            
        UnityEditor.Handles.DrawWireDisc(positionWithOffset, transform.up, radius);

        foreach(Vector2 v in pointsEcho)
        {
            Vector3 centerEcho = new Vector3(v.x, positionWithOffset.y, v.y);


            UnityEditor.Handles.DrawWireDisc(centerEcho, transform.up, .5f);
        }
    }
}
