using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

[System.Serializable]
public class MatchScore
{
    public string name = "";
    public int score = 0;
}

[System.Serializable]
public class LeaderboardData
{
    public List<MatchScore> scores;

    public LeaderboardData()
    {
        scores = new List<MatchScore>();
    }
}

public class Leaderboard : MonoBehaviour {
    
    const string DATA_FILE_NAME = "data.json";

    LeaderboardData data;
    NetworkScoreManager scoreManager;

    public GameObject Score;
    public GameObject CurrentScore;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        LoadGameData();
    }

    private void FixedUpdate()
    {
        if(SceneManager.GetActiveScene().name == "Game" && scoreManager == null)
        {
            scoreManager = FindObjectOfType<NetworkScoreManager>();
        }
    }

    private bool LoadGameData()
    {
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, DATA_FILE_NAME);

        if (File.Exists(filePath))
        {
            // Read the json from the file into a string
            string dataAsJson = File.ReadAllText(filePath);
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            data = JsonUtility.FromJson<LeaderboardData>(dataAsJson);

            if(data == null)
            {
                data = new LeaderboardData();
            }

            return true;
        }
        else
        {
            Debug.LogError("Cannot load game data! : File do not exist.");
        }
        return false;
    }
    
    // Should only be used in the GameEnd script plz
    public void SavePlayerProgress(string matchName)
    {
        try
        {
            MatchScore newScore = new MatchScore
            {
                name = matchName,    // get player name
                score = scoreManager.GameScore    // get player score
            };
            data.scores.Add(newScore);

            SaveGameData();
        } catch(ArgumentException e)
        {
            Debug.Log(e);
        }
        
    }

    private void SaveGameData()
    {
        string dataAsJson = JsonUtility.ToJson(data);

        string filePath = Application.streamingAssetsPath + "/" + DATA_FILE_NAME;
        File.WriteAllText(filePath, dataAsJson);
    }

    public void Display(Transform parent)
    {
        while (parent.childCount > 0)
        {
            Destroy(parent.GetChild(0).gameObject);
        }

        data.scores.Sort(CompareScore);

        foreach (MatchScore match in data.scores)
        {
            GameObject newScore = (GameObject)GameObject.Instantiate(Score);

            Text[] texts = newScore.GetComponentsInChildren<Text>();
            texts[0].text = match.name;
            texts[1].text = match.score.ToString();

            newScore.transform.SetParent(parent);
            newScore.transform.localScale = new Vector3(1, 1, 1);
            newScore.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
    }

    private static int CompareScore(MatchScore s1, MatchScore s2)
    {
        if (s1.score == s2.score) return 0;

        if (s1.score > s2.score) return -1;

        return 1;
    }

}
