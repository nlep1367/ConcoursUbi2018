using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Storyboard : MonoBehaviour {

    public bool isShowing = true;
    public float turningSpeed = 60f;
    public float pageTime = 2f;
    public List<GameObject> pages = new List<GameObject>();


    int currentPage = 0;
    bool isTurning = false;
    float stoppingAngle = 135;

	bool isStartStoryAmbientMusicCanged = false;
	bool isEndStoryAmbientMusicChanged = false;


    // Use this for initialization
    void Start () {
        if (!isShowing)
        {
            stoppingAngle = 0;
            turningSpeed *= -1;
        }
		GameEssentials.MusicPlayer.InStoryboard.TransitionTo (0);
	}
	
	// Update is called once per frame
	void Update () {
        if(!isTurning && currentPage != pages.Count)
            StartCoroutine("TurnPage");

        if (currentPage == pages.Count)
        {
            if (!isShowing)
            {
				//Fin
                // Somthing else, like changing scene to credits
                Destroy(gameObject);
            }
            else
            {
				// ChangeAmbient
                Destroy(gameObject);
            }
		}

		if (currentPage == pages.Count - 1)
		{
			if (isShowing) {
				// Debut
				if (!isStartStoryAmbientMusicCanged) {
					isStartStoryAmbientMusicCanged = true;
					GameEssentials.MusicPlayer.ToNextAmbient = true;
					GameEssentials.MusicPlayer.InGame.TransitionTo (2.5f);
				}
			}
		}

		if (currentPage == 0) {
			if (!isShowing) {
				if (!isEndStoryAmbientMusicChanged) {
					GameEssentials.MusicPlayer.InStoryboard.TransitionTo (1f);
					isEndStoryAmbientMusicChanged = true;
					GameEssentials.MusicPlayer.ToNextAmbient = true;
				}
			} else {
				GameEssentials.MusicPlayer.InStoryboard.TransitionTo (0);
			}
		}
    }

    IEnumerator TurnPage()
    {
        isTurning = true;
		// PageTurning sound
		GetComponent<AudioSource>().PlayDelayed (1.75f);

        if(isShowing)
            yield return new WaitForSeconds(pageTime);

        Transform page = pages[currentPage].transform;
        while (shouldRotate(page.rotation.eulerAngles.x))
        {
            page.Rotate(new Vector3(turningSpeed * Time.deltaTime, 0, 0));
            yield return null;
        }

        if (!isShowing)
            yield return new WaitForSeconds(pageTime);

        isTurning = false;
        currentPage++;        
    }

    bool shouldRotate(float rotation)
    {
        if (isShowing)
        {
            return rotation <= stoppingAngle;
        }
        else
        {
            return rotation <= 90;
        }
    }
}
