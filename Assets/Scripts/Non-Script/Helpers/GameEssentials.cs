using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public static class GameEssentials {
    public static ObjectiveManager ObjectiveManager;
    public static DialogueManager DialogueManager;
    public static ObjectiveSync ObjectiveSync;
    public static DialogueSync DialogueSync;
    public static NetworkScoreManager ScoreManager;

    public static PlayerGirl PlayerGirl;
    public static PlayerDog PlayerDog;
    public static NPC Npc;
    public static AmbientMusicControl MusicPlayer;


    public static bool IsGirl(Collider c)
    {
        NetworkBehaviour networkBehaviour = c.GetComponentInParent<NetworkBehaviour>();
        return networkBehaviour && networkBehaviour.isLocalPlayer && c.CompareTag(ConstantsHelper.PlayerGirlTag);
    }

    public static void ApplyObjectives(IEnumerable<int> ids, ObjectiveStateEnum state)
    {
        foreach (int id in ids)
        {
            Objective objective = ObjectiveManager.Objectives.Where(o => o.Id == id).First();
            switch (state)
            {
                case ObjectiveStateEnum.FAIL:
                    GameEssentials.ObjectiveSync.Cmd_FailObjectiveToServer(objective);
                    break;
                case ObjectiveStateEnum.SUCCESS:
                    GameEssentials.ObjectiveSync.Cmd_CompleteObjectiveToServer(objective);
                    break;
                case ObjectiveStateEnum.PROGRESS:
                    GameEssentials.ObjectiveSync.Cmd_AddObjectiveToServer(objective);
                    break;
            }
        }
    }
}
