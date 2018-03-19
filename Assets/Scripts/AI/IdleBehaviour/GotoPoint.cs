using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoPoint : IdleBehaviour
{
    public Transform Point;

    public override Vector3 Process()
    {
        return Point.position;
    }
}
