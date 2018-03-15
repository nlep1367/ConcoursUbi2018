using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockerAI : MonoBehaviour {

    public Transform StartPosition;
    public Transform EndPosition;

    public AIVision Vision;
    public IdleBehaviour Idle;
    
    // Update is called once per frame
	void Update ()
    {
        Vector3 Target;
        if (Vision.SeeSomething(out Target))
        {

        }

	}
}
