using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectUIScript : MonoBehaviour
{
    public GameObject Player;
    DoDamageScript dmgScript;

    public Image MeleeIcon;
    public Image RangedIcon;

    Vector3 inactiveScale = new Vector3(0.75f, 0.75f, 1.0f);
    Vector3 activeScale = new Vector3(1.5f, 1.5f, 1.0f);

    // Start is called before the first frame update
    void Start()
    {
        dmgScript = Player.GetComponent<DoDamageScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dmgScript.EnableMelee)
        {
            MeleeIcon.transform.localScale = activeScale;
            RangedIcon.transform.localScale = inactiveScale;
        }

        if (dmgScript.EnableRanged)
        {
            MeleeIcon.transform.localScale = inactiveScale;
            RangedIcon.transform.localScale = activeScale;
        }
    }
}
