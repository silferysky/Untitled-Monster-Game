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
    }

    public void UpdateStatUI()
    {
        for (int i = 0; i < 5; ++i)
        {
            if (value - 5 > i)
                bars[i].GetComponent<Image>().color = Color.red;
            else if (value > i)
                bars[i].GetComponent<Image>().color = Color.green;
            else
                bars[i].GetComponent<Image>().color = Color.white;
        }
    }

    public void UpdateStatUI(int newValue)
    {
        value = newValue;
        UpdateStatUI();
    }
}
