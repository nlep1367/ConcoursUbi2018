using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ObjectSync : NetworkBehaviour
{
    private const float POS_DIFF_THRESHOLD = .1f;
    private const float ROT_Y_DIFF_THRESHOLD = 5f;

    private Transform objTransform;

    [SyncVar]
    private Vector3 syncPos;
    [SyncVar]
    private float syncYRot;

    private Vector3 lastPos;
    private Quaternion lastYRot;

    public float lerpRate = 10.0f;

    void Start()
    {
        objTransform = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        SendMotion();
        LerpMotion();
    }

    void LerpMotion()
    {
        if (!hasAuthority)
        {
            objTransform.position = Vector3.Lerp(objTransform.transform.position, syncPos, Time.deltaTime * lerpRate);
            objTransform.rotation = Quaternion.Lerp(objTransform.transform.rotation, Quaternion.Euler(new Vector3(0, syncYRot, 0)), Time.deltaTime * lerpRate);
        }
    }

    [Command]
    void Cmd_SendMotionToServer(Vector3 pos, float rot)
    {
        syncPos = pos;
        syncYRot = rot;
    }

    [ClientCallback]
    void SendMotion()
    {
        if (hasAuthority)
        {
            if (ShouldMove() || ShouldRotate())
            {
                Cmd_SendMotionToServer(objTransform.position, objTransform.localEulerAngles.y);
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
        syncYRot = rot.eulerAngles.y;
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
        syncYRot = rot.eulerAngles.y;
        lastYRot = rot;
    }

    [ClientRpc]
    public void Rpc_Destroy()
    {
        Destroy(gameObject);
    }
}
