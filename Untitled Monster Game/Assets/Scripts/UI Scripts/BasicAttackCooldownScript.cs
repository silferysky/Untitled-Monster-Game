using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicAttackCooldownScript : MonoBehaviour
{
    DoDamageScript dmgScript;
    Image cooldown_fill;
    Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        cooldown_fill = GetComponent<Image>();

        GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in array)
        {
            if (go.GetComponent<DoDamageScript>())
            {
                dmgScript = go.GetComponent<DoDamageScript>();
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
        float cooldown_timer = 0.0f;

        if (dmgScript.EnableRanged)
        {
            cooldown_fill.fillAmount = dmgScript.GetRangedBasicCooldownAsFraction();
            cooldown_timer = dmgScript.GetRangedBasicCooldownTimer();
        }
        else if (dmgScript.EnableMelee)
        {
            cooldown_fill.fillAmount = dmgScript.GetMeleeBasicCooldownAsFraction();
            cooldown_timer = dmgScript.GetMeleeBasicCooldownTimer();
        }

        if (cooldown_timer > 0.0f)
            timerText.text = string.Format("{0:0.#}", cooldown_timer);
        else
            timerText.text = "";
    }
}
