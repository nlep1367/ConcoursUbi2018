using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour {
    bool isCarryingObject = false;
    GameObject pickableObject;
    GameObject carriedObject;
    public Transform anchorPoint;
    private HintUI hintUI;
    private Player _player;

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
                hintUI.Display(KeyCode.Y, "Throw in the garbage");
            }
            else
            {
                hintUI.Display(KeyCode.Y, "Drop the object");
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
            if(pickableObject != null)
            {
                isCarryingObject = true;
                carriedObject = pickableObject;
                _player.ChangeState(StateEnum.GRABBING);
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
        if (Input.GetButtonDown("Y"))
        {
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
