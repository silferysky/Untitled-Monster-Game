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

    public Animator animator;
    public GameObject projectile;
    public float ProjectileSpeed = 1.0f;
    public float ProjectileLifetime = 2.0f;

    public GameObject player;

    HealthScript myhealthscript;

    ShittyAIScript movement;

    // Start is called before the first frame update
    void Start()
    {
        myhealthscript = GetComponent<HealthScript>();
        movement = GetComponent<ShittyAIScript>();
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
                                animator.SetTrigger("IsAttacking");
                            }
                        }
                    }
                    if (EnableRanged)
                    {
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
                            animator.SetTrigger("IsAttacking");
                        }

                        /* enemiesToDamage = Physics2D.OverlapCircleAll(RangedAttackPos.position, RangedAttackRadius, EnemyMask);

                        if (enemiesToDamage.Length > 0)
                        {
                            foreach (Collider2D player in enemiesToDamage)
                            {
                                player.GetComponent<HealthScript>().TakeDamage(RangedDamage);
                            }
                        } */
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

                        /* PlayerScript playerscript = gameObject.GetComponent<PlayerScript>();
                        if (playerscript.isFacingRight && velocity.x < 0)
                            p.GetComponent<Transform>().rotation = Quaternion.Euler(0, 180, 0);
                        if (!playerscript.isFacingRight && velocity.x > 0)
                            p.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, 0); */
                    }

                    if (EnableMelee)
                    {
                        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(MeleeAttackPos.position, MeleeAttackRadius, EnemyMask);

                        foreach (Collider2D enemy in enemiesToDamage)
                        {
                            enemy.GetComponent<HealthScript>().TakeDamage(MeleeDamage);
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
