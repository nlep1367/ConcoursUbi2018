using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsUI : MonoBehaviour {

    public GameObject EchoControls;
    public GameObject IrisControls;

    bool isActive = false;
    bool isIris;

    private void Start()
    {
        isIris = GameObject.FindGameObjectWithTag("Fille").GetComponent<ObjectSync>().hasAuthority;
    }

    public void ToggleControls()
    {
        isActive = !isActive;

        if(isIris)  // If Iris -> Display Iris Controls
            IrisControls.SetActive(isActive);
        else        // else -> Display Echo
            EchoControls.SetActive(isActive);        
    }
}
