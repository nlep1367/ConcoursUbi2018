using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class DogBark : NetworkBehaviour {
    public float BarkRange;
    private DogBarkEcho Echo;
    
	// Update is called once per frame
	void Update () {
        if (!hasAuthority)
            return;

		if(Input.GetButton("Fire1"))
        {
            Cmd_StartBark(Color.green);
           // Bark((IBarkListener l) => l.ReactToAngryBark());
        }

        if (Input.GetButton("Fire2"))
        {
            Cmd_StartBark(Color.red);
            //Bark((IBarkListener l) =>  l.ReactToSoftBark());
        }

    }


    [Command]
    public void Cmd_StartBark(Color color)
    {
        if (!Echo)
            Echo = GameObject.FindGameObjectWithTag("Fille").GetComponent<DogBarkEcho>();

        Echo.StartBark(color);
    }

        void Bark(Action<IBarkListener> barkReaction)
    {
      /*  Collider[] colliders = Physics.OverlapSphere(this.transform.position, BarkRange);

        foreach(Collider c in colliders)
        {
            IBarkListener listener = c.gameObject.GetComponent<IBarkListener>();
            if(listener != null)
            {
                listener.ReactToBark();
                barkReaction(listener);
            }
        }
        */
    }
}
