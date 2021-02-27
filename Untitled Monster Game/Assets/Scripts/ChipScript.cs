using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipScript : MonoBehaviour, IComparable<ChipScript>
{
	public int ChipID;
	public int ChipLevel;
	public int ChipSize;
	public int ChipType; //0 = Atk, 1 = Stat, 2 = UI
	public string ChipName;
    public int ChipPoints; //Used for ChipMenu calculations

    public ChipScript()
    {
        ChipID = 0;
        ChipLevel = 0;
        ChipSize = 0;
        ChipType = 0;
        ChipName = "DEFAULT";
        ChipPoints = -1;
    }

    public ChipScript(int id, int level, int size, int type, string name)
    {
        ChipID = id;
        ChipLevel = level;
        ChipSize = size;
        ChipType = type;
        ChipName = name;

        EvaluateChip();
    }

    public void CopyChip(ChipScript rhs)
    {
        ChipID = rhs.ChipID;
        ChipLevel = rhs.ChipLevel;
        ChipSize = rhs.ChipSize;
        ChipType = rhs.ChipType;
        ChipName = rhs.ChipName;
        ChipPoints = rhs.ChipPoints;
    }

    public void EvaluateChip()
    {
        //ChipType > ChipSize > ChipLevel
        ChipPoints = 100 * (3 - ChipType) + 10 * (ChipSize) + 1 * (ChipLevel);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public int CompareTo(ChipScript rhs)
    {
        if (rhs == null)
        {
            return 1;
        }
        else
        {
            return this.ChipPoints.CompareTo(rhs.ChipPoints);
        }
    }
}
