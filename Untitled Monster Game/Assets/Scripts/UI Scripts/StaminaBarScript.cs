using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarScript : MonoBehaviour
{
    public GameObject Player; // The one with the movement script
    PlayerMovement playerMovement;
    Image staminaBar;

    // Start is called before the first frame update
    void Start()
    {
        staminaBar = GetComponent<Image>();
        playerMovement = Player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        staminaBar.fillAmount = playerMovement.GetStaminaAsFraction();
    }
}
