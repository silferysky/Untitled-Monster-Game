using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class LightningAbility : IAbility
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

    public int MaxTargets = 1;
    public float MaxRange = 1.0f;
    public GameObject LightningPrefab;
    public int Damage = 1;

    // Interal variables
    float cooldownTimer;
    bool wasCalled;
    public List<GameObject> targets;
    Color origColor;
    float fadeoutDuration = 2.0f;

    List<GameObject> GetTargets()
    {
        List<GameObject> targets = new List<GameObject>();
        Collider2D[] enemies = Physics2D.OverlapCircleAll(PlayerModel.transform.position, MaxRange, TargetMask);

        foreach (Collider2D enemy in enemies)
        {
            targets.Add(enemy.gameObject);
        }
        targets = targets.SortByDistance(PlayerModel.transform.position);

        if (targets.Count < MaxTargets)
        {
            return targets.GetRange(0, targets.Count);
        }
        else
        {
            return targets.GetRange(0, MaxTargets);
        }
    }

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
        origColor = LightningPrefab.GetComponent<SpriteRenderer>().color;

        foreach (GameObject target in GetTargets())
        {
            GameObject ln;
            ln = Instantiate(LightningPrefab, target.transform.position, target.transform.rotation);
            ln.GetComponent<Lightning>().SetParams(fadeoutDuration, origColor);
            target.GetComponent<HealthScript>().TakeDamage(Damage);
        }

        cooldownTimer = Cooldown;
    }

    public override void SetIsActive(bool set)
    {
        base.SetIsActive(set); // Must have this

        if (set)
        {
            PlayerModel.activeAbility = this;
        }
        else
            PlayerModel.activeAbility = null;
    }

    public override void Start()
    {
        //SetIsActive(true);
        wasCalled = false;

        if (MaxTargets < 0)
            MaxTargets = 0;
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
                targets = GetTargets();
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
