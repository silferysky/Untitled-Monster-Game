﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Chip = ChipScript;
using Holder = ChipHolder;

public class ChipMenuScript : MonoBehaviour
{
	public List<Chip> AttachedChips = new List<Chip>();
	public List<GameObject> DisplayedChips = new List<GameObject>();
	public List<GameObject> ChipTemplate = new List<GameObject>();
    public GameObject Background;
	public int MaxChipSize;
	public int CurChipSize;
    bool MenuIsOpen = false;

    public bool LootMenuIsOpen = false;
    public GameObject LootBackground;
    public List<GameObject> DisplayedLootChips = new List<GameObject>();
    public List<Chip> LootChips = new List<Chip>();
    public GameObject LootChipTemplate;
    public GameObject LastDeadboi;

    List<Chip> ChipLibraryStats = new List<Chip>();
    List<Chip> ChipLibraryUI = new List<Chip>();
    List<Chip> ChipLibraryAttacks = new List<Chip>();

    int SelectedChipInventory = -1;
    int SelectedChipLoot = -1;

    public List<Button> Buttons = new List<Button>();

    // Start is called before the first frame update
    void Start()
    {
        CurChipSize = 0;

        GenerateChipLibrary();

        Buttons[0].onClick.AddListener(ToggleMenu);

        //Disable this once chips are in place
        //TestSampleChips();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ToggleMenu();
        }


        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    mouseWorldPos.z = 0.0f;
        //    Ray ray = Camera.main.ScreenPointToRay(mouseWorldPos);
        //    Debug.Log(mouseWorldPos);
        //    RaycastHit hit;

        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        Debug.Log(hit.transform.name);
        //        if (hit.transform.tag == "LootChip")
        //        {
        //            SelectedChipInventory = hit.transform.gameObject.GetComponent<Chip>();
        //        }
        //        else if (hit.transform.tag == "Chip")
        //        {

        //        }
        //        else if (hit.transform.tag == "Button")
        //        {
        //            Debug.Log("BUTTON");
        //            if (hit.transform.name == "Accept")
        //            {
        //                Debug.Log("ACCEPT");
        //                CloseMenu();
        //            }
        //            else if (hit.transform.name == "Destroy")
        //            {
        //                Debug.Log("DESTROY");
        //                if (SelectedChipInventory != null)
        //                {
        //                    Destroy(SelectedChipInventory);
        //                    SortChips();
        //                    DisplayChips();
        //                }
        //            }
        //        }
        //    }
        //}
    }

    public void OpenMenu()
    {
        MenuIsOpen = true;
        Background.SetActive(true);
    }

    public void CloseMenu()
    {
        MenuIsOpen = false;
        Background.SetActive(false);
        SelectedChipInventory = -1;
        SelectedChipLoot = -1;
    }

    public void ToggleMenu()
    {
        if (!MenuIsOpen)
        {
            GameStateManager.gameState = GameState.ChipMenu;
            OpenMenu();
        }
        else
        {
            GameStateManager.gameState = GameState.Running;
            CloseMenu();
            CloseLootInventory();
        }
    }

    public void OpenLootInventory()
    {
        LootMenuIsOpen = true;
        LootBackground.SetActive(true);
    }

    public void CloseLootInventory()
    {
        LootMenuIsOpen = false;
        LootBackground.SetActive(false);
    }

    void HandleInventoryChip(int selectedChip)
    {
        if (LootMenuIsOpen)
        {
            //Transfer to Loot Menu if selected twice
            if (SelectedChipInventory == selectedChip)
            {
            }
        }
        else
        {
            //Just Select
            SelectedChipInventory = selectedChip;
        }
    }

    void HandleLootChip(GameObject instance)
    {
        Chip chip = instance.GetComponent<Chip>();
        int selectedChip = chip.ChipPosition;
        Debug.Log(selectedChip);
        AddChip(LootChips[selectedChip]);
        LootChips.RemoveAt(selectedChip);
        DisplayLoot(LastDeadboi);
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

        Vector3 curPos = Background.GetComponent<RectTransform>().position + new Vector3(0.0f, + Background.GetComponent<RectTransform>().rect.height * 0.45f, -1.0f);

        int loop = 0;
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
                    toInstantiate.GetComponent<Image>().color = new Color(0.55f, 0.21f, 0.25f);
                    break;
                case 1:
                    toInstantiate.GetComponent<Image>().color = new Color(0.45f, 0.41f, 0.27f);
                    break;
                case 2:
                    toInstantiate.GetComponent<Image>().color = new Color(0.5f, 0.55f, 0.47f);
                    break;
                default:
                    toInstantiate.GetComponent<Image>().color = Color.white;
                    break;
            }

            GameObject instance = Instantiate(toInstantiate, curPos + new Vector3(0.0f, toInstantiate.GetComponent<RectTransform>().rect.height / -2, 0.0f), Quaternion.identity);
            instance.transform.GetChild(0).GetComponent<Text>().text = c.ChipName;
            instance.transform.SetParent(Background.transform);
            instance.tag = "Chip";
            instance.GetComponent<Button>().onClick.AddListener(() => HandleInventoryChip(loop));
            DisplayedChips.Add(instance);

            //This is to offset Chip Size
            curPos += new Vector3(0.0f, toInstantiate.GetComponent<RectTransform>().rect.height * -1.2f, 0.0f);

            ++loop;
        }
	}

    void SortChips()
    {
        AttachedChips.Sort(delegate(Chip lhs, Chip rhs)
        {
            if (lhs.ChipPoints == rhs.ChipPoints) return 0;
            else if (lhs.ChipPoints < rhs.ChipPoints) return -1;
            else return 1;
        });
		//AttachedChips.Reverse();

        //string chipString = "";
        //foreach (Chip c in AttachedChips)
        //{
        //    chipString += c.ChipPoints + "|";
        //}
        //Debug.Log(chipString);
    }

    public void AddChip(Chip newChip)
    {
        if (CurChipSize + newChip.ChipSize > MaxChipSize)
            return;

        AttachedChips.Add(newChip);
        SortChips();
        DisplayChips();
    }

    void GenerateChipLibrary()
    {
        //STATS CHIPS
        ChipLibraryStats.Add(new Chip(0, 1, 1, 1, "ATK UP"));
        ChipLibraryStats.Add(new Chip(0, 2, 2, 1, "ATK UP+"));
        ChipLibraryStats.Add(new Chip(1, 1, 1, 1, "DEF UP"));
        ChipLibraryStats.Add(new Chip(1, 2, 2, 1, "DEF UP+"));
        ChipLibraryStats.Add(new Chip(2, 1, 1, 1, "SPD UP"));
        ChipLibraryStats.Add(new Chip(2, 2, 2, 1, "SPD UP+"));
        ChipLibraryStats.Add(new Chip(3, 2, 2, 1, "ATK SLOT"));

        //UI CHIPS
        ChipLibraryUI.Add(new Chip(0, 1, 1, 2, "SELF HP BAR"));
        ChipLibraryUI.Add(new Chip(1, 1, 2, 2, "ENEMY HP BAR"));
        ChipLibraryUI.Add(new Chip(2, 1, 2, 2, "COOLDOWN BAR"));

        //ATTACK CHIPS
        ChipLibraryAttacks.Add(new Chip(0, 1, 1, 0, "STRIKE"));
        ChipLibraryAttacks.Add(new Chip(1, 1, 1, 0, "SHOOT"));
    }

    public void GenerateLoot(GameObject holder)
    {
        Holder chipHolder = holder.GetComponent<Holder>();
        chipHolder.ClearChips();

        //Easiest to get stats chips, medium to get attack chips, hard to get UI chips
        int StatsChipWeightage = ChipLibraryStats.Count * 3;
        int AttackChipsWeightage = ChipLibraryAttacks.Count * 2;
        int UIChipWeightage = ChipLibraryUI.Count * 1;
        int MaxWeightage = StatsChipWeightage + AttackChipsWeightage + UIChipWeightage;
        int NumberOfChips = Random.Range(8, 10);

        for (int i = 0; i < NumberOfChips; ++i)
        {
            int ID = 0;
            int ChipWeightage = Random.Range(0, MaxWeightage + 1);
            if (ChipWeightage >= StatsChipWeightage)
            {
                ChipWeightage -= StatsChipWeightage;
                ++ID;

                if (ChipWeightage >= UIChipWeightage)
                {
                    ChipWeightage -= UIChipWeightage;
                    ++ID;
                }
            }

            Chip newChip = new Chip();
            switch (ID)
            {
                //Stats Chip
                case 0:
                    newChip.CopyChip(ChipLibraryStats[ChipWeightage / 3]);
                    break;
                case 1:
                    newChip.CopyChip(ChipLibraryUI[ChipWeightage / 1]);
                    break;
                case 2:
                    newChip.CopyChip(ChipLibraryAttacks[ChipWeightage / 2]);
                    break;
                default:
                    break;
            }
            chipHolder.AddChip(newChip);
        }

        //chipHolder.AddChip(ChipLibraryStats[0]);
        //chipHolder.AddChip(ChipLibraryStats[1]);
        LootChips = chipHolder.Chips;
    }

    public void DisplayLoot(GameObject holder)
    {
        foreach (GameObject go in DisplayedLootChips)
        {
            Destroy(go);
        }
        DisplayedLootChips.Clear();

        Holder chipHolder = holder.GetComponent<Holder>();
        int loop = 0;
        foreach (Chip c in LootChips)//chipHolder.Chips)
        {
            GameObject toInstantiate = LootChipTemplate;
            switch (c.ChipType)
            {
                case 0:
                    toInstantiate.GetComponent<Image>().color = new Color(0.55f, 0.21f, 0.25f);
                    break;
                case 1:
                    toInstantiate.GetComponent<Image>().color = new Color(0.45f, 0.41f, 0.27f);
                    break;
                case 2:
                    toInstantiate.GetComponent<Image>().color = new Color(0.5f, 0.55f, 0.47f);
                    break;
                default:
                    toInstantiate.GetComponent<Image>().color = Color.white;
                    break;
            }

            Vector3 curPos = LootBackground.GetComponent<RectTransform>().position + new Vector3(65 * (loop % 3 - 1), -65 * (loop / 3 - 1), -1.0f);
            GameObject instance = Instantiate(toInstantiate, curPos, Quaternion.identity);
            instance.transform.GetChild(0).GetComponent<Text>().text = c.ChipName;
            instance.transform.SetParent(LootBackground.transform);
            instance.tag = "LootChip";
            instance.GetComponent<Button>().onClick.AddListener(() => HandleLootChip(instance));
            DisplayedLootChips.Add(instance);

            ++loop;
        }
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
