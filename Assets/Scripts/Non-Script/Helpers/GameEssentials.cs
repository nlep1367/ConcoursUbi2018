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
}
