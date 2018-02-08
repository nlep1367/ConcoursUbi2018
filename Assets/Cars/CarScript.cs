using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour {

    public float speed = 5f;
    public float timeAlive = 11f;

    public Color[] colors;
   

    // Use this for initialization
    void Start () {
        Renderer rend = GetComponent<Renderer>();
        int randomChance = Random.Range(0, colors.Length);
        rend.material.color = colors[randomChance];        
    }
	
	// Update is called once per frame
	void Update () {
        transform.Translate(0, 0, speed * Time.deltaTime);
	}

    void OnEnable()
    {
        Invoke("Destroy", timeAlive);
    }

    void Destroy()
    {
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        CancelInvoke();
    }
}
