using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoFollowerAbilityScript : MonoBehaviour
{
    IAbility[] abilities = null;

    // Start is called before the first frame update
    void Start()
    {
        abilities = GetComponents<IAbility>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.gameState != GameState.Running)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (abilities.Length > 0)
                abilities[0].CallAbility();
        }
    }

    public float GetCooldownAsFraction()
    {
        if (abilities.Length > 0)
            return abilities[0].GetAbilityCooldownAsFraction();
        else
            return 0.0f;
    }

    public float GetCooldownTimer()
    {
        if (abilities.Length > 0)
            return abilities[0].GetAbilityCooldownTimer();
        else
            return 0.0f;
    }
}
