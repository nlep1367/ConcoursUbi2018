using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayPanelGUI : GUIControls {

    public GameObject newGameInput;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (newGameInput.GetComponent<InputField>().isFocused)
        {
            if (StaticInput.GetButtonDown("Submit") || StaticInput.GetButtonDown("Cancel"))
            {
                onEnableSelect.Select();
            }
        }
	}

    
}
