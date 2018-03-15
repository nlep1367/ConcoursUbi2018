using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPerson : BaseCamera {
    public Transform Player;

    public virtual void SetPlayer(Transform Player)
    {
        this.Player = Player;
    }
}
