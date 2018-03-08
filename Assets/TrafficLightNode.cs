using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TrafficLightNode : NetworkBehaviour {

    public List<TrafficLight> horizontal;
    public List<TrafficLight> vertical;

    // red must equal green + yellow
    public float redLightDuration = 3f;
    public float yellowLightDuration = 1f;
    public float greenLightDuration = 2f;
    public float pedestrianDurationMultiplicator = 5f;

    private bool isReadyV = true;
    private bool isReadyH = true;
    private bool needResetPedestrian = false;

    [SyncVar]
    public bool hasPedestrian = false;

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

    public void Update()
    {
        if (isReadyV)
        {
            isReadyV = false;
            CountdownToNextLight(false);
        }

        if (isReadyH)
        {
            isReadyH = false;
            CountdownToNextLight(true);
        }
    }

    void CountdownToNextLight(bool isHorizontal)
    {
        List<TrafficLight> tfs = isHorizontal ? horizontal : vertical;

        if (tfs.Count == 0)
            return;

        float timeToWait = 0.0f;

        switch (tfs[0].lightState)
        {
            case TrafficLightState.Green:
                timeToWait = hasPedestrian ? greenLightDuration * pedestrianDurationMultiplicator : greenLightDuration;
                break;

            case TrafficLightState.Yellow:
                timeToWait = yellowLightDuration;
                break;

            case TrafficLightState.Red:
                timeToWait = hasPedestrian ? greenLightDuration * pedestrianDurationMultiplicator + yellowLightDuration : redLightDuration;
                break;
        }

        if (timeToWait > redLightDuration)
            needResetPedestrian = true;

        StartCoroutine(Wait(timeToWait, tfs, isHorizontal));
    }
   
    public IEnumerator Wait(float timeToWait, List<TrafficLight> tfs, bool isHorizontal)
    {
        yield return new WaitForSeconds(timeToWait);
        SetNextLight(tfs, isHorizontal);
    }

    public void SetNextLight(List<TrafficLight> tfs, bool isHorizontal)
    {
        foreach(TrafficLight tf in tfs)
        {
            int nextVal = (int)tf.lightState + 1;
            tf.lightState = (TrafficLightState)Enum.ToObject(typeof(TrafficLightState), nextVal % (int)TrafficLightState.Count);
            tf.SetLightMaterial();
        }

        if(isHorizontal)
            isReadyH = true;
        else
            isReadyV = true;

        if (needResetPedestrian)
        {
            //need command/rpclient
            hasPedestrian = false;
        }
    }
}
