using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIControls : MonoBehaviour {

    public Button onEnableSelect;
    
    protected void OnEnable()
    {
        onEnableSelect.Select();
    }
}
