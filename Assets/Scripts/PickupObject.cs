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
                carriedObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

    void Drop()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isCarryingObject = false;
            carriedObject.GetComponent<Rigidbody>().isKinematic = false;
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
