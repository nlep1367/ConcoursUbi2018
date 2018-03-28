using System.Collections.Generic;
using UnityEngine.Networking;
using System.ComponentModel;
using System.Linq;

public class SyncListObjectives : SyncListStruct<Objective> { };

public class ObjectiveManager : NetworkBehaviour, INotifyPropertyChanged
{
    public List<Objective> Objectives;
    private SyncListObjectives _currentObjectives;
    public SyncListObjectives CurrentObjectives;

    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Use this for initialization
    void Awake()
    {
        GameEssentials.ObjectiveManager = this;
        PropertyChanged = delegate { };
        CurrentObjectives = new SyncListObjectives();
        Objectives = new List<Objective> {
            new Objective
            {
                Id = 0,
                Title = "Avoid hole",
                Description = "Reach the exit of the Mira center parking lot without falling in any hole",
                SuccessPoints = 1000,
                FailurePoints = -100,
            },
            new Objective
            {
                Id = 1,
                Title = "Exit Mira Center parking",
                Description = "Reach the exit of the Mira center parking lot.",
                SuccessPoints = 1000,
                FailurePoints = 0,
            },
            new Objective
            {
                Id = 2,
                Title = "Change traffic light",
                Description = "Press the traffic light button in order to change the traffic lights to red and make cars stop.",
                SuccessPoints = 300,
                FailurePoints = -200,
            },
            new Objective
            {
                Id = 3,
                Title = "Get to the chopper!",
                Description = "The dude told you to go in some shady back alley. Now do it!",
                SuccessPoints = 1500,
                FailurePoints = 0,
            }
        };
    }

    public void Add(Objective objective)
    {
        if (!IsCurrentObjective(objective) && objective.Status == ObjectiveStateEnum.BACKLOG)
        {
            objective.Status = ObjectiveStateEnum.PROGRESS;
            CurrentObjectives.Add(objective);
            PropertyChanged(this, new PropertyChangedEventArgs("Add"));
        }
    }

    public void RemoveObjective(Objective objective)
    {
        if (IsCurrentObjective(objective))
        {
            CurrentObjectives.Remove(objective);
        }
    }

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

    public bool IsCurrentObjective(Objective objective)
    {
        return CurrentObjectives.Where(obj => obj == objective).Any();
    }

    [ClientRpc]
    public void Rpc_AddObjectiveToServer(Objective objective)
    {
        Add(objective);
    }

    [ClientRpc]
    public void Rpc_CompleteObjectiveToServer(Objective objective)
    {
        Complete(objective);
    }

    [ClientRpc]
    public void Rpc_FailObjectiveToServer(Objective objective)
    {
        Fail(objective);
    }

}