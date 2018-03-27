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

    private bool isPickingUpObject = false;
    private Vector3 lerpInitialPosition;
    private Vector3 lerpFinalPosition;
    private Quaternion lerpInitialRotation;
    private Quaternion lerpFinalRotation;
    private float startTime;
    public float animationTime = 2.0f;

    private float oldObjectMass = 1.0f;
    private RigidbodyConstraints rigidbodyConstraints;

    private void Start()
    {
        hintUI = FindObjectOfType<HintUI>();
        _player = this.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update () {
		if(!isPickingUpObject && isCarryingObject)
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
        else if(isPickingUpObject && isCarryingObject)
        {
            PickingUpAnimation(carriedObject);
            if(Vector3.Distance(carriedObject.transform.position, lerpFinalPosition) < Vector3.kEpsilon)
            {
                isPickingUpObject = false;
            }
        }
        else
        {
            Pickup();
        }
	}

    void PickingUpAnimation(GameObject obj)
    {
        float timePass = (Time.time - startTime) / animationTime;

        obj.transform.position = Vector3.Lerp(lerpInitialPosition, lerpFinalPosition, timePass);
        obj.transform.rotation = Quaternion.Lerp(lerpInitialRotation, lerpFinalRotation, timePass);
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

                lerpInitialPosition = pickableObject.transform.position;
                lerpFinalPosition = anchorPoint.position;
                lerpInitialRotation = pickableObject.transform.rotation;
                lerpFinalRotation = anchorPoint.rotation;
                isPickingUpObject = true;
                startTime = Time.time;
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
            droppedObject = carriedObject;
            CmdEnableRigidBody(droppedObject);
            
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
