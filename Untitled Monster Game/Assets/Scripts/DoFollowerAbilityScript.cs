using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoFollowerAbilityScript : MonoBehaviour
{
    HealthScript myhealthscript;

    public float AbilityCooldown = 1.0f;
    public float AbilityCooldownTimer;

    // Start is called before the first frame update
    void Start()
    {
        myhealthscript = GetComponent<HealthScript>();
        AbilityCooldownTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.gameState != GameState.Running)
            return;

        if (AbilityCooldownTimer > 0.0f)
            AbilityCooldownTimer -= Time.deltaTime;

        if (AbilityCooldownTimer < 0.0f)
            AbilityCooldownTimer = 0.0f;

        DoAbility();
    }

    void DoAbility()
    {
        if (AbilityCooldownTimer == 0.0f)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Do Ability


                // Cooldown
                AbilityCooldownTimer = AbilityCooldown;
            }

        }
    }

    public float GetCooldownAsFraction()
    {
        return (AbilityCooldownTimer / AbilityCooldown);
    }

    public float GetCooldownTimer()
    {
        return AbilityCooldownTimer;
    }
}
