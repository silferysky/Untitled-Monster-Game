using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public Animator animator;
    public GameObject DamagePopUpPrefab;

    public int HP_Max = 10;
    public int HP_Current;
    bool isAlive;

    public bool IsLooted = false;

    // Ignore damage variables
    bool ignoreDamage = false;
    float ignoreDamageTimer;

    // Flicker function variables
    SpriteRenderer spriteRenderer;
    Color originalColour;
    Color newColour = new Vector4(0.0f, 1.0f, 0.0f, 1.0f); // rgba
    int numFlickers = 2; // How many times to switch to the new colour
    float originalColDuration = 0.3f;
    float newColDuration = 0.3f;
    int flickerCount;
    bool isFlickering;
    float flickerTimer;

    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
        HP_Current = HP_Max;

        spriteRenderer = GetComponent<SpriteRenderer>();
        isFlickering = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlickering)
            FlickerColour();

        if (CompareTag("Player"))
        {
            // Cheats
            if (Input.GetKeyDown(KeyCode.H))
                HealCheat(5);
            if (Input.GetKeyDown(KeyCode.J))
                DamageSelfCheat(5);
        }

        if (ignoreDamage)
        {
            if (ignoreDamageTimer > 0.0f)
            {
                ignoreDamageTimer -= Time.deltaTime;
            }
            else
            {
                ignoreDamageTimer = 0.0f;
                ignoreDamage = false;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isAlive)
            return;

        if (ignoreDamage)
        {
            if (damage < 0) // Allow heals
                HP_Current -= damage;
        }
        else // Carry on as normal
            HP_Current -= damage;

        if (HP_Current < 0)
            HP_Current = 0;
        if (HP_Current > HP_Max)
            HP_Current = HP_Max;

        if (HP_Current == 0)
        {
            isAlive = false;

            if (CompareTag("Player"))
                print("YOU DIED!");

            GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f);
            GetComponent<BoxCollider2D>().isTrigger = true;
            GetComponent<Rigidbody2D>().velocity = new Vector3();
            GetComponent<Rigidbody2D>().gravityScale = 0.0f;

            animator.SetTrigger("IsDead");
        }
        else
            isAlive = true;

        if (damage > 0)
            InstantiateDamagePopup(damage);
    }

    public bool GetAlive()
    {
        return isAlive;
    }

    public float GetHPFraction()
    {
        return ((float)HP_Current / (float)HP_Max);
    }

    public void HealCheat(int amount)
    {
        if (spriteRenderer)
        {
            originalColour = spriteRenderer.color;
            flickerCount = 0;
            isFlickering = true;
            flickerTimer = 0.0f;
        }

        TakeDamage(-amount);
    }

    public void DamageSelfCheat(int damage)
    {
        TakeDamage(damage);
    }

    void FlickerColour()
    {
        flickerTimer += Time.deltaTime;

        if (flickerCount == 0)
        {
            spriteRenderer.color = newColour;
            flickerTimer = 0.0f;
            ++flickerCount;
        }

        if (flickerCount <= numFlickers)
        {
            if (spriteRenderer.color == originalColour)
            {
                if (flickerTimer > originalColDuration)
                {
                    spriteRenderer.color = newColour;
                    flickerTimer = 0.0f;
                    ++flickerCount;
                }
            }
            else if (spriteRenderer.color == newColour)
            {
                if (flickerTimer > newColDuration)
                {
                    spriteRenderer.color = originalColour;
                    flickerTimer = 0.0f;
                }
            }
        }
        else // Revert back to original colour
        {
            spriteRenderer.color = originalColour;
            isFlickering = false;
        }
    }

    public void IgnoreDamage(float duration)
    {
        ignoreDamage = true;
        ignoreDamageTimer = duration;
    }

    void InstantiateDamagePopup(int damage)
    {
        GameObject popup;
        Quaternion dummy = new Quaternion();
        dummy.eulerAngles = new Vector3(0, 0, 0);
        Vector2 position = new Vector2(transform.position.x, transform.position.y + 2.0f);
        popup = Instantiate(DamagePopUpPrefab, position, dummy);
        popup.GetComponent<DamagePopUp>().SetParams(damage);
    }
}
