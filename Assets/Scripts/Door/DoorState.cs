using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class DoorState : NetworkBehaviour {
    Animator Anim;

    public Collider RelatedKey;

    private bool Opened = false;
    private bool Idle = false;

    private bool Locked = false;
    private bool MatchingKeyInserted = false;
    private bool WrongKey = false;

    private bool IsGirlInRange = false;

    private ObjectSync Os;

    private void Update()
    {
        if (Opened)
            Close();
        else
            Open();
    }

    void Start()
    {
        Anim = GetComponent<Animator>();

        if (RelatedKey != null)
            Locked = true;

        if (!isServer)
        { 
            Destroy(Anim);
            Destroy(this);
        }
    }

    private void OnGUI()
    {
        if(WrongKey)
        {
            GUI.Box(new Rect(0, 60, 200, 25), "Wrong key");
            WrongKey = false;
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

    void Open()
    {
        if (IsGirlInRange && Input.GetButtonDown("Y"))
        {
            if(Locked && MatchingKeyInserted)
            {
                Locked = false;
                OpenDoor();
            }
            else
            {
                OpenDoor();
            }
        }
    }

    void Close()
    {
        if (IsGirlInRange && Input.GetButtonDown("Y"))
        {
            CloseDoor();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider == RelatedKey)
        {
            GameObject.FindGameObjectWithTag("Fille").GetComponent<PickupObject>().InsertKeyInDoor();
            Destroy(collision.gameObject);
            MatchingKeyInserted = true;
        }

        if (collision.gameObject.CompareTag("PickableObject"))
        {
            WrongKey = true;
        }

        if (collision.gameObject.CompareTag("Fille"))
        {
            IsGirlInRange = true;
        }

    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fille"))
        {
            IsGirlInRange = false;
        }
    }
}
