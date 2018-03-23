using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PickupObject : NetworkBehaviour {
    bool isCarryingObject = false;
    bool isPickableObjectInRange = false;
    GameObject pickableObject;
    GameObject carriedObject;
    GameObject dropedObject;
    public Transform anchorPoint;

    private float oldObjectMass = 1.0f;

	// Update is called once per frame
	void Update () {
		if(isCarryingObject)
        {
            Carry(carriedObject);
            Drop();
        }
        else
        {
            Pickup();
        }
	}

    private void OnGUI()
    {
        if(isPickableObjectInRange)
        {
            GUI.Box(new Rect(0, 60, 200, 25), "Press E to pickup");
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
            if(isPickableObjectInRange && pickableObject != null)
            {
                isCarryingObject = true;
                carriedObject = pickableObject;

                CmdDisableRigidBody();
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

            CmdEnableRigidBody();

            ThrownableObject thrownable = carriedObject.GetComponentInParent<ThrownableObject>();
            if (thrownable != null)
            {
                thrownable.ThrowAway();
            }
            isCarryingObject = false;
            carriedObject = null;
        }
    }

    [Command]
    void CmdDisableRigidBody()
    {
        RpcDisableRigidBody();
    }

    [Command]
    void CmdEnableRigidBody()
    {
        RpcEnableRigidBody();
    }

    [ClientRpc]
    void RpcDisableRigidBody()
    {
        Rigidbody pickableRigidBody = pickableObject.GetComponent<Rigidbody>();
        if (pickableRigidBody != null)
        {
            oldObjectMass = pickableRigidBody.mass;
            Destroy(pickableRigidBody);
        }
    }

    [ClientRpc]
    void RpcEnableRigidBody()
    {
        if(dropedObject != null)
        {
            dropedObject.AddComponent<Rigidbody>();

            Rigidbody carriedNewRigidBody = carriedObject.GetComponent<Rigidbody>();
            carriedNewRigidBody.useGravity = true;
            carriedNewRigidBody.mass = oldObjectMass;

            dropedObject = null;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("PickableObject"))
        {
            isPickableObjectInRange = true;
            pickableObject = collider.gameObject;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("PickableObject"))
        {
            isPickableObjectInRange = false;
            pickableObject = null;
        }
    }
}
