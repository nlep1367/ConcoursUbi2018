using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fear : MonoBehaviour {

    public enum FearState {
        Calm = 15,
        Anxious = 35,
        Stress = 60,
        Panic = 85,
        NearDeath = 100
    };

    public FearState fearState = FearState.Calm;
    public float fearValue = 0.0f;
   
    private void UpdateFearState()
    {
        foreach (FearState fs in Enum.GetValues(typeof(FearState)))
        {
            if (fearValue < (int)fs)
            {
                fearState = fs;
                break;
            }
        }
    }

    public void IncreaseFear(float value)
    {
        if (fearValue <= 100)
        {
            fearValue = Math.Min((fearValue*Adaptation.TerrorMultiplier) + value, 100);
            UpdateFearState();
        }
    }

    public void DecreaseFear(float value)
    {
        if (fearValue >= 0)
        {
            fearValue = Math.Max(fearValue - value, 0);
            UpdateFearState();
        }
    }
}
