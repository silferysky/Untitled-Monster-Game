using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class _TemplateAbilityScript : MonoBehaviour
{
    /*
        Default Variables:
            - PlayerModel   :   PlayerModel object for ease of reference
            - TargetMask    :   Which layer to damage/heal/whatever
            - Cooldown      :   Duration between casts
            - Autocast      :   Automatically casts when cooldown timer is zero

        Add as many public variables as you need for the ability to execute
    */
    public PlayerScript PlayerModel;
    public LayerMask TargetMask;
    public float Cooldown = 10.0f;
    public bool Autocast = false;

    // Interal variables
    float cooldownTimer;

    /*
        Always have this CallAbility function for standardization.

        Use this function to call for the ability to trigger 
        in your driver script in case of manual trigger
    */
    public void CallAbility()
    {
        // Do stuff here
        {


        }

        cooldownTimer = Cooldown;
    }

    void Start()
    {
    }

    void Update()
    {
        if (GameStateManager.gameState != GameState.Running)
            return;

        if (cooldownTimer > 0.0f)
            cooldownTimer -= Time.deltaTime;
        if (cooldownTimer < 0.0f)
            cooldownTimer = 0.0f;

        if (Autocast || Input.GetKeyDown(KeyCode.E))
        {
            if (cooldownTimer == 0.0)
                CallAbility();
        }
    }

    public float GetAbilityCooldownAsFraction()
    {
        return (cooldownTimer / Cooldown);
    }

    public float GetAbilityCooldownTimer()
    {
        return cooldownTimer;
    }
}
