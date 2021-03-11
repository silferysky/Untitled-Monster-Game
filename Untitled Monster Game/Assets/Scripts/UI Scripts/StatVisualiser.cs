using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatVisualiser : MonoBehaviour
{
    public GameObject[] bars;
    public int value = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < value; ++i)
        {
            bars[i].GetComponent<Image>().color = Color.green;
        }
    }
}
