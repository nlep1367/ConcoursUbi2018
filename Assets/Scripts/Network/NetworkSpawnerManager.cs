using UnityEngine;

public class NetworkSpawnerManager : MonoBehaviour {

    private static NetworkSpawnerManager instance;

    public NetworkSpawner CarSpawner;

    private NetworkSpawnerManager() { }

    public static NetworkSpawnerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<NetworkSpawnerManager>();
            }
            return instance;
        }
    }
}
