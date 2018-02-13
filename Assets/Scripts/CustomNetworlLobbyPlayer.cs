using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworlLobbyPlayer : NetworkLobbyPlayer
{

    public override void OnClientEnterLobby()
    {
        base.OnClientEnterLobby();

        readyToBegin = true;
        SendReadyToBeginMessage();
    }

}
