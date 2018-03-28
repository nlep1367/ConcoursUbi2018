using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Emotion
{
    Positive,
    Negative
}

public class LerpColor : MonoBehaviour {

    public Color[] Positive;
    public Color[] Negative;

    private Color StartColor;
    private Color EndColor;

    public float duration = 2f;
    public Camera Cam;

    private Emotion CurrentEmotion;
    private int CurrentColor = 0;

    private float CurrentTime = 0.0f;
    
    public void Start()
    {
        if (Positive.Length < 2)
            Debug.LogError("Not Enough positive emotion color selected.");

        StartColor = Positive[0];
        EndColor = Positive[1];
    }

    public void SetEmotions(Emotion NewEmotion)
    {
        if (NewEmotion == CurrentEmotion)
            return;

        CurrentEmotion = NewEmotion;

        StartColor = Cam.backgroundColor;

        if (CurrentEmotion == Emotion.Positive)
        {
            EndColor = Positive[0];
            CurrentColor = Positive.Length - 1;
        }
        else
        {
            EndColor = Negative[0];
            CurrentColor = Negative.Length - 1;
        }

        CurrentTime = 0;

    }

    void OnEnable ()
    {
        if (Cam)
        {
            Cam.clearFlags = CameraClearFlags.SolidColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!enabled || !Cam)
            return;
        
        CurrentTime += Time.deltaTime;

        if (CurrentTime >= duration)
        {
            CurrentTime = 0;

            if (Emotion.Negative == CurrentEmotion)
            {
                CurrentColor = (CurrentColor + 1) % Negative.Length;
                StartColor = Negative[CurrentColor];
                EndColor = Negative[(CurrentColor + 1) % Negative.Length];
            }
            else
            {
                CurrentColor = (CurrentColor + 1) % Positive.Length;
                StartColor = Positive[CurrentColor];
                EndColor = Positive[(CurrentColor + 1) % Positive.Length];
            }
        }

        Cam.backgroundColor = Color.Lerp(StartColor, EndColor, CurrentTime / duration); 
    }

    public Emotion GetCurrentEmotion()
    {
        return CurrentEmotion;
    }
}
