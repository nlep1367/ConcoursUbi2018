using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ability
{
    bool isInCooldown = false;
    float startedTime;

    public float cooldown = 2f;

    [Header("Controls Cooldown Display [Optional]")]
    public CooldownUI controlUI;

    [Header("Hint Display [Optional]")]
    public HintUI hintUI;
    public KeyCode keyRef;
    public string actionText;

    public bool CanDoAbility()
    {
        if ((startedTime + cooldown) >= Time.time)
            isInCooldown = false;

        return !isInCooldown;
    }

    public bool DisplayHint()
    {
        if (CanDoAbility() && hintUI)
        {
            hintUI.Display(keyRef, actionText);

            return true;
        }

        return false;
    }
    public void HideHint()
    {
        if(hintUI)
            hintUI.Hide();
    }

    public bool DoAction()
    {
        if (!CanDoAbility())
        {
            return false;
        }

        isInCooldown = true;
        startedTime = Time.time;

        if (controlUI)
        {
            controlUI.StartAbility(cooldown);
        }

        return true;
    }

}