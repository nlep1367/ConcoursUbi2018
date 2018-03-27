using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class DoorState : NetworkBehaviour {

    public List<GameObject> RelatedKey;
    public List<GameObject> Locks;

    public Animator Anim;
    public DoorTrigger dt;

    private bool Opened = false;
    private bool Idle = false;
    
    private bool IsGirlInRange = false;

    private ObjectSync Os;

    private void Update()
    {
        if (IsGirlInRange && Input.GetButtonDown("Y") && Locks.Count == 0)
        {
            OpenDoor();
            CloseDoor();
        }

        if (!isServer)
            return;

        foreach (GameObject go in RelatedKey)
        {
            // Bonne clef
            if (Vector3.Distance(go.transform.position, dt.transform.position) < 1)
            {
                GameObject.FindGameObjectWithTag("Fille").GetComponent<PickupObject>().InsertKeyInDoor();

                GameObject lck = Locks[Locks.Count - 1];

                lck.GetComponent<FadeMaterial>().Rpc_Kill();
                go.GetComponent<FadeMaterial>().Rpc_Kill();
                
                Locks.Remove(lck);
                RelatedKey.Remove(go);
                break;
            }
        }

    }

    void Start()
    {
        dt.GirlTriggerState += GirlInRange;

        if (!isServer)
        { 
            Destroy(Anim);
            Destroy(this);
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
