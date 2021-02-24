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
    bool MenuIsOpen = false;
	
    // Start is called before the first frame update
    void Start()
    {
        CurChipSize = 0;

        //Disable this once chips are in place
        TestSampleChips();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (MenuIsOpen)
                OpenMenu();
            else
                CloseMenu();

            MenuIsOpen = !MenuIsOpen;
        }
    }

    public void OpenMenu()
    {
        Background.SetActive(true);
    }

    public void CloseMenu()
    {
        Background.SetActive(false);
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

            GameObject instance = Instantiate(toInstantiate, curPos + new Vector3(0.0f, toInstantiate.GetComponent<SpriteRenderer>().bounds.size.y / -2, 0.0f), Quaternion.identity);
            instance.transform.SetParent(Background.transform);
            DisplayedChips.Add(instance);

            //This is to offset Chip Size
            curPos += new Vector3(0.0f, -0.1f - toInstantiate.GetComponent<SpriteRenderer>().bounds.size.y, 0.0f);
        }
	}

    void SortChips()
    {
        AttachedChips.Sort();
		AttachedChips.Reverse();
    }

    public void AddChip(Chip newChip)
    {
        AttachedChips.Add(newChip);
        SortChips();
        DisplayChips();
    }

    void TestSampleChips()
    {
        AttachedChips.Clear();

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

}
