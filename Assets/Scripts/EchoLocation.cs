using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoLocation : MonoBehaviour {

    private const int MAX_POINT = 10;

    public float radius = 3.5f;
    private Vector3 positionWithOffset;

    private List<Vector2> pointsEcho = new List<Vector2>();

    
	// Use this for initialization
	void Start () {
       Vector3 size = GetComponent<MeshCollider>().bounds.size;
        positionWithOffset = transform.position + new Vector3(0, - size.y / 2.0f, 0);
	}
	
	// Update is called once per frame
	void Update () {
        GeneratePoint();
    }

    private void GeneratePoint()
    {
        if(pointsEcho.Count != MAX_POINT)
        {
            Vector2 pos = new Vector2(transform.position.x, transform.position.z) + Random.insideUnitCircle * radius;
            pointsEcho.Add(pos);
        }
    }

    private void OnDrawGizmos()
    {            
        UnityEditor.Handles.DrawWireDisc(positionWithOffset, transform.up, radius);

        foreach(Vector2 v in pointsEcho)
        {
            Vector3 v3 = new Vector3(v.x, 270, v.y);
            UnityEditor.Handles.DrawWireDisc(v3, transform.up, 1);
        }
    }
}
