using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum Controls
{
    A,
    B,
    X,
    Y,
    DownArrow,
    LeftArrow,
    RightArrow,
    UpArrow,
    LB,
    RB,
    LT,
    RT,
    LeftBump,
    RightBump,
    Select,
    Menu,
};

public class HintUI : MonoBehaviour {

    public GameObject HintPanel;

    public Image buttonImage;
    public Text actionTxt;

    public List<Sprite> sprites;

	public void Display(Controls button, string action)
    {        
        Sprite sprite = FindSprite(button);

        if (sprite) buttonImage.sprite = sprite;
        actionTxt.text = action;

        HintPanel.SetActive(true);
    }

    public void Hide()
    {
        HintPanel.SetActive(false);
        buttonImage.sprite = null;
        actionTxt.text = null;
    }

    string FindKeyRef(Controls button)
    {
        string name = "";

        try
        {
            switch (button)
            {
                case Controls.A:
                    name = "control_a";
                    break;
                case Controls.B:
                    name = "control_b";
                    break;
                case Controls.X:
                    name = "control_x";
                    break;
                case Controls.Y:
                    name = "control_y";
                    break;
                case Controls.DownArrow:
                    name = "control_downArrow";
                    break;
                case Controls.LeftArrow:
                    name = "control_leftArrow";
                    break;
                case Controls.RightArrow:
                    name = "control_rightArrow";
                    break;
                case Controls.UpArrow:
                    name = "control_upArrow";
                    break;
                case Controls.LB:
                    name = "control_lb";
                    break;
                case Controls.RB:
                    name = "control_rb";
                    break;
                case Controls.LT:
                    name = "control_lt";
                    break;
                case Controls.RT:
                    name = "control_rt";
                    break;
                case Controls.LeftBump:
                    name = "control_leftBump";
                    break;
                case Controls.RightBump:
                    name = "control_rightBump";
                    break;
                case Controls.Select:
                    name = "control_select";
                    break;
                case Controls.Menu:
                    name = "control_menu";
                    break;
                default:
                    throw new ArgumentException("Hint Panel: There's no references for that keycode");
            }
            
        }
        catch (ArgumentException e)
        {
            Debug.Log(e.Message);
        }       

        return name;
    }

    Sprite FindSprite(Controls button)
    {
        try
        {
            string name = FindKeyRef(button);
            if (sprites.Count == 0) throw new ArgumentException("Hint Panel: There's no sprite");

            foreach(Sprite img in sprites)
            {
                if(img.name == name)
                {
                    return img;
                }
            }
            throw new ArgumentException("Hint Panel: There's no sprite with that name");
        }
        catch (ArgumentException e)
        {
            Debug.Log(e.Message);
        }

        return null;
    }
}
