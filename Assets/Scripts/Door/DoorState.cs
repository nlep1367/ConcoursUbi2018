using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class DoorState : NetworkBehaviour {
    Animator Anim;

    public bool CloseTest = false;
    public bool OpenTest = false;

    private bool Opened = false;
    private bool Idle = false;

    private ObjectSync Os;

    private void Update()
    {
        if(OpenTest)
        {
            OpenTest = false;
            OpenDoor();
        }
        
        if(CloseTest)
        {
            CloseTest = false;
            CloseDoor();
        }
    }

    void Start()
    {
        Anim = GetComponent<Animator>();

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
}
