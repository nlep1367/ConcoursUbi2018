using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour {
    bool isCarryingObject = false;
    bool isPickableObjectInRange = false;
    GameObject pickableObject;
    GameObject carriedObject;
    public Transform anchorPoint;

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
            ThrownableObject thrownable = carriedObject.GetComponentInParent<ThrownableObject>();
            if (thrownable != null)
            {
                thrownable.ThrowAway();
            }
            isCarryingObject = false;
            carriedObject = null;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("PickableObject"))
        {
            isPickableObjectInRange = true;
            pickableObject = collision.gameObject;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("PickableObject"))
        {
            isPickableObjectInRange = false;
            pickableObject = null;
        }
    }
}
