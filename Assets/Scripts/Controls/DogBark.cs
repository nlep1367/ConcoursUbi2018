using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBark : MonoBehaviour {
    public float BarkRange;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.E))
        {
            Bark((IBarkListener l) => l.ReactToAngryBark());
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Bark((IBarkListener l) =>  l.ReactToSoftBark());
        }

    }

    void Bark(Action<IBarkListener> barkReaction)
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, BarkRange);

        foreach(Collider c in colliders)
        {
            IBarkListener listener = c.gameObject.GetComponent<IBarkListener>();
            if(listener != null)
            {
                listener.ReactToBark();
                barkReaction(listener);
            }
        }

    }
}
