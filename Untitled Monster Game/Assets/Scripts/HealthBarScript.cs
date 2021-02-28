using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    HealthScript health;
    Image healthBar_fill;

    // Start is called before the first frame update
    void Start()
    {
        healthBar_fill = GetComponent<Image>();

        GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject go in array)
        {
            if (go.GetComponent<HealthScript>())
            {
                health = go.GetComponent<HealthScript>();
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        healthBar_fill.fillAmount = health.GetHPFraction();
    }
}
