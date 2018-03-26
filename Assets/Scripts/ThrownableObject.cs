using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ThrownableObject : NetworkBehaviour {

    public bool IsInThrownZone = false;

    public void ThrowAway()
    {
        gameObject.GetComponent<ObjectSync>().Rpc_Destroy();
    }
}
