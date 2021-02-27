using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 m_playerLocation;
	public float MovementSpeed = 20.0f;
    Rigidbody2D rigidbody;

    public GameObject PlayerModel;
	
    // Start is called before the first frame update
    void Start()
    {
        m_playerLocation = new Vector3(0.0f, 0.0f, -5.0f);
		rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
	    Vector3 moveVec = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            //rigidbody.AddForce(new Vector3(0.0f, jumpForce, 0.0f), ForceMode2D.Impulse);
            //isJumping = true;

            moveVec.y += 1.0f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveVec.y -= 1.0f;
        }
        if (Input.GetKey(KeyCode.A))
	    {
            moveVec.x -= 1.0f;
            PlayerModel.GetComponent<PlayerScript>().FlipSprite(false);
        }
	    if (Input.GetKey(KeyCode.D))
	    {
            moveVec.x += 1.0f;
            PlayerModel.GetComponent<PlayerScript>().FlipSprite(true);
        }
		
		rigidbody.AddForce(moveVec * Time.deltaTime * MovementSpeed, ForceMode2D.Impulse);
		
		//For Counterforce
		rigidbody.AddForce(rigidbody.velocity * -0.5f, ForceMode2D.Force);
    }
}
