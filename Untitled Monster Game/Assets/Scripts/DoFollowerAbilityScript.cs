using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoFollowerAbilityScript : MonoBehaviour
{
    PlayerScript playerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.gameState != GameState.Running)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            playerScript.activeAbility.CallAbility();
        }
    }

    public float GetCooldownAsFraction()
    {
        if (playerScript.activeAbility)
            return playerScript.activeAbility.GetAbilityCooldownAsFraction();
        else
            return 0.0f;
    }

    public float GetCooldownTimer()
    {
        if (playerScript.activeAbility)
            return playerScript.activeAbility.GetAbilityCooldownTimer();
        else
            return 0.0f;
    }
}
