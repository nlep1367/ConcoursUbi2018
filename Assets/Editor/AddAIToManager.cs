using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NewBehaviourScript : MonoBehaviour {

	[MenuItem("Custom/AI/Add all AI")]
    static void AddAllAi()
    {
        GameObject Manager = GameObject.FindGameObjectWithTag("AI-Manager");
        AIManager AIManager = Manager.GetComponent<AIManager>();
        
        GameObject[] objects = GameObject.FindGameObjectsWithTag("AI");

        AIManager.AllAI.Clear();

        foreach(GameObject ai in objects)
        {
            AIManager.AllAI.Add(ai.GetComponent<BaseAI>());
        }
    }
}
