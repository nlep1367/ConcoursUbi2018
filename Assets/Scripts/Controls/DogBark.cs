using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBark : MonoBehaviour {
    public float BarkRange;

	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Fire1"))
        {
            Bark((IBarkListener l) => l.ReactToAngryBark());
        }

        if (Input.GetButton("Fire2"))
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
