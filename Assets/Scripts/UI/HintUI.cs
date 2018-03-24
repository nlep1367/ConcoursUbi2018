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
        string name = "";
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
                name = "xbox_Y";
                break;
        }
        Sprite sprite = FindSprite(name);

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

    Sprite FindSprite(string name)
    {
        try
        {
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
