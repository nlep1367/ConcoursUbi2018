using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CarPoolScript : NetworkBehaviour
{

    public float spawningTime = 5f;
    public Path path;
    private NetworkSpawner carSpawner;

	// Use this for initialization
	void Start () {
        carSpawner = GetComponentInParent<NetworkSpawner>();
        InvokeRepeating("Roll", 0, spawningTime);
	}

    [Server]
    void Roll()
    {
        GameObject obj = carSpawner.GetFromPool();

        if (obj == null)
            return;

        CarEngine car = obj.GetComponent<CarEngine>();
        car.Initialize(carSpawner, path);

        obj.GetComponent<ObjectSync>().Rpc_SetObjectActive(true);
    }
}
