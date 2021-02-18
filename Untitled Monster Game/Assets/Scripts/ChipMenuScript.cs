using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chip = ChipScript;

public class ChipMenuScript : MonoBehaviour
{
	public List<Chip> AttachedChips;
	public List<GameObject> DisplayedChips;
	public List<GameObject> ChipDisplays;
	public int MaxChipSize;
	public int CurChipSize;
	
    // Start is called before the first frame update
    void Start()
    {
        CurChipSize = 0;
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
		
		foreach (Chip c in AttachedChips)
		{
			//Instantiate()
		}
	}
}
