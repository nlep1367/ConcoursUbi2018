using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPoolScript : MonoBehaviour {

    public float spawningTime = 5f;
    public Vector3 spawningPosition = new Vector3(0, 0, 0);

    public Path path;

	// Use this for initialization
	void Start () {
        InvokeRepeating("Roll", 0, spawningTime);
	}

    void Roll()
    {
        GameObject obj = ObjectPoolScript.current.GetPooledObject();

        if (obj == null) return;

        CarEngine car = obj.GetComponent<CarEngine>();
        car.Initialize(transform.position + spawningPosition, transform.rotation, path);        

        obj.SetActive(true);
    }
}
