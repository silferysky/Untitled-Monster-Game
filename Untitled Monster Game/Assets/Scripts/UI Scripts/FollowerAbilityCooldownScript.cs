using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowerAbilityCooldownScript : MonoBehaviour
{
    DoFollowerAbilityScript doAbilScript;
    Image cooldown_fill;
    Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        cooldown_fill = GetComponent<Image>();

        GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in array)
        {
            if (go.GetComponent<DoFollowerAbilityScript>())
            {
                doAbilScript = go.GetComponent<DoFollowerAbilityScript>();
                break;
            }
        }

        foreach (Transform child in transform.parent)
        {
            if (child.tag == "TimerText")
            {
                timerText = child.GetComponent<Text>();
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        cooldown_fill.fillAmount = doAbilScript.GetCooldownAsFraction();

        float cooldown_timer = doAbilScript.GetCooldownTimer();
        if (cooldown_timer > 0)
            timerText.text = string.Format("{0:0.#}", cooldown_timer);
        else
            timerText.text = "";
    }
}
