using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storyboard : MonoBehaviour {

    public bool isShowing = true;
    public float turningSpeed = 60f;
    public float pageTime = 2f;
    public List<GameObject> pages = new List<GameObject>();

    int currentPage = 0;
    bool isTurning = false;
    float stoppingAngle = 135;

    // Use this for initialization
    void Start () {
        if (!isShowing)
        {
            stoppingAngle = 0;
            turningSpeed *= -1;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(!isTurning && currentPage != pages.Count)
            StartCoroutine("TurnPage");

        if (currentPage == pages.Count)
        {
            if (!isShowing)
            {
                // Somthing else, like changing scene to credits
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator TurnPage()
    {
        isTurning = true;

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
