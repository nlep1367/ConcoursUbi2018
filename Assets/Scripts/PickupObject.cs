using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class PickupObject : NetworkBehaviour {
    bool isCarryingObject = false;
    GameObject pickableObject;
    GameObject carriedObject;
    GameObject droppedObject;
    public Transform anchorPoint;
    private HintUI hintUI;
    private Player _player;
    
    public bool CanDrop = true;
    private bool isPickingUp = false;
    private bool isForceHideHintUi = false;

    public float FailSafePickup = 0.75f;
    private float oldObjectMass = 1.0f;
    private RigidbodyConstraints rigidbodyConstraints;

    private void Start()
    {
        hintUI = FindObjectOfType<HintUI>();
        _player = this.GetComponent<Player>();
    }

    public GameObject GetCarriedObject()
    {
        return carriedObject;
    }

    public void SetForceHideHintUi(bool value)
    {
        isForceHideHintUi = value;
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (!isLocalPlayer)
            return;

        if (isCarryingObject && carriedObject == null)
        {
            isCarryingObject = false;
        }

        if (isPickingUp && isCarryingObject)
        {
            ThrownableObject thrownable = carriedObject.GetComponentInParent<ThrownableObject>();

            if(!isForceHideHintUi)
            {
                if (thrownable != null && thrownable.IsInThrownZone)
                {
                    hintUI.Display(Controls.X, "Throw in the garbage");
                }
                else
                {
                    if (CanDrop)
                    {
                        hintUI.Display(Controls.X, "Drop the object");
                    }
                }
            }

            Carry(carriedObject);
            Drop();
        }
        else
        {
            if (pickableObject != null && !pickableObject.GetComponent<ObjectSync>().test)
                Pickup();
            else
            {
                if (pickableObject != null)
                {
                    hintUI.Hide();
                    HighlightObject hob = pickableObject.GetComponentInChildren<HighlightObject>();

                    if (hob != null)
                        hob.ToggleHighlight(false);

                    pickableObject = null;
                }          
            }
                
        }
    }

    [Command]
    public void Cmd_SetAuth(GameObject g, bool t)
    {
        g.GetComponent<ObjectSync>().test = t;
    }

    void Carry(GameObject obj)
    {
        NetworkIdentity nid = obj.GetComponent<NetworkIdentity>();
        if (nid != null && nid.hasAuthority)
        {
            obj.transform.position = anchorPoint.position;
            obj.transform.rotation = anchorPoint.rotation;
        }
        else
        {
            Debug.LogError("Carrying an object that you dont have the authority of.");
            isCarryingObject = false;
            carriedObject = null;
        }
    }

    public void PickingUpAnimation()
    {
        isPickingUp = true;
    }

    void Pickup()
    {
        if (StaticInput.GetButtonDown("X"))
        {
            if (pickableObject != null)
            {
                if (pickableObject.GetComponent<Collectible>() != null)
                {
                    CmdDestroyCollectible(pickableObject);
                    GetComponent<PlayerScoreManager>().Cmd_AddPoints(pickableObject.GetComponent<Collectible>().GetCollectibleScoreObj());
                    pickableObject = null;
                    hintUI.Hide();
                }
                else
                {
                    isCarryingObject = true;
                    carriedObject = pickableObject;
                    Cmd_SetAuth(carriedObject, true);
                    _player.ChangeState(StateEnum.GRABBING);

                    if (!isServer)
                    {
                            NetworkIdentity ni = carriedObject.GetComponent<NetworkIdentity>();
                            ni.localPlayerAuthority = true;
                            Cmd_GetAutority(ni);
                    }

                    StartCoroutine(WaitForPickUp());

                    CmdDisableRigidBody(carriedObject);
                }
            }
        }
    }

    [Command]
    void CmdDestroyCollectible(GameObject gameObj)
    {
        if(gameObj != null)
        { 
            FadeMaterial fm = gameObj.GetComponent<FadeMaterial>();
            if (fm != null)
                fm.Rpc_Kill();
        }
    }

    
    [Command]
    public void Cmd_GetAutority(NetworkIdentity ni)
    {
        NetworkConnection temp = GetComponent<NetworkIdentity>().connectionToClient;
        if(ni != null)
        { 
            ni.localPlayerAuthority = true;

            ni.AssignClientAuthority(temp);
        }
    }


    [Command]
    public void Cmd_RemoveAutority(NetworkIdentity ni)
    {
        NetworkConnection temp = GetComponent<NetworkIdentity>().connectionToClient;

        ni.RemoveClientAuthority(temp);
    }

    public IEnumerator WaitForPickUp()
    {
        yield return new WaitForSeconds(FailSafePickup);
        PickingUpAnimation();
    }

    public void InsertKeyInDoor()
    {
        SetForceHideHintUi(false);
        isCarryingObject = false;
        carriedObject = null;
    }

    void Drop()
    {   
        if (StaticInput.GetButtonDown("X"))
        {
            droppedObject = carriedObject;
            CmdEnableRigidBody(droppedObject);
            
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

            Cmd_SetAuth(carriedObject, false);
            isCarryingObject = false;
            isPickingUp = false;
            carriedObject = null;
        }
    }

    [Command]
    void CmdDisableRigidBody(GameObject gameObj)
    {
        RpcDisableRigidBody(gameObj);
    }
    
    [Command]
    void CmdEnableRigidBody(GameObject gameObj)
    {
        RpcEnableRigidBody(gameObj);
    }
    
    [ClientRpc]
    void RpcDisableRigidBody(GameObject gameObj)
    {
        if (gameObj == null)
            return;

        Rigidbody pickableRigidBody = gameObj.GetComponent<Rigidbody>();
        if (pickableRigidBody != null)
        {
            oldObjectMass = pickableRigidBody.mass;
            rigidbodyConstraints = pickableRigidBody.constraints;
            Destroy(pickableRigidBody);
        }

        gameObj.GetComponents<Collider>().Where((c) => !c.isTrigger).First().enabled = false;
        gameObj.GetComponents<Collider>().Where((c) => c.isTrigger).First().enabled = false;
    }
    
    [ClientRpc]
    void RpcEnableRigidBody(GameObject gameObj)
    {
        if(gameObj != null)
        {
            gameObj.GetComponents<Collider>().Where((c) => !c.isTrigger).First().enabled = true;
            gameObj.GetComponents<Collider>().Where((c) => c.isTrigger).First().enabled = true;
            gameObj.AddComponent<Rigidbody>();
    
            Rigidbody carriedNewRigidBody = gameObj.GetComponent<Rigidbody>();
            carriedNewRigidBody.useGravity = true;
            carriedNewRigidBody.mass = oldObjectMass;
            carriedNewRigidBody.constraints = rigidbodyConstraints;

            gameObj = null;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (GetComponent<ObjectSync>().hasAuthority && collider.gameObject.CompareTag("PickableObject") && pickableObject == null)
        {
			hintUI.Display(Controls.X, "Pick up object");
			pickableObject = collider.gameObject;
            pickableObject.GetComponentInChildren<HighlightObject>().ToggleHighlight(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (GetComponent<ObjectSync>().hasAuthority && collider.gameObject.CompareTag("PickableObject") && collider.gameObject == pickableObject)
        {
            hintUI.Hide();
            HighlightObject hob = pickableObject.GetComponentInChildren<HighlightObject>();

            if(hob != null)
                hob.ToggleHighlight(false);

            pickableObject = null;
        }
    }
}
