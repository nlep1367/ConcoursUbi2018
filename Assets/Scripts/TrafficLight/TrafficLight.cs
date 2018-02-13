using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrafficLightState
{
    Red,
    Green,
    Yellow
}

public class TrafficLight : MonoBehaviour {

    public Renderer RedLight;
    public Material RedMaterial;
    public float redLightDuration = 5f;

    public Renderer YellowLight;
    public Material YellowMaterial;
    public float yellowLightDuration = 1f;

    public Renderer GreenLight;
    public Material GreenMaterial;
    public float greenLightDuration = 0.5f;

    public Material DefaultMaterial;

    public bool pedestrianTrigger = false;
    private bool isChanging = false;

    public TrafficLightState lightState = TrafficLightState.Green;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (pedestrianTrigger && !isChanging)
        {
            isChanging = true;
            PedestrianButton();
        }
    }

    public void PedestrianButton()
    {
        StartCoroutine(ChangeLight());
    }

    IEnumerator ChangeLight()
    {
        yield return new WaitForSeconds(greenLightDuration);
        lightState = TrafficLightState.Yellow;
        GreenLight.material = DefaultMaterial;
        YellowLight.material = YellowMaterial;

        yield return new WaitForSeconds(yellowLightDuration);
        lightState = TrafficLightState.Red;
        YellowLight.material = DefaultMaterial;
        RedLight.material = RedMaterial;

        yield return new WaitForSeconds(redLightDuration);
        lightState = TrafficLightState.Green;
        RedLight.material = DefaultMaterial;
        GreenLight.material = GreenMaterial;

        pedestrianTrigger = isChanging = false;
    }
}
