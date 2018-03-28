using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsUI : MonoBehaviour {

    public GameObject EchoControls;
    public GameObject IrisControls;

    bool wasSet = false;

    public void DisplayControls(bool isIris)
    {
        if (!wasSet)
        {
            wasSet = true;

            if (isIris)  // If Iris -> Display Iris Controls
                IrisControls.SetActive(true);
            else        // else -> Display Echo
                EchoControls.SetActive(true);
        }
    }
}
