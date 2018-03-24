using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResolutionDropdown : MonoBehaviour {

    Dropdown dropdown;
    Resolution[] resolutions;
    int currentResolutionIndex = 0;
    int navigationIndex = 0;
    public Vector2 m_NextScrollPosition = Vector2.up;
    ScrollRect scrollRect;
    Selectable oldSelectedElement = null;

    private List<Selectable> m_Selectables = new List<Selectable>();

    // Use this for initialization
    void Start () {
        string currentRes = Screen.width + " x " + Screen.height;
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

        dropdown = GetComponent<Dropdown>();
        resolutions = Screen.resolutions;

        for(int i=0; i< resolutions.Length; ++i)
        {
            Dropdown.OptionData option = new Dropdown.OptionData
            {
                text = resolutions[i].width + " x " + resolutions[i].height
            };
            options.Add(option);

            if (option.text == currentRes) currentResolutionIndex = i;
        }

        dropdown.ClearOptions();
        dropdown.AddOptions(options);
        dropdown.value = currentResolutionIndex;
    }
	
	public void SetNewResolution(int index)
    {
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);
        currentResolutionIndex = index;
        #if !EMM_ES2
            PlayerPrefs.SetInt("currentScreenResolutionCount", index);
        #else
            ES2.Save(currentScreenResolutionCount, "currentScreenResolutionCount");
        #endif
    }

    void FixedUpdate()
    {
        // Scroll via input.
        if (!scrollRect)
        {
            scrollRect = GetComponentInChildren<ScrollRect>();
            if(scrollRect) scrollRect.content.GetComponentsInChildren(m_Selectables);
            return;
        }
        ScrollToSelected();
        scrollRect.normalizedPosition = m_NextScrollPosition;
    }

    void ScrollToSelected()
    {        
        Selectable selectedElement = EventSystem.current.currentSelectedGameObject ? EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>() : null;
        
        if (selectedElement != oldSelectedElement)
        {
            int selectedIndex = m_Selectables.IndexOf(selectedElement);
            m_NextScrollPosition = new Vector2(0, 1 - (selectedIndex / ((float)resolutions.Length- 1)));
            oldSelectedElement = selectedElement;
        }
    }
}
