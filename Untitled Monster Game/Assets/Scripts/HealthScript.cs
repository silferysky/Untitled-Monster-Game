using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public int HP_Max = 10;
    public int HP_Current;
    bool isAlive;

    public bool IsLooted = false;

    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
        HP_Current = HP_Max;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TakeDamage(int damage)
    {
        if (!isAlive)
            return;

        HP_Current -= damage;

        // Hard limit minimal hp to 0
        if (HP_Current < 0)
            HP_Current = 0;

        if (HP_Current == 0)
        {
            isAlive = false;

            if (CompareTag("Player"))
                print("YOU DIED!");

            GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f);
            GetComponent<BoxCollider2D>().isTrigger = true;
            GetComponent<Rigidbody2D>().velocity = new Vector3();
            GetComponent<Rigidbody2D>().gravityScale = 0.0f;
        }
    }

    public bool GetAlive()
    {
        return isAlive;
    }

    public float GetHPFraction()
    {
        return ((float)HP_Current / (float)HP_Max);
    }
}
