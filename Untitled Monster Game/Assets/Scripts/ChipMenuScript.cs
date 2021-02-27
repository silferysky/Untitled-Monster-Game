using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public bool LootMenuIsOpen = false;
    int SelectedChipInventory = -1;
    int SelectedChipLoot = -1;

    public List<Button> Buttons = new List<Button>();

    // Start is called before the first frame update
    void Start()
    {
        CurChipSize = 0;

        Buttons[0].onClick.AddListener(ToggleMenu);

        //Disable this once chips are in place
        TestSampleChips();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        Background.SetActive(true);
    }

    public void CloseMenu()
    {
        Background.SetActive(false);
        SelectedChipInventory = -1;
        SelectedChipLoot = -1;
    }

    public void ToggleMenu()
    {
        MenuIsOpen = !MenuIsOpen;

        if (MenuIsOpen)
            OpenMenu();
        else
            CloseMenu();
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
