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
        cooldown_fill.fillAmount = dmgScript.GetBasicAttackCDFraction();

        float cooldown_timer = dmgScript.GetBasicAttackCooldownTimer();
        if (cooldown_timer > 0)
            timerText.text = string.Format("{0:0.#}", cooldown_timer);
        else
            timerText.text = "";
    }
}
