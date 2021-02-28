using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamageScript : MonoBehaviour
{
    public bool EnableRanged = false;
    public int RangedDamage = 1;

    public bool EnableMelee = false;
    public int MeleeDamage = 1;

    public float BasicAttackCooldown = 1.0f;
    public float cooldownTimer;

    GameObject parent;
    HealthScript myhealthscript;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;
        myhealthscript = parent.GetComponent<HealthScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer > 0.0f)
            cooldownTimer -= Time.deltaTime;

        if (cooldownTimer < 0.0f)
            cooldownTimer = 0.0f;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (CompareTag("AI"))
        {
            if (myhealthscript.GetAlive())
            {
                if (other.tag == "Player")
                {
                    HealthScript other_hs = other.transform.gameObject.GetComponent<HealthScript>();

                    if (other_hs && cooldownTimer == 0.0f)
                    {
                        other_hs.TakeDamage(MeleeDamage);
                        cooldownTimer = BasicAttackCooldown;
                    }
                }
            }
        }

        if (CompareTag("Player"))
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.L)) // L is temporary because I'm viewing via scene
            {
                if (other.tag == "AI")
                {
                    HealthScript other_hs = other.transform.gameObject.GetComponent<HealthScript>();

                    if (other_hs && cooldownTimer == 0.0f)
                    {
                        other_hs.TakeDamage(MeleeDamage);
                        cooldownTimer = BasicAttackCooldown;
                    }
                }
            }
        }
    }
}
