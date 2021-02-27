using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public int HP = 10;
    bool isAlive;

    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TakeDamage(int damage)
    {
        if (!isAlive)
            return;

        HP -= damage;

        // Hard limit minimal hp to 0
        if (HP < 0)
            HP = 0;

        if (HP == 0)
        {
            isAlive = false;

            if (CompareTag("Player"))
                print("YOU DIED!");

            GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f);
            GetComponent<BoxCollider2D>().isTrigger = true;
            GetComponent<Rigidbody2D>().velocity = new Vector3();
        }
    }

    public bool GetAlive()
    {
        return isAlive;
    }
}
