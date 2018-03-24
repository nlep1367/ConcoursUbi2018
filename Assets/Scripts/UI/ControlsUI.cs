using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsUI : MonoBehaviour {

    public GameObject EchoControls;
    public GameObject IrisControls;

    bool isActive = false;
    bool isIris = false;

    public GameObject Iris;

    private void Update()
    {
        if(Iris == null)
        {
            GameObject temp = GameObject.FindGameObjectWithTag("Fille");
            if(temp != null)
            {
                Iris = temp;
                isIris = Iris.GetComponent<ObjectSync>().hasAuthority;
            }
        }
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
