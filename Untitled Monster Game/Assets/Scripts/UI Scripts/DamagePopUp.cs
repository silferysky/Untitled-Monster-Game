using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePopUp : MonoBehaviour
{
    public Text TextObj;
    public Vector2 Force = new Vector2(0.0f, 2.0f);
    public float Lifetime = 1.0f;
    public float FadeDuration = 1.0f;

    int damage;
    Color origColor;
    float fadeTimer;
    float lifeTimer;

    // Start is called before the first frame update
    void Start()
    {
        TextObj.text = damage.ToString();
        GetComponent<Rigidbody2D>().AddForce(Force, ForceMode2D.Impulse);
        origColor = TextObj.color;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer += Time.deltaTime;
        fadeTimer += Time.deltaTime;

        if (lifeTimer >= Lifetime)
        {
            Destroy(this);
            return;
        }

        TextObj.color = new Color(origColor.r, origColor.b, origColor.g, Mathf.Lerp(origColor.a, 0.0f, fadeTimer / FadeDuration));
    }

    public void SetParams(int _damage)
    {
        damage = _damage;
    }
}
