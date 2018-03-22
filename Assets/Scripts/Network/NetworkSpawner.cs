using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkSpawner : NetworkBehaviour {

    public GameObject prefab;
    public int maxInstances;
    public Transform spawnPoint;
    public float angleSpawn;

    private List<GameObject> instances = new List<GameObject>();
    private Queue<GameObject> availables = new Queue<GameObject>();

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    private int obstructedCounter = 0;

    void Start()
    {
        if (NetworkServer.active)
        {
            spawnPosition = spawnPoint.transform.position;
            spawnRotation = Quaternion.Euler(0, angleSpawn, 0);
        }
    }

    [Server]
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            obstructedCounter++;
        }
    }

    [Server]
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            obstructedCounter--;
        }
    }

    [Server]
    GameObject InstantiatePrefab()
    {
        GameObject go = Instantiate(prefab, spawnPosition, spawnRotation);
        instances.Add(go);
        NetworkServer.Spawn(go);

        return go;
    }

    [Server]
    public GameObject GetFromPool()
    {
        if (obstructedCounter != 0 || maxInstances == instances.Count)
            return null;

        GameObject go = (availables.Count == 0) ? InstantiatePrefab() : availables.Dequeue();

        go.transform.position = spawnPosition;
        go.transform.rotation = spawnRotation;
        go.GetComponent<ObjectSync>().Rpc_SetMotion(spawnPosition, spawnRotation);
        go.GetComponent<ObjectSync>().Rpc_SetObjectActive(true);

        return go;
    }

    [Server]
    public void ReturnToPool(GameObject go)
    {
        go.GetComponent<ObjectSync>().Rpc_SetObjectActive(false);
        availables.Enqueue(go);
    }
}
