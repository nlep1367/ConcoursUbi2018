using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[ExecuteInEditMode]
public class DogBarkEcho : NetworkBehaviour
{
    public float BarkRadius;
    private float Contour, ContourStep, Timer = 0f;
    public Transform DogPosRef;
    public float Duration = 3f;
    public float StillDuration = 2f;
    private float Increment = 0f;
    public float ContourBegin = 0.2f;

    // Use this for initialization
    void Start()
    {
        Increment = Adaptation.MaximumBarkRadius / Duration;
        Contour = ContourBegin;
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
        Shader.SetGlobalFloat("_ContourWidth", Contour);
        BarkRadius = 0;
        Timer = 0;

        RpcStopDogVisual();
    }

    // Update is called once per frame
    void Update()
    {
        if (BarkRadius < Adaptation.MaximumBarkRadius)
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
                StopBark();
                enabled = false;
            }
        }
    }

    [ClientRpc]
    void RpcStopDogVisual()
    {
        GameObject PlaneBark = GameObject.FindGameObjectWithTag("DogEcho");
        if (PlaneBark)
        {
            PlaneBark.GetComponent<Renderer>().enabled = false;
        }
    }
}
