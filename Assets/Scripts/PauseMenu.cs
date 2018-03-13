using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PauseMenu : MonoBehaviour {

    private void OnEnable()
    {
        // TODO: lock controlls for the player
        // TODO: Maybe pause game
    }
    
    public void Exit()
    {
        if (NetworkServer.active)
            NetworkManager.singleton.StopHost();
        else
            GameObject.Find("LobbyManager").GetComponent<MatchMakingLobbyManager>().ExitMatch();
    }


}