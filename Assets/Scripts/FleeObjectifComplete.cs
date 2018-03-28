using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class FleeObjectifComplete : MonoBehaviour {


    public FleeAI FleeAI;
    public int ObjId;

    // Use this for initialization
	void Start () {
        FleeAI.WasSpooked += SquirrelWasSpooked;
    }

    void SquirrelWasSpooked()
    {
        GameEssentials.ObjectiveSync.Cmd_CompleteObjectiveToServer(GameEssentials.ObjectiveManager.Objectives.Where(o => o.Id == ObjId).First());

        FleeAI.WasSpooked -= SquirrelWasSpooked;
    }
}
