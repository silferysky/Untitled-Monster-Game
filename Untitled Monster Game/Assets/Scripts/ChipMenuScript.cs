using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chip = ChipScript;

public class ChipMenuScript : MonoBehaviour
{
	public List<Chip> AttachedChips = new List<Chip>();
	public List<GameObject> DisplayedChips = new List<GameObject>();
	public List<GameObject> ChipTemplate = new List<GameObject>();
    public GameObject Background;
	public int MaxChipSize;
	public int CurChipSize;
	
    // Start is called before the first frame update
    void Start()
    {
        CurChipSize = 0;
        AttachedChips.Add(new Chip(0, 1, 1, 0, "First"));
        AttachedChips.Add(new Chip(0, 1, 2, 1, "Second"));
        AttachedChips.Add(new Chip(0, 1, 3, 2, "Third"));
        AttachedChips.Add(new Chip(0, 1, 2, 0, "Fourth"));
        AttachedChips.Add(new Chip(0, 1, 1, 0, "Fifth"));


        foreach (Chip c in AttachedChips)
        {
            c.EvaluateChip();
        }
        
        SortChips();
        DisplayChips();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	bool CheckChipValidity()
	{
		int Total = 0;
		foreach (Chip c in AttachedChips)
		{
			Total += c.ChipSize;
		}
		
		if (Total > MaxChipSize)
			return false;
		
		return true;
	}
	
	void DisplayChips()
	{
		foreach (GameObject go in DisplayedChips)
		{
			Destroy(go);
		}
		DisplayedChips.Clear();

        Vector3 curPos = transform.position + new Vector3(0.0f, -0.2f + Background.GetComponent<SpriteRenderer>().bounds.size.y / 2, -1.0f);

		foreach (Chip c in AttachedChips)
		{
            //Instantiate
            GameObject toInstantiate;
            if (c.ChipSize == 1)
            {
                toInstantiate = ChipTemplate[0];
            }
            else if (c.ChipSize == 2)
            {
                toInstantiate = ChipTemplate[1];
            }
            else //if (c.ChipSize == 3)
            {
                toInstantiate = ChipTemplate[2];
            }

            switch (c.ChipType)
            {
                case 0:
                    toInstantiate.GetComponent<SpriteRenderer>().color = new Color(0.55f, 0.21f, 0.25f);
                    break;
                case 1:
                    toInstantiate.GetComponent<SpriteRenderer>().color = new Color(0.45f, 0.41f, 0.27f);
                    break;
                case 2:
                    toInstantiate.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.55f, 0.47f);
                    break;
                default:
                    toInstantiate.GetComponent<SpriteRenderer>().color = Color.white;
                    break;
            }

            //This is to offset Chip Size
            DisplayedChips.Add(Instantiate(toInstantiate, curPos + new Vector3(0.0f, toInstantiate.GetComponent<SpriteRenderer>().bounds.size.y / -2, 0.0f), Quaternion.identity));

            curPos += new Vector3(0.0f, -0.1f - toInstantiate.GetComponent<SpriteRenderer>().bounds.size.y, 0.0f);
        }
	}

    void SortChips()
    {
        AttachedChips.Sort();
		AttachedChips.Reverse();
        //List<Chip> TempHolder = new List<Chip>();
        //List<Chip> TempChips = new List<Chip>();

        //for (int i = 0; i < AttachedChips.Count; ++i)
        //{
        //    Chip c = new Chip();
        //    c.CopyChip(AttachedChips[i]);
        //    TempChips.Add(c);
        //}

        //Chip BestChip = new Chip();
        //int ID = 0;
        //int HighestPoints = 0;

        ////Loop X number of chips times
        //for (int i = 0; i < AttachedChips.Count; ++i)
        //{
        //    //Loop through to find best chip
        //    for (int j = 0; j < TempChips.Count; ++j)
        //    {
        //        Chip c = new Chip();
        //        c.CopyChip(TempChips[j]);
        //        if (c.ChipPoints > HighestPoints)
        //        {
        //            HighestPoints = c.ChipPoints;
        //            BestChip.CopyChip(c);
        //            ID = j;
        //        }
        //    }
        //    TempHolder.Add(BestChip);
        //    TempChips.RemoveAt(ID);
        //    BestChip = new Chip();
        //}

        //AttachedChips = TempHolder;
    }

}
