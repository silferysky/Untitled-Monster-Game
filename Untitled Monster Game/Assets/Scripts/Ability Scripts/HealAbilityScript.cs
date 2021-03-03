using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class HealAbilityScript : IAbility
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
    bool wasCalled;
    float cooldownTimer;

    HealthScript healthScript;

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
        if (Autocast && (healthScript.HP_Current < healthScript.HP_Max))
        {
            healthScript.HealCheat(HealAmount);

            cooldownTimer = Cooldown;
        }
        else if (wasCalled)
        {
            healthScript.HealCheat(HealAmount);

            cooldownTimer = Cooldown;
        }
    }

    public override void SetIsActive(bool set)
    {
        base.SetIsActive(set); // Must have this

        if (set)
            PlayerModel.activeAbility = this;
        else
            PlayerModel.activeAbility = null;
    }

    public override void Start()
    {
        wasCalled = false;

        healthScript = Target.GetComponent<HealthScript>();
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
