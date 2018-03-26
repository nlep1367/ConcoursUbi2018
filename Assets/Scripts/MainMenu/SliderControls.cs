using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderControls : MonoBehaviour {

    public EventSystem sys;
    public float speed = 0.1f;

    Slider slider;
    bool isDown = false;

    private void Start()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update () {
		if(sys.currentSelectedGameObject == gameObject)
        {
            /*
             * if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                slider.value += speed;
            } else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                slider.value -= speed;
            }
            */

            /*float axisvalue = Input.GetAxis("Horizontal_Move");

            if (Mathf.Abs(axisvalue) >= 0.99f && !isDown)
            {
                isDown = true;
                slider.value = 1;
            }
            else if (Mathf.Abs(axisvalue) <= 0.1f)
            {
                isDown = false;
            }*/
        }
	}
}
