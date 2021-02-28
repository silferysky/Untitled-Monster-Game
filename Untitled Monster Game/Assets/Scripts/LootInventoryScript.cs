using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootInventoryScript : MonoBehaviour
{
	public List<GameObject> ItemSprites;
	
    // Start is called before the first frame update
    void Start()
    {
		gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void SetLootPosition(Vector3 position)
	{
        //transform.position = new Vector3(position.x, position.y, transform.position.z);
        GetComponent<RectTransform>().position = position;
    }
	
	public void SetSprite(Sprite texture, int index)
	{
		if (index > 3 || index < 0)
			return;
		
		ItemSprites[index].GetComponent<SpriteRenderer>().sprite = texture;
	}
}
