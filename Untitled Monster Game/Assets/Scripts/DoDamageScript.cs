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
    public float BACooldownTimer;

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
        if (BACooldownTimer > 0.0f)
            BACooldownTimer -= Time.deltaTime;

        if (BACooldownTimer < 0.0f)
            BACooldownTimer = 0.0f;

        // Account for attacking when not in collision
        if (CompareTag("Player"))
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.L)) // L is temporary because I'm viewing via scene
            {
                if (BACooldownTimer == 0.0f)
                {
                    BACooldownTimer = BasicAttackCooldown;
                }
            }
        }
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

                    if (other_hs && BACooldownTimer == 0.0f)
                    {
                        other_hs.TakeDamage(MeleeDamage);
                        BACooldownTimer = BasicAttackCooldown;
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

                    if (other_hs && BACooldownTimer == 0.0f)
                    {
                        other_hs.TakeDamage(MeleeDamage);
                        BACooldownTimer = BasicAttackCooldown;
                    }
                }
            }
        }
    }

    public float GetBasicAttackCDFraction()
    {
        return (BACooldownTimer / BasicAttackCooldown);
    }

    public float GetBasicAttackCooldownTimer()
    {
        return BACooldownTimer;
    }
}
