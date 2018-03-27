using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterestZone : MonoBehaviour {
    public GameObject PathStart;
    public GameObject PathEnd;

    private void Start()
    {
        if (!PathEnd || !PathStart)
            Debug.LogError("Need to set things up dude - InterestZone");
    }
}
