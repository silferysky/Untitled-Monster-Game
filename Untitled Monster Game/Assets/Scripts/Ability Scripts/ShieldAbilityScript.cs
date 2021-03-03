using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ShieldAbilityScript : IAbility
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
    public LayerMask ProjectileLayer;
    public float Cooldown = 10.0f;
    public bool Autocast = false;

    public GameObject Shield;
    public float ShieldDuration = 1.5f;

    // Interal variables
    bool wasCalled;
    float cooldownTimer;

    bool isShieldActive;
    float shieldTimer;

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
        shieldTimer = ShieldDuration;
        isShieldActive = true;

        PlayerModel.GetComponent<HealthScript>().IgnoreDamage(ShieldDuration);
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
        //SetIsActive(true);
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

        if (isShieldActive)
        {
            shieldTimer -= Time.deltaTime;

            if (shieldTimer > 0.0f)
            {
                MaintainShield();
            }
            else if (shieldTimer <= 0.0f)
            {
                shieldTimer = 0.0f;
                isShieldActive = false;
                Shield.SetActive(false);
                cooldownTimer = Cooldown; // Cooldown only starts ticking when shield expires
            }
        }
    }

    public override float GetAbilityCooldownAsFraction()
    {
        return (cooldownTimer / Cooldown);
    }

    public override float GetAbilityCooldownTimer()
    {
        return cooldownTimer;
    }

    public void MaintainShield()
    {
        // Note: Uses shield's x local scale as radius

        Shield.SetActive(true);

        Collider2D[] projectilesToDestroy = Physics2D.OverlapCircleAll(Shield.transform.position, Shield.transform.localScale.x / 2, ProjectileLayer);

        foreach (Collider2D projectile in projectilesToDestroy)
        {
            print("Blocked!");
            Destroy(projectile.gameObject);
        }
    }
}
