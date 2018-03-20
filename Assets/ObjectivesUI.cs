using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ObjectivesUI : MonoBehaviour {
    
    public GameObject ObjectiveModel;

    public GameObject ObjectiveList;

    List<GameObject> objectives;
    const int SHORT_SIZE = 20;

    private void Start()
    {
        objectives = new List<GameObject>();
    }

    private void ClearObjectives()
    {
        foreach(GameObject go in objectives)
        {
            Destroy(go);
        }
    }

    public void UpdateObjectives(List<string> objs)
    {
        ClearObjectives();

        foreach(string s in objs)
        {
            // Create Objective
            GameObject objective = Instantiate(ObjectiveModel);
            objective.GetComponentInChildren<Text>().text = s;

            objective.transform.SetParent(ObjectiveList.transform);
            objective.transform.localScale = new Vector3(1, 1, 1);
            objectives.Add(objective);
        }
    }
}
