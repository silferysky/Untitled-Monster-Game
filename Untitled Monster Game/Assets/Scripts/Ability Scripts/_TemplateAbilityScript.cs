using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class _TemplateAbilityScript : IAbility
{
    // NOTE: Make sure you have DoFamiliarAbilityScript in the Player as well! *******
    
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

    // Interal variables
    float cooldownTimer;
    bool wasCalled;

    /*
        Always have this CallAbility function for standardization.

        Use this function to call for the ability to trigger 
        in your driver script in case of manual trigger
    */
    public override void CallAbility()
    {
        wasCalled = true;
    }

    public override void DoAbility()
    {
        // Do stuff here

        cooldownTimer = Cooldown;
    }

    public override void Start()
    {
        wasCalled = false;
    }

    public override void Update()
    {
        if (GameStateManager.gameState != GameState.Running)
            return;

        if (cooldownTimer > 0.0f)
            cooldownTimer -= Time.deltaTime;
        if (cooldownTimer < 0.0f)
            cooldownTimer = 0.0f;

        if (cooldownTimer == 0.0)
        {
            if (Autocast || wasCalled)
            {
                DoAbility();
            }
        }

        wasCalled = false;
    }

    public override float GetAbilityCooldownAsFraction()
    {
        return (cooldownTimer / Cooldown);
    }

    public override float GetAbilityCooldownTimer()
    {
        return cooldownTimer;
    }
}
