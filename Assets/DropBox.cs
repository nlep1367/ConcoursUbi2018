using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DropBox : NetworkBehaviour {

    public FleeAI ai;

    private void Start()
    {
        ai.WasSpooked += Drop;
    }

    private void Drop()
    {
        gameObject.GetComponent<ObjectSync>().Rpc_SetObjectActive(true);
    }
}
