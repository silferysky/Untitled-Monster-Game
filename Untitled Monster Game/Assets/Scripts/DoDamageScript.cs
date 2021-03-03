﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamageScript : MonoBehaviour
{
    public GameObject player;
    public LayerMask EnemyMask;
    public Animator animator;

    public bool EnableRanged = false;
    public int RangedDamage = 1;

    public bool EnableMelee = false;
    public int MeleeDamage = 1;

    public float RangedBasicAttackCooldown = 1.0f;
    public float MeleeBasicAttackCooldown = 1.0f;

    public Transform MeleeAttackPos;
    public float MeleeAttackRadius = 0.0f;

    public Transform RangedAttackPos;
    public float RangedAttackRadius = 0.0f;

    public GameObject projectile;
    public float ProjectileSpeed = 1.0f;
    public float ProjectileLifetime = 2.0f;

    HealthScript myhealthscript;
    ShittyAIScript movement;

    float rangedBasicCooldownTimer;
    float meleeBasicCooldownTimer;

    public LayerMask ProjectileLayerForParrying;
    bool isParrying;
    public float ParryDuration = 0.5f;
    public float ParryCooldown = 1.0f;
    float parryTimer;
    float parryCDTimer;

    // Start is called before the first frame update
    void Start()
    {
        myhealthscript = GetComponent<HealthScript>();
        movement = GetComponent<ShittyAIScript>();

        rangedBasicCooldownTimer = 0.0f;
        meleeBasicCooldownTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.gameState != GameState.Running)
            return;

        if (rangedBasicCooldownTimer > 0.0f)
            rangedBasicCooldownTimer -= Time.deltaTime;
        if (meleeBasicCooldownTimer > 0.0f)
            meleeBasicCooldownTimer -= Time.deltaTime;

        if (rangedBasicCooldownTimer < 0.0f)
            rangedBasicCooldownTimer = 0.0f;
        if (meleeBasicCooldownTimer < 0.0f)
            meleeBasicCooldownTimer = 0.0f;

        if (CompareTag("Player"))
        {
            if (Input.GetMouseButtonDown(1)) // RMB
            {
                EnableMelee = !EnableMelee;
                EnableRanged = !EnableRanged;
            }

            if (Input.GetMouseButtonDown(0)) // LMB
            {
                if (EnableMelee)
                    DoParry();
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
                if ((EnableRanged && rangedBasicCooldownTimer == 0.0f) ||
                    (EnableMelee && meleeBasicCooldownTimer == 0.0f))
                {
                    Collider2D[] enemiesToDamage;

                    if (EnableMelee && movement.isAttacking)
                    {
                        enemiesToDamage = Physics2D.OverlapCircleAll(MeleeAttackPos.position, MeleeAttackRadius, EnemyMask);

                        if (enemiesToDamage.Length > 0)
                        {
                            foreach (Collider2D player in enemiesToDamage)
                            {
                                player.GetComponent<HealthScript>().TakeDamage(RangedDamage);
                                animator.SetTrigger("IsAttacking");
                            }
                        }
                    }
                    if (EnableRanged && movement.isAttacking)
                    {
                        enemiesToDamage = Physics2D.OverlapCircleAll(RangedAttackPos.position, RangedAttackRadius, EnemyMask);

                        if (CompareTag("Follower"))
                        {
                            foreach (Collider2D enemy in enemiesToDamage)
                            {
                                if (enemy.CompareTag("AI"))
                                {
                                    player = enemy.gameObject;
                                    break;
                                }
                            }
                        }

                        Vector3 endPos = player.transform.position;
                        Vector3 velocity = endPos - RangedAttackPos.position;
                        velocity.z = 0.0f; // So that normalize will ignore the magnitude of z

                        if (!movement.isFacingRight)
                        {
                            GameObject p;
                            p = Instantiate(projectile, RangedAttackPos.position, transform.rotation);

                            velocity.Normalize();

                            p.GetComponent<Projectile>().SetParams(RangedDamage, ProjectileLifetime, EnemyMask);

                            p.GetComponent<Rigidbody2D>().velocity = velocity * ProjectileSpeed;

                            if (movement.isFacingRight && velocity.x < 0)
                                p.GetComponent<Transform>().rotation = Quaternion.Euler(0, 180, 0);
                            if (!movement.isFacingRight && velocity.x > 0)
                                p.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, 0);

                            movement.isAttacking = true;
                        }
                    }

                    if (EnableRanged)
                        rangedBasicCooldownTimer = RangedBasicAttackCooldown;
                    if (EnableMelee)
                        meleeBasicCooldownTimer = MeleeBasicAttackCooldown;
                }
            }
        }

        if (CompareTag("Player"))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if ((EnableRanged && rangedBasicCooldownTimer == 0.0f) ||
                    (EnableMelee && meleeBasicCooldownTimer == 0.0f))
                {
                    if (EnableRanged)
                    {
                        Vector3 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        Vector3 velocity = endPos - RangedAttackPos.position;
                        velocity.z = 0.0f; // So that normalize will ignore the magnitude of z
                        velocity.Normalize();
                                                      
                        GameObject p;
                        p = Instantiate(projectile, RangedAttackPos.position, transform.rotation);
                        p.GetComponent<Projectile>().SetParams(RangedDamage, ProjectileLifetime, EnemyMask);

                        p.GetComponent<Rigidbody2D>().velocity = velocity * ProjectileSpeed;

                        PlayerScript playerscript = gameObject.GetComponent<PlayerScript>();
                        if (playerscript.isFacingRight && velocity.x < 0)
                            p.GetComponent<Transform>().rotation = Quaternion.Euler(0, 180, 0);
                        if (!playerscript.isFacingRight && velocity.x > 0)
                            p.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, 0);
                    }

                    if (EnableMelee)
                    {
                        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(MeleeAttackPos.position, MeleeAttackRadius, EnemyMask);

                        foreach (Collider2D enemy in enemiesToDamage)
                        {
                            enemy.GetComponent<HealthScript>().TakeDamage(MeleeDamage);
                        }
                    }

                    if (EnableRanged)
                        rangedBasicCooldownTimer = RangedBasicAttackCooldown;
                    if (EnableMelee)
                        meleeBasicCooldownTimer = MeleeBasicAttackCooldown;
                }
            }
        }
    }

    public void DoParry()
    {
        Collider2D[] projectilesToDestroy = Physics2D.OverlapCircleAll(MeleeAttackPos.position, MeleeAttackRadius, ProjectileLayerForParrying);
        
        foreach (Collider2D projectile in projectilesToDestroy)
        {
            print("Parried!");
            Destroy(projectile.gameObject);
        }
    }

    public float GetRangedBasicCooldownAsFraction()
    {
        return (rangedBasicCooldownTimer / RangedBasicAttackCooldown);
    }

    public float GetMeleeBasicCooldownAsFraction()
    {
        return (meleeBasicCooldownTimer / MeleeBasicAttackCooldown);
    }

    public float GetRangedBasicCooldownTimer()
    {
        return rangedBasicCooldownTimer;
    }

    public float GetMeleeBasicCooldownTimer()
    {
        return meleeBasicCooldownTimer;
    }

    
}
