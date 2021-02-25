using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamageScript : MonoBehaviour
{
    public bool EnableRanged = false;
    public int RangedDamage = 1;

    public bool EnableMelee = false;
    public int MeleeDamage = 1;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (tag == "AI")
        {
            if (other.tag == "Player")
            {
                HealthScript hs = other.transform.gameObject.GetComponent<HealthScript>();

                if (hs)
                    hs.TakeDamage(MeleeDamage);
            }
        }

        if (tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                if (other.tag == "AI")
                {
                    HealthScript hs = other.transform.gameObject.GetComponent<HealthScript>();

                    if (hs)
                    {
                        hs.TakeDamage(MeleeDamage);
                    }
                }
            }
        }
    }
}
