using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class EndGame : NetworkBehaviour {

    [SyncVar]
    public bool Ended = false;

    void Start()
    {
    }

    void OnTriggerStay(Collider other)
    {
        if (isServer)
        {
            if (StaticInput.GetButtonDown("X") && GameEssentials.IsGirl(other))
            {
                Rpc_ENDTHEMALL();
            }
        }
    }

    [ClientRpc]
    void Rpc_ENDTHEMALL()
    {
        GameEnd ge = GameObject.FindObjectOfType<GameEnd>();
        ge.Finish();
    }
}
