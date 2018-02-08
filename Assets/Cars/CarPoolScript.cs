using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WayPoint
{
    public GameObject point;

    public float speed;

    public bool isCirculationLight;

}

public class CarPoolScript : MonoBehaviour {

    public float spawningTime = 2f;

    public List<WayPoint> wayPoints;

	// Use this for initialization
	void Start () {
        wayPoints = new List<WayPoint>();
        InvokeRepeating("Roll", 0, spawningTime);
	}
	
	void Roll()
    {
        GameObject obj = ObjectPoolScript.current.GetPooledObject();

        if (obj == null) return;

        obj.transform.position = transform.position + new Vector3(0,-1,5);
        obj.transform.rotation = transform.rotation;
        obj.SetActive(true);
    }
}
