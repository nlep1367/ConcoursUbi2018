using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DropBox : NetworkBehaviour {

    public FleeAI ai;
    private Vector3 initialPos;
    [SyncVar]
    private bool shouldDrop = false;

    private void Start()
    {
        ai.WasSpooked += Drop;
        initialPos = gameObject.transform.position;
    }

    void Update()
    {
        if (!shouldDrop)
        {
            gameObject.transform.position = initialPos;
        }
    }

    private void Drop()
    {
        Cmd_Drop();
        shouldDrop = true;
    }

    [Command]
    private void Cmd_Drop()
    {
        shouldDrop = true;
    }
}
