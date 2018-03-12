using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class DogBarkEcho : MonoBehaviour
{
    private float BarkRadius, Contour, ContourStep, Timer = 0f;
    public Transform DogPosRef;
    public Color BarkColor;
    public float MaxRad = 8f;
    public float Duration = 3f;
    public float StillDuration = 2f;
    private float Increment = 0f;
    public float ContourBegin = 0.2f;
    // Use this for initialization
    void Start()
    {
        Increment = MaxRad / Duration;
        Contour = ContourBegin;
        Shader.SetGlobalColor("_ColorBark", BarkColor);
        Shader.SetGlobalFloat("_ContourWidth", Contour);
        ContourStep = Contour / StillDuration;

    }
    void OnEnable()
    {
        Shader.SetGlobalVector("_DogPosition", DogPosRef.position);
        Shader.SetGlobalColor("_ColorBark", BarkColor);
    }
    // Update is called once per frame
    void Update()
    {
        if (enabled)
        {
            if (BarkRadius < MaxRad)
            {
                BarkRadius += Increment * Time.deltaTime;
                Shader.SetGlobalFloat("_BarkRadius", BarkRadius);
            }
            else
            {
                Timer += Time.deltaTime;
                Contour -= ContourStep * Time.deltaTime;
                Shader.SetGlobalFloat("_ContourWidth", Contour);
                if (Timer >= StillDuration)
                {
                    Contour = ContourBegin;
                    enabled = false;
                    Shader.SetGlobalFloat("_BarkRadius", 0);
                    Shader.SetGlobalFloat("_ContourWidth", Contour);
                    BarkRadius = 0;
                    Timer = 0;
                }
            }
        }
    }
}
