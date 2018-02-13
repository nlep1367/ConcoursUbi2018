using UnityEngine;
using UnityEngine.Networking;

public class StateSync : NetworkBehaviour
{
    public enum State { White, Red, Blue, Green }

    private MeshRenderer objMeshRenderer;

    [SyncVar]
    private State syncState;
    private State lastState;

    void Start()
    {
        objMeshRenderer = GetComponent<MeshRenderer>();
        syncState = lastState = State.White;
    }

    void FixedUpdate()
    {
        if (hasAuthority)
        {
            InputTest();
        }
        
        SendState();
        ApplyState();
    }

    void InputTest()
    {
        if (Input.GetKeyDown("1"))
        {
            syncState = State.Red;
            objMeshRenderer.material.color = Color.red;
        }
        else if (Input.GetKeyDown("2"))
        {
            syncState = State.Blue;
            objMeshRenderer.material.color = Color.blue;
        }
        else if (Input.GetKeyDown("3"))
        {
            syncState = State.Green;
            objMeshRenderer.material.color = Color.green;
        }
    }

    [Command]
    void Cmd_SendStateToServer(State s)
    {
        syncState = s;
    }

    [ClientCallback]
    void SendState()
    {
        if (hasAuthority)
        {
            if (syncState != lastState)
            {
                Cmd_SendStateToServer(syncState);
                lastState = syncState;
            }
        }
    }

    void ApplyState()
    {
        if (!hasAuthority)
        {
            if (syncState != lastState)
            {
                switch (syncState)
                {
                    case State.Blue:
                        objMeshRenderer.material.color = Color.blue;
                        break;

                    case State.Red:
                        objMeshRenderer.material.color = Color.red;
                        break;

                    case State.Green:
                        objMeshRenderer.material.color = Color.green;
                        break;
                }

                syncState = lastState;
            }
        }
    }
}
