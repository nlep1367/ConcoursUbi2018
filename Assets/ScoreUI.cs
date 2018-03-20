using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Point
{
    public int score = 0;
    public string content;

    public Point(int p, string c = "")
    {
        score = p;
        content = c;
    }

    public override string ToString()
    {
        string message = (score >= 0) ? "+" : "-";
        return message + score.ToString() + " " + content;
    }
}


public class ScoreUI : MonoBehaviour {

    public GameObject NewPointsPanel;
    public Text scoreText;

    private int score = 0;
    private Queue<Point> pointsToShow;

    float duration = 0f;
    public float normalDuration = 1.5f;
    public float fastDuration = .5f;
    public int slowThreshold = 2;
    float currentTime = 0;
    bool isShowing = false;

    // Use this for initialization
    void Start () {
        pointsToShow = new Queue<Point>();
        duration = normalDuration;
	}
	
	// Update is called once per frame
	void Update () {
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
        score += pointsToShow.Peek().score;
        scoreText.text = score.ToString() + " pts";
    }

    public void AddScore(int newScore, string reason)
    {
        pointsToShow.Enqueue(new Point(newScore, reason));
    }
}
