using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoPoint : IdleBehaviour
{
    public Vector3 Point;

    public override Vector3 Process()
    {
        return Point;
    }
}
