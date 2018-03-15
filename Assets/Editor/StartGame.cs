using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class StartGame : MonoBehaviour {

	[MenuItem("Custom/StartGame")]
    static void AddAllAi()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
        EditorApplication.ExecuteMenuItem("Edit/Play");
    }
}
