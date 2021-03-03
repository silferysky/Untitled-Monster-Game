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
    public GameObject Lightning;
    List<GameObject> lightnings = null;

    // Interal variables
    float cooldownTimer;
    bool wasCalled;
    public List<GameObject> targets;
    float fadeoutDuration = 2.0f;
    float fadeoutTimer;
    Color origColor;
    bool isCasting;

    List<GameObject> GetTargets()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(gameObject.transform.position, MaxRange, TargetMask);
        foreach (Collider2D enemy in enemies)
        {
            targets.Add(enemy.gameObject);
        }
        targets = targets.SortByDistance(gameObject.transform.position);

        if (targets.Count < MaxTargets)
            targets.GetRange(0, targets.Count);
        else
            targets.GetRange(0, MaxTargets);

        return targets;
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
        origColor = Lightning.GetComponent<SpriteRenderer>().color;

        foreach (GameObject target in targets)
        {
            GameObject lightning;
            lightning = Instantiate(Lightning, target.transform.position, target.transform.rotation);
            lightnings.Add(lightning);

            print("Add");
        }

        fadeoutTimer = 0.0f;
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
                isCasting = true;
                DoAbility();
            }
        }

        wasCalled = false;

        if (isCasting)
        {
            if (fadeoutTimer < fadeoutDuration)// Fade out lighting
            {
                foreach (GameObject ln in lightnings)
                {
                    Color color = ln.GetComponent<SpriteRenderer>().color;
                    ln.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, Mathf.Lerp(origColor.a, 0.0f, fadeoutTimer / fadeoutDuration));
                }

                fadeoutTimer += Time.deltaTime;
            }
            else
            {
                foreach (GameObject ln in lightnings)
                {
                    Destroy(ln);
                }
                isCasting = false;
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
    
}
