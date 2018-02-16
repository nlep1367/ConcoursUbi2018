using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkLobbyPlayer : NetworkLobbyPlayer
{
    void Update()
    {
        if (isLocalPlayer && ClientScene.ready && !readyToBegin)
        {
            readyToBegin = true;
            SendReadyToBeginMessage();
        }
    }
}
