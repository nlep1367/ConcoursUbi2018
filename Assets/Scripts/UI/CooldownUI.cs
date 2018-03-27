using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour {

    Image imageCooldown;
    bool isUsed = false;

    public float cooldown = 0f;

    private void Start()
    {
        imageCooldown = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update () {
        if (isUsed) {
            Effect();
        }
	}

    public void StartAbility(float time)
    {
        if(time > 0)
        {
            isUsed = true;
            cooldown = time;
            imageCooldown.fillAmount = 1;
        }
    }

    private void Effect()
    {
        imageCooldown.fillAmount -= 1 / cooldown * Time.deltaTime;

        if (imageCooldown.fillAmount <= 0)
        {
            imageCooldown.fillAmount = 0;
            isUsed = false;
        }
    }
}
