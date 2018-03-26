using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterestZoneManager : MonoBehaviour {

    public GameObject keyOne;
    public GameObject keyTwo;
    public GameObject squirrel;

    public List<InterestZone> interestZones;
    private bool areZonesInitialized = false;


    // Use this for initialization
    void Start()
    {
        areZonesInitialized = interestZones.Count > 3;
    }

    void Update()
    {
        if (!areZonesInitialized)
        {
            var interestZonesFound = GameObject.FindObjectsOfType<InterestZone>();

            if(interestZonesFound.Length > 3)
            {
                foreach(var zone in interestZonesFound)
                {
                    interestZones.Add(zone);
                }
                areZonesInitialized = true;
            }
        }
        else
        {
            SetUpInterestZones();
        }
    }

    void SetUpInterestZones()
    {
        SetUpObjectInZone(ref keyOne);
        SetUpObjectInZone(ref keyTwo);
        SetUpSquirrelInZone(ref squirrel);
    }

    int GetRandomIndex()
    {
        System.Random r = new System.Random();
        return r.Next(0, interestZones.Count);
    }

    void SetUpObjectInZone(ref GameObject gameObj)
    {
        var index = GetRandomIndex();

        gameObj.transform.position = interestZones[index].transform.position;
        interestZones.RemoveAt(index);
    }

    void SetUpSquirrelInZone(ref GameObject gameObj)
    {
        var index = GetRandomIndex();

        Transform trans = interestZones[index].transform;

        FleeAI fleeAi = gameObj.GetComponent<FleeAI>();
        fleeAi.FirstPoint = interestZones[index].PathStart.transform;

        gameObj.transform.position = trans.position;
        interestZones.RemoveAt(index);
    }
}
