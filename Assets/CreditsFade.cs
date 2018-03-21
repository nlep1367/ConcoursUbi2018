using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FadeState
{
    None,
    FadeIn,
    Showing,
    FadeOut
}

public class CreditsFade : MonoBehaviour {

    public CreditsFade PreviousImage;

    public float ShowTime = 2;

    public float CurrentTime;

    public FadeState State = FadeState.None;

    private bool Showing;

    Action Ending;

    private const float FadeTime = 1;
    private SpriteRenderer render;

    void Start()
    {
        PreviousImage.Ending += BeginAnim;
        render = GetComponent<SpriteRenderer>();

        if (State == FadeState.FadeIn)
            Showing = true;

    }

    void BeginAnim()
    {
        State = FadeState.FadeIn;
        Showing = true;
    }

	// Update is called once per frame
	void Update () {
        float CurrentAlpha = 0;

        // If im showing then add time
        if(Showing)
        {
            CurrentTime += Time.deltaTime;
        }
        
        // We have ended the sequence
        if(CurrentTime > ShowTime)
        {
            CurrentTime = 0;
            CurrentAlpha = 0;

            Showing = false;
            State = FadeState.None;
        }

        // FadeIn is finished

        switch (State)
        { 
            case FadeState.None:
                break;

            case FadeState.FadeIn:
                CurrentAlpha = CurrentTime / FadeTime;

                if (CurrentAlpha > 1.0f)
                {
                    State = FadeState.Showing;
                    CurrentAlpha = 1;
                }
                break;

            case FadeState.Showing:
                CurrentAlpha = 1;

                if (CurrentTime > ShowTime - FadeTime)
                {
                    State = FadeState.FadeOut;
                    Ending.Invoke();
                }
                break;

            case FadeState.FadeOut:
                CurrentAlpha = (ShowTime - CurrentTime) / FadeTime;
                
                break;
        }
        
        CurrentAlpha = Mathf.Clamp(CurrentAlpha, 0, 1);

        render.color = new Color(255, 255, 255, CurrentAlpha);
    }
}
