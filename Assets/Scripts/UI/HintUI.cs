using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HintUI : MonoBehaviour {

    public GameObject HintPanel;

    public Image buttonImage;
    public Text actionTxt;

    public List<Sprite> sprites;

	public void Display(KeyCode button, string action)
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

    string FindKeyRef(KeyCode button)
    {
        string name = "";

        try
        {
            switch (button)
            {
                case KeyCode.Y:
                    name = "xbox_Y";
                    break;
                case KeyCode.B:
                    name = "xbox_B";
                    break;
                case KeyCode.A:
                    name = "xbox_A";
                    break;
                case KeyCode.X:
                    name = "xbox_X";
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

    Sprite FindSprite(KeyCode button)
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
