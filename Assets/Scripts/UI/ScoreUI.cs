using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour {

    public GameObject NewPointsPanel;
    public Text scoreText;

    private int score = 0;
    private Queue<ScoreObj> pointsToShow;

    float duration = 0f;
    public float normalDuration = 1.5f;
    public float fastDuration = .5f;
    public int slowThreshold = 2;
    float currentTime = 0;
    bool isShowing = false;

    private NetworkScoreManager scoreManager;

    // Use this for initialization
    void Start () {
        pointsToShow = new Queue<ScoreObj>();
        duration = normalDuration;        
    }
	
	// Update is called once per frame
	void Update () {

        if (scoreManager == null)
        {
            GameObject temp = GameObject.FindGameObjectWithTag("ScoreManager");

            if(temp != null)
            {
                scoreManager = temp.GetComponent<NetworkScoreManager>();
                scoreManager.displayAddScore += AddScore;
                scoreManager.displayLoseScore += LoseScore;
            }
        }

        if (pointsToShow.Count > 0)
        {
            currentTime += Time.deltaTime;
            if (!isShowing)
            {
                currentTime = 0;
                isShowing = true;

                // Display message
                NewPointsPanel.GetComponentInChildren<Text>().text = pointsToShow.Peek().ToString();

                if (!NewPointsPanel.activeSelf)
                {
                    NewPointsPanel.SetActive(true);
                }

                // Adaptive duration
                duration = (pointsToShow.Count > slowThreshold) ? fastDuration : normalDuration;
            }
            else if (currentTime > duration)
            {
                isShowing = false;
                UpdateScore();
                pointsToShow.Dequeue();
            }
        }
        else if (NewPointsPanel.activeSelf)
        {
            NewPointsPanel.SetActive(false);
        }
    }

    public void UpdateScore()
    {
        score += pointsToShow.Peek().points;

        if (score < 0)
            score = 0;

        scoreText.text = score.ToString() + " pts";
    }

    public void AddScore(ScoreObj scoreObj)
    {
        pointsToShow.Enqueue(scoreObj);
    }

    public void LoseScore(ScoreObj scoreObj)
    {
        scoreObj.points = -scoreObj.points;
        pointsToShow.Enqueue(scoreObj);
    }
}
