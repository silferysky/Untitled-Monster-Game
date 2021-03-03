using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    Color origColor;
    float fadeoutDuration;
    float fadeoutTimer;
       

    public void SetParams(float _fadeoutDuration, Color _origCol)
    {
        fadeoutDuration = _fadeoutDuration;
        origColor = _origCol;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeoutTimer < fadeoutDuration)// Fade out lighting
        {

            Color color = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, Mathf.Lerp(origColor.a, 0.0f, fadeoutTimer / fadeoutDuration));

            fadeoutTimer += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
