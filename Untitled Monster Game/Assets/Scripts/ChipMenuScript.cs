using System.Collections;
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

    public GameObject StatsBackground;

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
    public GameObject HUDGameObject;
    public GameObject PlayerStatsObject;

    public GameObject AbilityScripts;

    public GameObject MAtkSpdVisualiser;
    public GameObject RAtkSpdVisualiser;

    public GameObject MAtkVisualiser;
    public GameObject RAtkVisualiser;

    // Start is called before the first frame update
    void Start()
    {
        CurChipSize = 0;

        Vector3 startPos = new Vector3(Screen.width * -0.25f, 0.0f, 0.0f);
        Background.GetComponent<RectTransform>().anchoredPosition = startPos;
        Background.GetComponent<RectTransform>().sizeDelta = new Vector3(Screen.width * 300 / 1920, Screen.height * 850 / 1080, 1.0f);

        LootBackground.GetComponent<RectTransform>().anchoredPosition = new Vector3();
        LootBackground.GetComponent<RectTransform>().sizeDelta = new Vector3(Screen.width * 400 / 1920, Screen.height * 400 / 1080, 1.0f);

        StatsBackground.GetComponent<RectTransform>().anchoredPosition = new Vector3(Screen.width * 0.25f, 0.0f, 0.0f);
        StatsBackground.GetComponent<RectTransform>().sizeDelta = new Vector3(Screen.width * 320 / 1920, Screen.height * 440 / 1080, 1.0f);
        foreach (Transform child in StatsBackground.transform)
        {
            child.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector3(Screen.width * 160 / 1920, Screen.height * 30 / 1080, 1.0f);
            child.GetComponent<Text>().fontSize = Screen.width * 18 / 1920;
        }

        //Accept Btn
        Buttons[0].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(startPos.x + Screen.width * 480 / 1920, startPos.y + Screen.height * -310 / 1080, 1.0f);
        Buttons[0].gameObject.GetComponent<RectTransform>().sizeDelta = new Vector3(Screen.width * 250 / 1920, Screen.height * 50 / 1080, 1.0f);
        Buttons[0].gameObject.transform.GetChild(0).GetComponent<Text>().fontSize = Screen.width * 24 / 1920;

        //Destroy Btn
        Buttons[1].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(startPos.x + Screen.width * 480 / 1920, startPos.y + Screen.height * -370 / 1080, 1.0f);
        Buttons[1].gameObject.GetComponent<RectTransform>().sizeDelta = new Vector3(Screen.width * 250 / 1920, Screen.height * 50 / 1080, 1.0f);
        Buttons[1].gameObject.transform.GetChild(0).GetComponent<Text>().fontSize = Screen.width * 24 / 1920;

        GenerateChipLibrary();
        CreateDefaultChips();

        Buttons[0].onClick.AddListener(ToggleMenu);
        Buttons[1].onClick.AddListener(DestroyChip);

        UpdateAllStatVisualiser();
        //Disable this once chips are in place
        //TestSampleChips();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
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
        StatsBackground.SetActive(true);
    }

    public void CloseMenu()
    {
        MenuIsOpen = false;
        Background.SetActive(false);
        StatsBackground.SetActive(false);
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
        //Debug.Log(selectedChip);
        if (LootMenuIsOpen)
        {
            //Transfer to Loot Menu if selected twice
            if (SelectedChipInventory == selectedChip)
            {
                Chip newChip = new Chip();
                newChip.CopyChip(AttachedChips[selectedChip]);

                CurChipSize -= newChip.ChipSize;
                LastDeadboi.GetComponent<ChipHolder>().Chips.Add(newChip);
                AttachedChips.RemoveAt(selectedChip);

                if (newChip.ChipType == Chip.CType.STAT)
                {
                    UpdateStatsChips();
                }
                else if (newChip.ChipType == Chip.CType.UI)
                {
                    UpdateUIChips();
                }

                DisplayChips();
                DisplayLoot(LastDeadboi);
            }
            else
            {
                SelectedChipInventory = selectedChip;
            }
        }
        else
        {
            //Just Select
            SelectedChipInventory = selectedChip;
        }
    }

    void HandleLootChip(int selectedChip)
    {
        //Chip chip = LootChips[selectedChip];
        Chip chip = LastDeadboi.GetComponent<ChipHolder>().Chips[selectedChip];
        //Debug.Log(selectedChip);

        bool success = AddChip(LootChips[selectedChip]);

        if (!success)
            return;

        if (chip.ChipType == Chip.CType.UI)
        {
            UpdateUIChips();
        }
        else if (chip.ChipType == Chip.CType.STAT)
        {
            UpdateStatsChips();
        }
        //instance.GetComponent<ChipHolder>().Chips.RemoveAt(selectedChip);
        LastDeadboi.GetComponent<ChipHolder>().Chips.RemoveAt(selectedChip);
        //LootChips.RemoveAt(selectedChip);

        foreach(Chip c in LastDeadboi.GetComponent<ChipHolder>().Chips)
        {
            if (c.ChipPosition > selectedChip)
                --c.ChipPosition;
        }
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

        //Magic fucking numbers
        Vector3 curPos = Background.GetComponent<RectTransform>().position + new Vector3(0.0f, Background.GetComponent<RectTransform>().rect.height * 0.475f, -1.0f);
        float baseHeight = Background.GetComponent<RectTransform>().rect.height * 4 / 85;
        float baseWidth = Background.GetComponent<RectTransform>().rect.width * 14 /15;
        
        int loop = 0;
		foreach (Chip c in AttachedChips)
        {
            Vector2 chipSize = new Vector2();
            chipSize.x = baseWidth;
            chipSize.y = baseHeight;

            //float xDiff = chipSize.x * 0.525f;//* 3 / 5;
            //float yDiff = chipSize.y * 0.525f;//* 3 / 5;

            //Instantiate
            GameObject toInstantiate;
            if (c.ChipSize == 1)
            {
                toInstantiate = ChipTemplate[0];
            }
            else if (c.ChipSize == 2)
            {
                toInstantiate = ChipTemplate[1];
                chipSize.y *= 2;
            }
            else //if (c.ChipSize == 3)
            {
                toInstantiate = ChipTemplate[2];
                chipSize.y *= 3;
            }

            //Debug.Log("CHIP SIZE: " + chipSize.x + "|" + chipSize.y);
            //Add small offset based off current size
            curPos -= new Vector3(0.0f, chipSize.y / 2.0f, 0.0f);

            switch (c.ChipType)
            {
                case Chip.CType.ATK:
                    toInstantiate.GetComponent<Image>().color = new Color(0.55f, 0.21f, 0.25f);
                    break;
                case Chip.CType.UI:
                    toInstantiate.GetComponent<Image>().color = new Color(0.5f, 0.55f, 0.47f);
                    break;
                case Chip.CType.STAT:
                    toInstantiate.GetComponent<Image>().color = new Color(0.45f, 0.41f, 0.27f);
                    break;
                default:
                    toInstantiate.GetComponent<Image>().color = Color.white;
                    break;
            }

            int tempInt = loop; //This is a must, since otherwise the value will always be same (max value of loop)
            GameObject instance = Instantiate(toInstantiate, curPos + new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            instance.GetComponent<RectTransform>().sizeDelta = new Vector2(chipSize.x, chipSize.y);
            instance.transform.GetChild(0).GetComponent<Text>().text = c.ChipName;
            instance.transform.SetParent(Background.transform);
            instance.tag = "Chip";
            instance.GetComponent<Button>().onClick.AddListener(() => HandleInventoryChip(tempInt));
            instance.SetActive(true);
            DisplayedChips.Add(instance);

            //This is to offset Chip Size
            //curPos += new Vector3(0.0f, toInstantiate.GetComponent<RectTransform>().rect.height * -0.6f, 0.0f);
            curPos -= new Vector3(0.0f, chipSize.y * 0.5f + baseHeight * 0.2f, 0.0f);

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
		AttachedChips.Reverse();

        //string chipString = "";
        //foreach (Chip c in AttachedChips)
        //{
        //    chipString += c.ChipPoints + "|";
        //}
        //Debug.Log(chipString);
    }

    public bool AddChip(Chip newChip)
    {
        if (CurChipSize + newChip.ChipSize > MaxChipSize)
            return false;

        AttachedChips.Add(newChip);
        CurChipSize += newChip.ChipSize;
        SortChips();
        DisplayChips();

        return true;
    }

    void GenerateChipLibrary()
    {
        //ATTACK CHIPS
        ChipLibraryAttacks.Add(new Chip(4, 1, 1, Chip.CType.ATK, "HEAL"));
        ChipLibraryAttacks.Add(new Chip(5, 1, 1, Chip.CType.ATK, "SHIELD"));
        ChipLibraryAttacks.Add(new Chip(6, 1, 1, Chip.CType.ATK, "LIGHTNING"));

        //UI CHIPS
        ChipLibraryUI.Add(new Chip(0, 1, 1, Chip.CType.UI, "SELF STATUS"));
        ChipLibraryUI.Add(new Chip(1, 1, 1, Chip.CType.UI, "COOLDOWN STATUS"));
        ChipLibraryUI.Add(new Chip(2, 1, 1, Chip.CType.UI, "WEAPON MODE STATUS"));
        ChipLibraryUI.Add(new Chip(3, 1, 1, Chip.CType.UI, "DAMAGE INDICATOR"));

        //STATS CHIPS
        ChipLibraryStats.Add(new Chip(0, 1, 1, Chip.CType.STAT, "MELEE ATK UP"));
        ChipLibraryStats.Add(new Chip(0, 2, 2, Chip.CType.STAT, "MELEE ATK UP+"));
        ChipLibraryStats.Add(new Chip(1, 1, 1, Chip.CType.STAT, "MELEE SPD UP"));
        ChipLibraryStats.Add(new Chip(1, 2, 2, Chip.CType.STAT, "MELEE SPD UP+"));
        ChipLibraryStats.Add(new Chip(2, 1, 1, Chip.CType.STAT, "PROJ ATK UP"));
        ChipLibraryStats.Add(new Chip(2, 2, 2, Chip.CType.STAT, "PROJ ATK UP+"));
        ChipLibraryStats.Add(new Chip(3, 1, 1, Chip.CType.STAT, "PROJ SPD UP"));
        ChipLibraryStats.Add(new Chip(3, 2, 2, Chip.CType.STAT, "PROJ SPD UP+"));
    }

    public void GenerateLoot(GameObject holder)
    {
        Holder chipHolder = holder.GetComponent<Holder>();
        chipHolder.ClearChips();

        //Easiest to get stats chips, medium to get attack chips, hard to get UI chips
        int StatsChipWeightage = ChipLibraryStats.Count * 3;
        int AttackChipsWeightage = ChipLibraryAttacks.Count * 2; //For this demo set Attack Chip Weightage to 0
        int UIChipWeightage = ChipLibraryUI.Count * 1 * 0; //For this demo set UI Chip Weightage to 0
        int MaxWeightage = StatsChipWeightage + AttackChipsWeightage + UIChipWeightage;
        int NumberOfChips = Random.Range(1, 4);

        for (int i = 0; i < NumberOfChips; ++i)
        {
            int ID = 0;
            int ChipWeightage = Random.Range(0, MaxWeightage);
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

            //Debug.Log("CHIP WEIGHTAGE" + ChipWeightage);
            //Debug.Log("TYPE" + ID);
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

        float xDiff = LootBackground.GetComponent<RectTransform>().rect.width * 0.525f;//* 3 / 5;
        float yDiff = LootBackground.GetComponent<RectTransform>().rect.height * 0.525f;//* 3 / 5;

        Holder chipHolder = holder.GetComponent<Holder>();
        int loop = 0;
        foreach (Chip c in chipHolder.Chips /*LootChips*/)
        {
            GameObject toInstantiate = LootChipTemplate;
            switch (c.ChipType)
            {
                case Chip.CType.ATK:
                    toInstantiate.GetComponent<Image>().color = new Color(0.55f, 0.21f, 0.25f);
                    break;
                case Chip.CType.UI:
                    toInstantiate.GetComponent<Image>().color = new Color(0.5f, 0.55f, 0.47f);
                    break;
                case Chip.CType.STAT:
                    toInstantiate.GetComponent<Image>().color = new Color(0.45f, 0.41f, 0.27f);
                    break;
                default:
                    toInstantiate.GetComponent<Image>().color = Color.white;
                    break;
            }

            int tempInt = loop;
            toInstantiate.GetComponent<RectTransform>().sizeDelta = new Vector3(xDiff * 0.6f, yDiff * 0.6f);
            Vector3 curPos = LootBackground.GetComponent<RectTransform>().position - new Vector3(-xDiff * (loop % 3 - 1) * 0.6f, yDiff * (loop / 3 - 1) * 0.6f, -1.0f);
            GameObject instance = Instantiate(toInstantiate, curPos, Quaternion.identity);
            instance.transform.GetChild(0).GetComponent<Text>().text = c.ChipName;
            instance.transform.SetParent(LootBackground.transform);
            instance.tag = "LootChip";
            instance.GetComponent<Button>().onClick.AddListener(() => HandleLootChip(tempInt));
            instance.SetActive(true);
            DisplayedLootChips.Add(instance);

            ++loop;
        }
    }

    void UpdateUIChips()
    {
        Transform Canvas = HUDGameObject.transform.GetChild(0);
        int UINum = Canvas.transform.childCount;
        for (int i = 0; i < UINum; ++i)
        {
            Canvas.GetChild(i).gameObject.SetActive(false);
        }

        //Special case for damage popup
        Canvas.parent.GetChild(1).gameObject.SetActive(false);

        foreach (Chip c in AttachedChips)
        {
            if (c.ChipType != Chip.CType.UI)
                continue;

            //Refer to GenerateChipLibrary
            switch (c.ChipID)
            {
                case 0: //Status Bars
                    Canvas.GetChild(0).gameObject.SetActive(true);
                    Canvas.GetChild(1).gameObject.SetActive(true);
                    break;
                case 1: //Cooldowns
                    Canvas.GetChild(2).gameObject.SetActive(true);
                    Canvas.GetChild(3).gameObject.SetActive(true);
                    break;
                case 2: //Weapon Type
                    Canvas.GetChild(4).gameObject.SetActive(true);
                    Canvas.GetChild(5).gameObject.SetActive(true);
                    break;
                case 3: //Damage Popup
                    Canvas.parent.GetChild(1).gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }

    void UpdateStatsChips()
    {
        int BaseMeleeATK = 2, BaseRangedATK = 1;
        float BaseMeleeCD = 0.3f, BaseRangedCD = 0.3f;

        int MAtkValue = 1;
        int MAtkSpdValue = 1;
        int RAtkValue = 1;
        int RAtkSpdValue = 1;

        foreach (Chip c in AttachedChips)
        {
            if (c.ChipType != Chip.CType.STAT)
                continue;

            switch (c.ChipID)
            {
                case 0:
                    MAtkValue += c.ChipLevel;
                    break;
                case 1:
                    MAtkSpdValue += c.ChipLevel;
                    break;
                case 2:
                    RAtkValue += c.ChipLevel;
                    break;
                case 3:
                    RAtkSpdValue += c.ChipLevel;
                    break;
                case 4:
                    AbilityScripts.GetComponent<HealAbilityScript>().SetIsActive(true);
                    break;
                case 5:
                    AbilityScripts.GetComponent<ShieldAbilityScript>().SetIsActive(true);
                    break;
                case 6:
                    AbilityScripts.GetComponent<LightningAbility>().SetIsActive(true);
                    break;
                default:
                    break;
            }
        }

        BaseMeleeATK += (MAtkValue - 1);
        BaseMeleeCD -= 0.05f * (MAtkSpdValue - 1);
        BaseRangedATK += (RAtkValue - 1);
        BaseRangedCD -= 0.05f * (RAtkSpdValue - 1);

        DoDamageScript damageScript = PlayerStatsObject.GetComponent<DoDamageScript>();
        damageScript.MeleeDamage = BaseMeleeATK;
        damageScript.MeleeBasicAttackCooldown = BaseMeleeCD;
        damageScript.RangedDamage = BaseRangedATK;
        damageScript.RangedBasicAttackCooldown = BaseRangedCD;

        MAtkSpdVisualiser.GetComponent<StatVisualiser>().UpdateStatUI(MAtkSpdValue);
        RAtkSpdVisualiser.GetComponent<StatVisualiser>().UpdateStatUI(RAtkSpdValue);
        MAtkVisualiser.GetComponent<StatVisualiser>().UpdateStatUI(MAtkValue);
        RAtkVisualiser.GetComponent<StatVisualiser>().UpdateStatUI(RAtkValue);
    }

    void UpdateAllStatVisualiser()
    {
        MAtkSpdVisualiser.GetComponent<StatVisualiser>().UpdateStatUI();
        RAtkSpdVisualiser.GetComponent<StatVisualiser>().UpdateStatUI();
        MAtkVisualiser.GetComponent<StatVisualiser>().UpdateStatUI();
        RAtkVisualiser.GetComponent<StatVisualiser>().UpdateStatUI();
    }

    void CreateDefaultChips()
    {
        Chip newChip;
        foreach (Chip c in ChipLibraryUI)
        {
            //AddChip is not used here cause SortChips and DisplayChips will be called multiple times. This will be handled manually
            newChip = new Chip();
            newChip.CopyChip(c);
            AttachedChips.Add(newChip);
            CurChipSize += newChip.ChipSize;
        }

        /* foreach (Chip c in ChipLibraryAttacks)
        {
            newChip = new Chip();
            newChip.CopyChip(c);
            AttachedChips.Add(newChip);
        } */

        UpdateUIChips();
        SortChips();
        DisplayChips();
    }

    void TestSampleChips()
    {
        AttachedChips.Clear();

        AttachedChips.Add(new Chip(0, 1, 1, Chip.CType.ATK, "First"));
        AttachedChips.Add(new Chip(0, 1, 2, Chip.CType.UI, "Second"));
        AttachedChips.Add(new Chip(0, 1, 3, Chip.CType.STAT, "Third"));
        AttachedChips.Add(new Chip(0, 1, 2, Chip.CType.ATK, "Fourth"));
        AttachedChips.Add(new Chip(0, 1, 1, Chip.CType.UI, "Fifth"));


        foreach (Chip c in AttachedChips)
        {
            c.EvaluateChip();
        }

        SortChips();
        DisplayChips();
    }

    void DestroyChip()
    {
        if (SelectedChipInventory != -1)
        {
            Chip.CType type = AttachedChips[SelectedChipInventory].ChipType;
            AttachedChips.RemoveAt(SelectedChipInventory);

            if (type == Chip.CType.UI)
                UpdateUIChips();
            else if (type == Chip.CType.STAT)
                UpdateStatsChips();

            DisplayChips();
            SelectedChipInventory = -1;
        }
    }
}
