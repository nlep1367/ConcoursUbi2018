using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum TrafficLightState
{
    Red,
    Green,
    Yellow,
    Count,
}

public class TrafficLight : NetworkBehaviour {

    [SyncVar]
    public TrafficLightState syncState;
    private TrafficLightState lastState = TrafficLightState.Count;

    public Material lights;

    // Use this for initialization
    void Awake () {
        //lights = GetComponent<Renderer>().materials[1];
    }

    public TrafficLightState getLightState()
    {
        return syncState;
    }

    [ClientRpc]
    public void Rpc_SendStateToServer(TrafficLightState tls)
    {
        syncState = tls;
        SetLightMaterial();
    }

    public void SetState(TrafficLightState tls)
    {
        syncState = tls;
        SetLightMaterial();
    }

/*
[ClientCallback]
void SendState()
{
    if (syncState != lastState)
    {
        Cmd_SendStateToServer(syncState);
        lastState = syncState;
    }
}
*/
public void SetLightMaterial()
    {
        if (syncState != lastState)
        {
            switch (syncState)
            {
                case TrafficLightState.Green:
                    lights.mainTextureOffset = new Vector2(0.75f, 0);
                    break;

                case TrafficLightState.Yellow:
                    lights.mainTextureOffset = new Vector2(0, 0);
                    break;

                case TrafficLightState.Red:
                    if(GetComponentInParent<TrafficLightNode>().hasPedestrian)
                    {
                        lights.mainTextureOffset = new Vector2(0.25f, 0);
                    }
                    else
                    {
                        lights.mainTextureOffset = new Vector2(0.50f, 0);
                    }
                    
                    break;
            }
            lastState = syncState;
        }
    }
}