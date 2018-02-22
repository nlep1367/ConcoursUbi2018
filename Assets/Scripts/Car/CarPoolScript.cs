using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CarPoolScript : NetworkBehaviour
{

    public float spawningTime = 5f;
    public Path path;

	// Use this for initialization
	void Start () {
        InvokeRepeating("Roll", 0, spawningTime);
	}

    [Server]
    void Roll()
    {
        GameObject obj = NetworkSpawnerManager.Instance.CarSpawner.GetFromPool();

        CarEngine car = obj.GetComponent<CarEngine>();
        car.Initialize(path);

        obj.GetComponent<ObjectSync>().Rpc_SetObjectActive(true);
    }
}
