using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAbility : MonoBehaviour
{
    bool isActive = false;

    public bool GetIsActive()
    {
        return isActive;
    }

    public void SetIsActive(bool set)
    {
        isActive = set;
        
        foreach (PlayerScript playerScript in GetComponents<PlayerScript>())
        {
            playerScript.activeAbility = this;
        }
    }

    public abstract void CallAbility();

    public abstract void DoAbility();

    public abstract void Start();

    public abstract void Update();

    public abstract float GetAbilityCooldownAsFraction();

    public abstract float GetAbilityCooldownTimer();
}
