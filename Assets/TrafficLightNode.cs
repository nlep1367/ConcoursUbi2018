using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightNode : MonoBehaviour {

    public List<TrafficLight> horizontal;
    public List<TrafficLight> vertical;

    // red must equal green + yellow
    public float redLightDuration = 3f;
    public float yellowLightDuration = 1f;
    public float greenLightDuration = 2f;

    public void Start()
    {
        foreach (TrafficLight tf in horizontal)
        {
            tf.lightState = TrafficLightState.Green;
            tf.SetLightMaterial();
        }

        foreach (TrafficLight tf in vertical)
        {
            tf.lightState = TrafficLightState.Red;
            tf.SetLightMaterial();
        }
    }

    private bool isReadyV = true;
    private bool isReadyH = true;

    public void Update()
    {
        if (isReadyV)
        {
            isReadyV = false;
            ShowNextLightVertical();
        }

        if (isReadyH)
        {
            isReadyH = false;
            ShowNextLightHorizontal();
        }
    }

    void ShowNextLightHorizontal()
    {
        if (horizontal.Count == 0)
            return;

        switch (horizontal[0].lightState)
        {
            case TrafficLightState.Green:
                StartCoroutine(Green(true));
                break;

            case TrafficLightState.Yellow:
                StartCoroutine(Yellow(true));
                break;

            case TrafficLightState.Red:
                StartCoroutine(Red(true));
                break;
        }
    }

    void ShowNextLightVertical()
    {
        if (vertical.Count == 0)
            return;

        switch (vertical[0].lightState)
        {
            case TrafficLightState.Green:
                StartCoroutine(Green(false));
                break;

            case TrafficLightState.Yellow:
                StartCoroutine(Yellow(false));
                break;

            case TrafficLightState.Red:
                StartCoroutine(Red(false));
                break;
        }
    }

    public IEnumerator Red(bool isHorizontal)
    {
        yield return new WaitForSeconds(redLightDuration);
        AdjustLight(isHorizontal);
    }

    public IEnumerator Yellow(bool isHorizontal)
    {
        yield return new WaitForSeconds(yellowLightDuration);
        AdjustLight(isHorizontal);
    }

    public IEnumerator Green(bool isHorizontal)
    {
        yield return new WaitForSeconds(greenLightDuration);
        AdjustLight(isHorizontal);
    }

    public void AdjustLight(bool isHorizontal)
    {
        if (isHorizontal)
        {
            foreach(TrafficLight tf in horizontal)
            {
                int nextVal = (int)tf.lightState + 1;
                tf.lightState = (TrafficLightState)Enum.ToObject(typeof(TrafficLightState), nextVal % (int)TrafficLightState.Count);
                tf.SetLightMaterial();
            }
            isReadyH = true;
        }
        else
        {
            foreach (TrafficLight tf in vertical)
            {
                int nextVal = (int)tf.lightState + 1;
                tf.lightState = (TrafficLightState)Enum.ToObject(typeof(TrafficLightState), nextVal % (int)TrafficLightState.Count);
                tf.SetLightMaterial();
            }
            isReadyV = true;
        }
    }

}
