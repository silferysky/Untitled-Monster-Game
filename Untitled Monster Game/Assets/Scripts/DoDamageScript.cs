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

    public LayerMask EnemyMask;
    
    public Transform MeleeAttackPos;
    public float MeleeAttackRadius = 0.0f;

    public Transform RangedAttackPos;
    public float RangedAttackRadius = 0.0f;

    HealthScript myhealthscript;

    // Start is called before the first frame update
    void Start()
    {
        myhealthscript = GetComponent<HealthScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (BACooldownTimer > 0.0f)
            BACooldownTimer -= Time.deltaTime;

        if (BACooldownTimer < 0.0f)
            BACooldownTimer = 0.0f;

        if (CompareTag("Player"))
        {
            if (Input.GetMouseButtonDown(1))
            {
                EnableMelee = !EnableMelee;
                EnableRanged = !EnableRanged;
            }
        }

        ActuallyDoDamage();
    }

    void ActuallyDoDamage()
    {
        if (CompareTag("AI"))
        {
            if (myhealthscript.GetAlive())
            {
                if (BACooldownTimer == 0.0f)
                {
                    Collider2D[] enemiesToDamage;

                    if (EnableMelee)
                    {
                        enemiesToDamage = Physics2D.OverlapCircleAll(MeleeAttackPos.position, MeleeAttackRadius, EnemyMask);

                        if (enemiesToDamage.Length > 0)
                        {
                            foreach (Collider2D player in enemiesToDamage)
                            {
                                player.GetComponent<HealthScript>().TakeDamage(RangedDamage);
                            }
                        }
                    }
                    if (EnableRanged)
                    {
                        enemiesToDamage = Physics2D.OverlapCircleAll(RangedAttackPos.position, RangedAttackRadius, EnemyMask);

                        if (enemiesToDamage.Length > 0)
                        {
                            foreach (Collider2D player in enemiesToDamage)
                            {
                                player.GetComponent<HealthScript>().TakeDamage(RangedDamage);
                            }
                        }
                    }

                    BACooldownTimer = BasicAttackCooldown;
                }
            }
        }

        if (CompareTag("Player"))
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.L)) // L is temporary because I'm viewing via scene
            {
                if (BACooldownTimer == 0.0f)
                {
                    Collider2D[] enemiesToDamage = null;

                    if (EnableMelee)
                        enemiesToDamage = Physics2D.OverlapCircleAll(MeleeAttackPos.position, MeleeAttackRadius, EnemyMask);
                    if (EnableRanged)
                        enemiesToDamage = Physics2D.OverlapCircleAll(RangedAttackPos.position, RangedAttackRadius, EnemyMask);

                    foreach (Collider2D enemy in enemiesToDamage)
                    {
                        if (EnableMelee)
                        {
                            enemy.GetComponent<HealthScript>().TakeDamage(MeleeDamage);
                        }
                        if (EnableRanged)
                        {
                            enemy.GetComponent<HealthScript>().TakeDamage(RangedDamage);
                        }
                    }

                    BACooldownTimer = BasicAttackCooldown;
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
