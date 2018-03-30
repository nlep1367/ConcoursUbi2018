using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ObjectSync : NetworkBehaviour
{
    private const float POS_DIFF_THRESHOLD = .1f;
    private const float ROT_Y_DIFF_THRESHOLD = 5f;

    private Transform objTransform;
    private InGameUI inGameUi;

    [SyncVar]
    private Vector3 syncPos;
    [SyncVar]
    private Quaternion syncRot;

    [SyncVar]
    public bool test = false;

    private Vector3 lastPos;
    private Quaternion lastYRot;

    public float lerpRate = 10.0f;
    private int skipFrame = 0;
    void Start()
    {
        objTransform = GetComponent<Transform>();
        inGameUi = FindObjectOfType<InGameUI>();
    }

    void FixedUpdate()
    {
        if (skipFrame != 2)
        {
            SendMotion();
            LerpMotion();
        }
        else
            skipFrame = -1;

        skipFrame++;
    }

    void LerpMotion()
    {
        if (!hasAuthority)
        {
            objTransform.position = Vector3.Lerp(objTransform.transform.position, syncPos, Time.deltaTime * lerpRate);
            objTransform.rotation = Quaternion.Lerp(objTransform.transform.rotation, syncRot, Time.deltaTime * lerpRate);
        }
    }

    [Command]
    void Cmd_SendMotionToServer(Vector3 pos, Quaternion rot)
    {
        syncPos = pos;
        syncRot = rot;
    }

    [ClientCallback]
    void SendMotion()
    {
        if (hasAuthority)
        {
            if (ShouldMove() || ShouldRotate())
            {
                Cmd_SendMotionToServer(objTransform.position, objTransform.rotation);
                lastPos = objTransform.position;
                lastYRot = objTransform.rotation;
            }
        }
    }

    private bool ShouldMove()
    {
        return Vector3.Distance(objTransform.position, lastPos) > POS_DIFF_THRESHOLD;
    }

    private bool ShouldRotate()
    {
        return Quaternion.Angle(objTransform.rotation, lastYRot) > ROT_Y_DIFF_THRESHOLD;
    }

    [ClientRpc]
    public void Rpc_SetMotion(Vector3 pos, Quaternion rot)
    {
        if (objTransform)
        {
            objTransform.position = pos;
            objTransform.rotation = rot;
        }
        lastPos = syncPos = pos;
        syncRot = rot;
        lastYRot = rot;
    }


    [ClientRpc]
    public void Rpc_SetObjectActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    [ClientRpc]
    public void Rpc_SetRotation(Quaternion rot)
    {
        if (objTransform)
        {
            objTransform.rotation = rot;
        }
        syncRot = rot;
        lastYRot = rot;
    }

    [ClientRpc]
    public void Rpc_Destroy()
    {
        Destroy(gameObject);
    }
}
