using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Adaptation {
    const float MaxBarkRadius = 10f;
    const float MinBarkRadius = 4f;
    const float MaxBarkRange = 4f;
    const float MinBarkRange = 1f;
    const float MaxTerrorMultiplier = 1f;
    const float MinTerrorMultiplier = 0.2f;


    private static float _maxBarkRadius = 5f;
    public static float MaximumBarkRadius
    {
        get { return _maxBarkRadius; }
        set { _maxBarkRadius = Mathf.Clamp(value, MinBarkRadius, MaxBarkRadius); }
    }

    private static float _maxBarkRange = 5f;
    public static float BarkRange
    {
        get { return _maxBarkRange; }
        set { _maxBarkRange = Mathf.Clamp(value, MinBarkRange, MaxBarkRange); }
    }

    private static float _terrorMultiplier = 5f;
    public static float TerrorMultiplier
    {
        get { return _terrorMultiplier; }
        set { _terrorMultiplier = Mathf.Clamp(value, MinTerrorMultiplier, MaxTerrorMultiplier); }
    }
}
