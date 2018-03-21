using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Objective {
    public ObjectiveStateEnum Status;

    public int Id;
    public string Title;
    public string Description;
    public int SuccessPoints;
    public int FailurePoints;

    public int Succeed()
    {
        if (Status == ObjectiveStateEnum.PROGRESS)
        {
            Status = ObjectiveStateEnum.SUCCESS;
            return SuccessPoints;
        }
        return 0;
    }

    public int Fail()
    {
        if(Status == ObjectiveStateEnum.PROGRESS)
        {
            Status = ObjectiveStateEnum.FAIL;
            return FailurePoints;
        }

        return 0;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var objectToCompareWith = (Objective)obj;

        return objectToCompareWith.Id == Id;

    }

    public static bool operator ==(Objective o1, Objective o2)
    {
        return o1.Equals(o2);
    }

    public static bool operator !=(Objective o1, Objective o2)
    {
        return !o1.Equals(o2);
    }
}
