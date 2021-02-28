using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarScript : MonoBehaviour
{
    public GameObject model;
    HealthScript health;
    Slider healthbar;

    // Start is called before the first frame update
    void Start()
    {
        healthbar = GetComponent<Slider>();

        health = model.GetComponent<HealthScript>(); // Geez
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.value = health.GetHPFraction();
    }
}
