using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class HealAbilityScript : MonoBehaviour
{
    /*
        Default Variables:
            - PlayerModel   :   PlayerModel object for ease of reference
            - Target        :   Single target OR
            - TargetMask    :   Multi target via a Layer Mask
            - Cooldown      :   Duration between casts
            - Autocast      :   Automatically casts when cooldown timer is zero

        Add as many public variables as you need for the ability to execute
    */
    public PlayerScript PlayerModel;
    public GameObject Target;
    public LayerMask TargetMask;
    public float Cooldown = 10.0f;
    public bool Autocast = false;

    public int HealAmount;

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
            Target.GetComponent<HealthScript>().HealCheat(HealAmount);
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
