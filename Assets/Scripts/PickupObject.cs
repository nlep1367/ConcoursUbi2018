using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class PickupObject : NetworkBehaviour {
    bool isCarryingObject = false;
    GameObject pickableObject;
    GameObject carriedObject;
    GameObject dropedObject;
    public Transform anchorPoint;
    private HintUI hintUI;
    private Player _player;

    private float oldObjectMass = 1.0f;
    private RigidbodyConstraints rigidbodyConstraints;

    private void Start()
    {
        hintUI = FindObjectOfType<HintUI>();
        _player = this.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update () {
		if(isCarryingObject)
        {
            ThrownableObject thrownable = carriedObject.GetComponentInParent<ThrownableObject>();
            if (thrownable != null && thrownable.IsInThrownZone)
            {
                hintUI.Display(KeyCode.A, "Throw in the garbage");
            }
            else
            {
                hintUI.Display(KeyCode.A, "Drop the object");
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(pickableObject != null)
            {
                isCarryingObject = true;
                carriedObject = pickableObject;
                _player.ChangeState(StateEnum.GRABBING);

                CmdDisableRigidBody(carriedObject);
            }
        }
    }

    public void InsertKeyInDoor()
    {
        isCarryingObject = false;
        carriedObject = null;
    }

    void Drop()
    {   
        if (Input.GetKeyDown(KeyCode.E))
        {
            dropedObject = carriedObject;
            CmdEnableRigidBody(dropedObject);
            
            ThrownableObject thrownable = carriedObject.GetComponentInParent<ThrownableObject>();
            if (thrownable != null && thrownable.IsInThrownZone)
            {
                thrownable.ThrowAway();
                hintUI.Hide();
            }
            isCarryingObject = false;
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
    void RpcDisableRigidBody(GameObject gameObject)
    {
        Rigidbody pickableRigidBody = gameObject.GetComponent<Rigidbody>();
        if (pickableRigidBody != null)
        {
            oldObjectMass = pickableRigidBody.mass;
            rigidbodyConstraints = pickableRigidBody.constraints;
            Destroy(pickableRigidBody);
        }

        pickableObject.GetComponents<Collider>().Where((c) => !c.isTrigger).First().enabled = false;
    }
    
    [ClientRpc]
    void RpcEnableRigidBody(GameObject gameObj)
    {
        if(gameObj != null)
        {
            gameObj.GetComponents<Collider>().Where((c) => !c.isTrigger).First().enabled = true;
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
        if (GetComponent<ObjectSync>().hasAuthority && collider.gameObject.CompareTag("PickableObject"))
        {
            hintUI.Display(KeyCode.A, "Pick up " + collider.gameObject.name);
            pickableObject = collider.gameObject;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (GetComponent<ObjectSync>().hasAuthority && collider.gameObject.CompareTag("PickableObject"))
        {
            hintUI.Hide();
            pickableObject = null;
        }
    }
}
