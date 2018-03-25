using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterestZoneManager : MonoBehaviour {

    public GameObject keyOne;
    public GameObject keyTwo;
    public GameObject squirrel;

    private List<GameObject> interestZones;
    private bool areZonesInitialized = false;


    // Use this for initialization
    void Start()
    {
        interestZones = new List<GameObject>();
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
                    interestZones.Add(zone.gameObject);
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
        SetUpObjectInZone(ref squirrel);
    }

    void SetUpObjectInZone(ref GameObject gameObj)
    {
        System.Random r = new System.Random();

        var index = r.Next(0, interestZones.Count);

        //move object to zone center
        gameObj.transform.position = interestZones[index].transform.position;
        interestZones.RemoveAt(index);
    }
}
