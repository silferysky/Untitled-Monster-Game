using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float timer;

    public int damage;
    public LayerMask enemyMask;

    public float lifetime;

    public void SetParams(int _damage, float _lifetime, LayerMask _mask)
    {
        damage = _damage;
        enemyMask = _mask;
        lifetime = _lifetime;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= lifetime)
        {
            Destroy(this.gameObject);
        }

        Collider2D enemyToDamage = Physics2D.OverlapCircle(transform.position, transform.localScale.x * 0.5f, enemyMask);

        if (enemyToDamage)
        {
            enemyToDamage.GetComponent<HealthScript>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
