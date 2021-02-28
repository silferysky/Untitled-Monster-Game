using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chip = ChipScript;

public class ChipHolder : MonoBehaviour
{
    public List<Chip> Chips = new List<Chip>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AddChip(Chip newChip)
    {
        Chips.Add(newChip);
    }

    public void ClearChips()
    {
        Chips.Clear();
    }

    public void RemoveChip(int pos)
    {
        Chips.RemoveAt(pos);
    }
}
