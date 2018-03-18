using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DogBarkEcho : MonoBehaviour
{
    private float BarkRadius, Contour, ContourStep, Timer = 0f;
    public Transform DogPosRef;
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
       // Shader.SetGlobalColor("_ColorBark", Color.Black);
        Shader.SetGlobalFloat("_ContourWidth", Contour);
        ContourStep = Contour / StillDuration;

    }
    
    public void StartBark(Color color)
    {
        if (!DogPosRef)
        {
            DogPosRef = GameObject.FindGameObjectWithTag("Doggo").transform;
        }

        if (enabled)
            StopBark();
        
        Shader.SetGlobalVector("_DogPosition", DogPosRef.position);
        Shader.SetGlobalColor("_ColorBark", color);

        this.enabled = true;
    }

    void StopBark()
    {
        Contour = ContourBegin;
        Shader.SetGlobalFloat("_BarkRadius", 0.0f);
        Shader.SetGlobalFloat("_ContourWidth", 0);
        BarkRadius = 0;
        Timer = 0;
    }

    // Update is called once per frame
    void Update()
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
