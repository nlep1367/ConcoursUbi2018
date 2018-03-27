using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PickupObject : NetworkBehaviour {
    bool isCarryingObject = false;
    GameObject pickableObject;
    GameObject carriedObject;
    public Transform anchorPoint;
    private HintUI hintUI;
    private Player _player;

    public bool CanDrop = true;

    private void Start()
    {
        hintUI = FindObjectOfType<HintUI>();
        _player = this.GetComponent<Player>();
    }

    public GameObject GetCarriedObject()
    {
        return carriedObject;
    }

    // Update is called once per frame
    void Update() {

        if (!isLocalPlayer)
            return;

        if (isCarryingObject)
        {
            ThrownableObject thrownable = carriedObject.GetComponentInParent<ThrownableObject>();
            if (thrownable != null && thrownable.IsInThrownZone)
            {
                hintUI.Display(KeyCode.Y, "Throw in the garbage");
            }
            else
            {
                if (CanDrop)
                {
                    hintUI.Display(KeyCode.Y, "Drop the object");
                }
            }

            Carry(carriedObject);
            Drop();
        }
        else
        {
            Pickup();
        }
    }

    void Carry(GameObject obj)
    {
        obj.transform.position = anchorPoint.position;
        obj.transform.rotation = anchorPoint.rotation;
    }

    void Pickup()
    {
        if (Input.GetButtonDown("Y"))
        {
            if (pickableObject != null)
            {
                isCarryingObject = true;
                carriedObject = pickableObject;
                _player.ChangeState(StateEnum.GRABBING);

                if(!isServer)
                { 
                    NetworkIdentity ni = carriedObject.GetComponent<NetworkIdentity>();
                    ni.localPlayerAuthority = true;
                    Cmd_GetAutority(ni);
                }
            }
        }
    }

    [Command]
    public void Cmd_GetAutority(NetworkIdentity ni)
    {
        NetworkConnection temp = GetComponent<NetworkIdentity>().connectionToClient;
        ni.localPlayerAuthority = true;

        ni.AssignClientAuthority(temp);
    }


    [Command]
    public void Cmd_RemoveAutority(NetworkIdentity ni)
    {
        NetworkConnection temp = GetComponent<NetworkIdentity>().connectionToClient;

        ni.RemoveClientAuthority(temp);
    }

    public void InsertKeyInDoor()
    {
        isCarryingObject = false;
        carriedObject = null;
    }

    void Drop()
    {   
        if (Input.GetButtonDown("Y"))
        {
            ThrownableObject thrownable = carriedObject.GetComponentInParent<ThrownableObject>();
            if (thrownable != null && thrownable.IsInThrownZone)
            {
                thrownable.ThrowAway();
                hintUI.Hide();
            }

            if (!isServer)
            {
                NetworkIdentity ni = carriedObject.GetComponent<NetworkIdentity>();
                Cmd_RemoveAutority(ni);
            }
            isCarryingObject = false;
            carriedObject = null;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (GetComponent<ObjectSync>().hasAuthority && collision.gameObject.CompareTag("PickableObject"))
        {
            hintUI.Display(KeyCode.Y, "Pick up " + collision.gameObject.name);
            pickableObject = collision.gameObject;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (GetComponent<ObjectSync>().hasAuthority && collision.gameObject.CompareTag("PickableObject"))
        {
            hintUI.Hide();
            pickableObject = null;
        }
    }
}
