using System.Collections.Generic;
using UnityEngine.Networking;
using System.ComponentModel;
using System.Linq;
using System.Collections;

public class SyncListObjectives : SyncListStruct<Objective> { };

public class ObjectiveManager : NetworkBehaviour, INotifyPropertyChanged
{
    public List<Objective> Objectives;
    private SyncListObjectives _currentObjectives;
    public List<Objective> CurrentObjectives;

    public event PropertyChangedEventHandler PropertyChanged;

    public System.Action<List<Objective>> ObjectivesChanged;

    // Use this for initialization
    void Awake()
    {
        GameEssentials.ObjectiveManager = this;
        PropertyChanged = delegate { };
        CurrentObjectives = new List<Objective>();
        Objectives = new List<Objective> {
            new Objective
            {
                Id = 0,
                Title = "Escape the Lot",
                Description = "Escape the Parking Lot",
                SuccessPoints = 500,
                FailurePoints = 0,
            },
            new Objective
            {
                Id = 1,
                Title = "Scare the Squirrels",
                Description = "Scare the SQUIRRELS",
                SuccessPoints = 50,
                FailurePoints = 0,
            },
            new Objective
            {
                Id = 2,
                Title = "Dodge the holes",
                Description = "Get through the parking lot without hitting any holes",
                SuccessPoints = 0,
                FailurePoints = 0,
            },
            new Objective
            {
                Id = 3,
                Title = "Save the Environment",
                Description = "Bonus : Save the environment",
                SuccessPoints = 0,
                FailurePoints = 0,
            },
            new Objective
            {
                Id = 4,
                Title = "Cross Safely Part 1",
                Description = "Cross the street safely",
                SuccessPoints = 100,
                FailurePoints = 0,
            },
            new Objective
            {
                Id = 5,
                Title = "Asking for help Part 1",
                Description = "Ask anyone for the direction to the Dog Cafe",
                SuccessPoints = 25,
                FailurePoints = 0,
            },
            new Objective
            {
                Id = 6,
                Title = "The alley",
                Description = "Get through the alley",
                SuccessPoints = 750,
                FailurePoints = 0,
            },
            new Objective
            {
                Id = 7,
                Title = "One, or two, key to rule them all",
                Description = "Find two keys in the alley to open the door",
                SuccessPoints = 100,
                FailurePoints = 0,
            },
            new Objective
            {
                Id = 8,
                Title = "Cross Safely Part 2",
                Description = "Cross the street safely (part 2)",
                SuccessPoints = 150,
                FailurePoints = 0,
            },
            new Objective
            {
                Id = 9,
                Title = "Ask Direction (part 2)",
                Description = "Ask someone else for the direction to the Dog Cafe (again)",
                SuccessPoints = 25,
                FailurePoints = 0,
            },
            new Objective
            {
                Id = 10,
                Title = "Barrier Key",
                Description = "Find the Barrier Key",
                SuccessPoints = 50,
                FailurePoints = 0,
            },
            new Objective
            {
                Id = 11,
                Title = "Get Inside!",
                Description = "Get inside the Market Place",
                SuccessPoints = 250,
                FailurePoints = 0,
            },
            new Objective
            {
                Id = 12,
                Title = "The End",
                Description = "Find the Dog Cafe",
                SuccessPoints = 500,
                FailurePoints = 0,
            },
            new Objective
            {
                Id = 13,
                Title = "Get over there",
                Description = "Help Echo get on the dumpster",
                SuccessPoints = 250,
                FailurePoints = 0,
            },
            new Objective
            {
                Id = 14,
                Title = "Stuck Door",
                Description = "Unblock the door",
                SuccessPoints = 100,
                FailurePoints = 0,
            }
        };
    }

    public void Add(Objective objective)
    {
        CurrentObjectives.Add(objective);
        //objective.Status = ObjectiveStateEnum.PROGRESS;
        //CurrentObjectives.Add(objective);
        if(ObjectivesChanged != null)
            ObjectivesChanged.Invoke(CurrentObjectives);
    }

    public void RemoveObjectives()
    {
        foreach(Objective obj in CurrentObjectives)
        {
            GameEssentials.PlayerGirl.GetComponent<PlayerScoreManager>().Cmd_AddPoints(
                new ScoreObj( obj.SuccessPoints, "Objective Completed"));
        }

        CurrentObjectives.Clear();

        if (ObjectivesChanged != null)
            ObjectivesChanged.Invoke(CurrentObjectives);
    }
    /*
    public int Complete(Objective objective)
    {
        if (IsCurrentObjective(objective))
        {
            RemoveObjective(objective);
            PropertyChanged(this, new PropertyChangedEventArgs("Complete"));
            return objective.Succeed();
        }
        return 0;
    }
    */
    /*
    public int Fail(Objective objective)
    {
        if (IsCurrentObjective(objective))
        {
            RemoveObjective(objective);
            PropertyChanged(this, new PropertyChangedEventArgs("Fail"));

            return objective.Fail();
        }
        return 0;
    }
    */
   /* public bool IsCurrentObjective(Objective objective)
    {
        return CurrentObjectives.Where(obj => obj == objective).Any();
    }*/

    [ClientRpc]
    public void Rpc_AddObjectiveToServer(Objective objectives)
    {
        Add(objectives);
    }

    [ClientRpc]
    public void Rpc_RemoveObjectives()
    {
        RemoveObjectives();
    }
    /*
    [ClientRpc]
    public void Rpc_FailObjectiveToServer(Objective objective)
    {
        Fail(objective);
    }
    */
}