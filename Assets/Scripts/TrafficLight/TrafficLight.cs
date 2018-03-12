using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrafficLightState
{
    Red,
    Green,
    Yellow,
    Count,
}

public class TrafficLight : MonoBehaviour {

    public Renderer RedLight;
    public Material RedMaterial;

    public Renderer YellowLight;
    public Material YellowMaterial;

    public Renderer GreenLight;
    public Material GreenMaterial;

    public Material DefaultMaterial;

    public TrafficLightState lightState;

    // Use this for initialization
    void Start () {
        GreenLight.material = DefaultMaterial;
        YellowLight.material = DefaultMaterial;
        RedLight.material = DefaultMaterial;
    }

    public void SetLightMaterial()
    {
        switch (lightState)
        {
            case TrafficLightState.Green:
                GreenLight.material = GreenMaterial;
                RedLight.material = DefaultMaterial;
                break;

            case TrafficLightState.Yellow:
                YellowLight.material = YellowMaterial;
                GreenLight.material = DefaultMaterial;
                break;

            case TrafficLightState.Red:
                RedLight.material = RedMaterial;
                YellowLight.material = DefaultMaterial;
                break;
        }
    }
}
