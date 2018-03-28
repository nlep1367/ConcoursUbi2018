using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class ObjectivesUI : MonoBehaviour {
    
    public GameObject ObjectiveModel;

    public GameObject ObjectiveList;

    List<GameObject> objectives;
    const int SHORT_SIZE = 20;

    public ObjectiveManager OManager;

    private void Start()
    {
        objectives = new List<GameObject>();
        OManager.ObjectivesChanged += UpdateObjectives;
        //OManager.PropertyChanged += ObjectiveManager_PropertyChanged;
    }

    private void ObjectiveManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        //Letting it there if we wish to be more performant
        //if(e.PropertyName == "Add")
        //{

        //}
        //else if (e.PropertyName == "Complete")
        //{

        //}
        //else if (e.PropertyName == "Fail")
        //{

        //}
       // IEnumerable<string> objs = GameEssentials.ObjectiveManager.CurrentObjectives.Select<Objective, string>(obj => obj.Title);
       // UpdateObjectives(objs);
    }

    private void ClearObjectives()
    {
        foreach(GameObject go in objectives)
        {
            Destroy(go);
        }
    }

    public void UpdateObjectives(List<Objective> objs)
    {
        ClearObjectives();

        foreach(Objective obj in objs)
        {
            // Create Objective
            GameObject objective = Instantiate(ObjectiveModel);
            objective.GetComponentInChildren<Text>().text = obj.Description;

            objective.transform.SetParent(ObjectiveList.transform);
            objective.transform.localScale = new Vector3(1, 1, 1);
            objectives.Add(objective);
        }
    }
}
