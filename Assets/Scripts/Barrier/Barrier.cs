using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public enum BarrierState
{
    Opened,
    Closed
}

public class Barrier : NetworkBehaviour {
    public Animator Anim;
    public BarrierTrigger BT;

    public List<GameObject> Keys;
    public bool Locked;

    private bool GirlIsInsideTrigger = false;
    private BarrierState CurrentState = BarrierState.Closed;

    void Start() {
        BT.GirlTriggerState += GirlTrigger;
    }

    void GirlTrigger(bool state)
    {
        GirlIsInsideTrigger = state;
    }

    [Server]
    private void Update()
    {
        if(GirlIsInsideTrigger && Keys.Count == 0 && Input.GetButtonDown("X"))
        {
            if (!Anim.GetBool("Opening") && !Anim.GetBool("Closing"))
            { 
                if (CurrentState == BarrierState.Closed)
                    Open();
                else
                    Close();
            }
        }
    }

    public void Open()
    {
        if(!Anim.GetBool("Opening") && !Anim.GetBool("Closing"))
        { 
            Anim.SetBool("Opening", true);
            CurrentState = BarrierState.Opened;
        }
    }

    public void Close()
    {
        if (!Anim.GetBool("Opening") && !Anim.GetBool("Closing"))
        {
            Anim.SetBool("Closing", true);
            CurrentState = BarrierState.Closed;
        }
    }

    public void AnimationEnded()
    {
        Anim.SetBool("Opening", false);
        Anim.SetBool("Closing", false);
    }
}
