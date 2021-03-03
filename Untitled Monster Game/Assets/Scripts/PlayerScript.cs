using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    Vector3 m_playerLocation;
	public float MovementSpeed = 20.0f;
    Rigidbody2D rigidbody;
    HealthScript healthScript;

    public IAbility activeAbility = null;//{ get; set; }

    // Sprite Flipping
    public bool isFacingRight = true;

    public GameObject ChipMenu;
	
    // Start is called before the first frame update
    void Start()
    {
        m_playerLocation = new Vector3(0.0f, 0.0f, -5.0f);
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        healthScript = gameObject.GetComponent<HealthScript>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //if (collision.otherCollider.gameObject.tag != "AI")
        //    isJumping = false;
        if (Input.GetKeyDown(KeyCode.F))
        {
            ChipMenuScript chipMenu = ChipMenu.GetComponent<ChipMenuScript>();
            if (healthScript.GetAlive() && other.tag == "AI")
            {
                HealthScript other_hs = other.transform.gameObject.GetComponent<HealthScript>();

                if (other_hs && !other_hs.GetAlive())
                {
                    if (!other_hs.IsLooted)
                    {
                        //Generate Loot
                        other_hs.IsLooted = true;
                        chipMenu.GenerateLoot(other.gameObject);

                        //Move this to outside if statement once decide to actually code re-visiting bodies
                        chipMenu.OpenMenu();
                        chipMenu.OpenLootInventory();
                        chipMenu.DisplayLoot(other.gameObject);
                        chipMenu.LastDeadboi = other.gameObject;
                    }
                    
                }
            }
        }
    }

    public void FlipSprite(bool face_right_this_frame)
    {
        if (face_right_this_frame && !isFacingRight)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            isFacingRight = true;
        }
        else if (!face_right_this_frame && isFacingRight)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            isFacingRight = false;
        }
    }
}
