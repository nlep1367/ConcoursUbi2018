using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsFade : MonoBehaviour {

    public float ShowTime = 2;

    public float CurrentTime;

    private bool FadeIn;
    private bool FadeOut;

    private bool Showing;

    Action<int> End;

    private const float FadeTime = 1;
	
	// Update is called once per frame
	void Update () {

        if(Showing)
        {
            CurrentTime += Time.deltaTime;
        }

        float CurrentAlpha = 0; ;

        if (FadeIn)
        { 
            CurrentAlpha = CurrentTime / FadeTime;
        }
        else if(FadeOut)
        {
            CurrentAlpha = (ShowTime - CurrentTime) / FadeTime; 
        }

        if(CurrentTime > ShowTime)
        {
            CurrentTime = 0;
            CurrentAlpha = 0;

            Showing = false;
            FadeOut = false;
        }

        if(FadeIn && CurrentAlpha > 1.0f)
        {
            FadeIn = false;
            CurrentAlpha = 1;
        }

        CurrentAlpha = Mathf.Clamp(CurrentAlpha, 0, 1);
    }
}
