using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ThrownableObject : NetworkBehaviour {

    bool isInThrownZone = false;

    public void ThrowAway()
    {
        if (isInThrownZone)
            gameObject.GetComponent<ObjectSync>().Rpc_Destroy();
    }

    public void SetIsInThrownZone(bool val)
    {
        isInThrownZone = val;
    }
}
