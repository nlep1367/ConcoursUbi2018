using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkListenerPrint : MonoBehaviour, IBarkListener
{
    public void ReactToAngryBark()
    {
        Debug.Log("One canine army.");
    }

    public void ReactToBark()
    {
        Debug.Log("I heard the bark. I wait for the bite.");
    }

    public void ReactToSoftBark()
    {
        Debug.Log("*Soothing bark*");
    }
}
