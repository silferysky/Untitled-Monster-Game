using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    Vector3 m_playerLocation;
	public float MovementSpeed = 20.0f;

    public float jumpForce = 2.0f;

    Rigidbody2D rigidbody;
    public bool isJumping = false;

    // Sprite Flipping
    bool isFacingRight = true;
	
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
        if (Input.GetKey(KeyCode.W) && !isJumping)
        {
            rigidbody.AddForce(new Vector3(0.0f, jumpForce, 0.0f), ForceMode2D.Impulse);
            isJumping = true;

            //moveVec.y += 1.0f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            //moveVec.y -= 1.0f;
        }
        if (Input.GetKey(KeyCode.A))
	    {
            moveVec.x -= 1.0f;
            FlipSprite(false);
        }
	    if (Input.GetKey(KeyCode.D))
	    {
            moveVec.x += 1.0f;
            FlipSprite(true);
        }
		
		rigidbody.AddForce(moveVec * Time.deltaTime * MovementSpeed, ForceMode2D.Impulse);
		
		//For Counterforce
		rigidbody.AddForce(rigidbody.velocity * -0.5f, ForceMode2D.Force);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //if (collision.otherCollider.gameObject.tag != "AI")
        //    isJumping = false;
    }

    private void FlipSprite(bool face_right_this_frame)
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
