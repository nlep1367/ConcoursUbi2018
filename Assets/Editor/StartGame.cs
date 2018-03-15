using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;

[InitializeOnLoadAttribute]
public class StartGame : MonoBehaviour {
    
    static StartGame()
    {
        EditorApplication.playModeStateChanged += Test;
    }

    [MenuItem("Custom/StartGame")]
    static void GotoMainMenu()
    {
        EditorPrefs.SetString("MapName", EditorSceneManager.GetActiveScene().name);
        
        EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
        EditorApplication.ExecuteMenuItem("Edit/Play");
    }

    private static void Test(PlayModeStateChange state)
    {
        String MapName = EditorPrefs.GetString("MapName");
        
        if(state == PlayModeStateChange.EnteredEditMode && MapName != "")
        { 
            EditorSceneManager.OpenScene("Assets/Scenes/" + MapName + ".unity");
            EditorPrefs.SetString("MapName", "");
        }
    }
}
