using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePopUp : MonoBehaviour
{
    public Text TextObj;
    public Vector2 Force = new Vector2(0.0f, 2.0f);
    int damage;

    // Start is called before the first frame update
    void Start()
    {
        TextObj.text = damage.ToString();
        GetComponent<Rigidbody2D>().AddForce(Force, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetParams(int _damage)
    {
        damage = _damage;
    }
}
