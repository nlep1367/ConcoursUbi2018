using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class DoorState : NetworkBehaviour {

    public List<GameObject> RelatedKey;
    public List<GameObject> Locks;

    public Animator Anim;
    public DoorTrigger dt;
    public GameObject blockingObj;

    private bool isDoorBlocked = false;
    private bool Opened = false;
    private bool Idle = false;
    
    private bool IsGirlInRange = false;

    private ObjectSync Os;

    private void Update()
    {
        if(isDoorBlocked)
        {
            CheckIfDoorStillBlocked();
        }

        if (IsGirlInRange && Input.GetButtonDown("A") && Locks.Count == 0 && !isDoorBlocked)
        {
            OpenDoor();
            CloseDoor();
        }

        if (!isServer)
            return;

        foreach (GameObject go in RelatedKey)
        {
            // Bonne clef
            if (Vector3.Distance(go.transform.position, dt.transform.position) < 3)
            {
                if(IsGirlInRange && Input.GetButtonDown("A") && Locks.Count > 0)
                {
                    GameObject.FindGameObjectWithTag("Fille").GetComponent<PickupObject>().InsertKeyInDoor();
                    dt.hUI.Display(Controls.A, "Open Door");

                    GameObject lck = Locks[Locks.Count - 1];

                    lck.GetComponent<FadeMaterial>().Rpc_Kill();
                    go.GetComponent<FadeMaterial>().Rpc_Kill();
                
                    Locks.Remove(lck);
                    RelatedKey.Remove(go);
                    break;
                }
            }
        }

    }

    public void CheckIfDoorStillBlocked()
    {
        isDoorBlocked = Vector3.Distance(gameObject.transform.position, blockingObj.transform.position) < 3;
    }

    //Always call this funtion if you are sure that the girl is in range of the door
    public bool IsKeyRelatedToLocks()
    {
        GameObject carriedObj = GameObject.FindGameObjectWithTag("Fille").GetComponent<PickupObject>().GetCarriedObject();

        foreach (GameObject go in RelatedKey)
        {
            if (carriedObj == go)
            {
                return true;
            }
        }
        return false;
    }

    void Start()
    {
        dt.GirlTriggerState += GirlInRange;

        if (!isServer)
        { 
            Destroy(Anim);
            Destroy(this);
        }

        if(blockingObj != null)
        {
            isDoorBlocked = true;
        }
    }
    
    public void StopAnimating()
    {
         Idle = true;
    }

    public void OpenDoor()
    {
        if (!Idle || Opened)
            return;

        Anim.Play("Door_Open");
        Idle = false;
        Opened = true;
    }

    public void CloseDoor()
    {
        if (!Idle || !Opened)
            return;

        Anim.Play("Door_Close");
        Idle = false;
        Opened = false;
    }

    void GirlInRange(bool state)
    {
        IsGirlInRange = state;
    }  

    [Server]
    void OnCollisionEnter(Collision collision)
    {
        if (!isServer)
            return; 

    }
}
