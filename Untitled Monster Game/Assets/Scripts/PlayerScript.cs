using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    Vector3 m_playerLocation;
	float MovementSpeed = 5.0f;
	
	Rigidbody2D rigidbody;
	
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
            moveVec.y += 1.0f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveVec.y -= 1.0f;
        }
        if (Input.GetKey(KeyCode.A))
	    {
            moveVec.x -= 1.0f;
	    }
	    if (Input.GetKey(KeyCode.D))
	    {
            moveVec.x += 1.0f;
	    }
		
		rigidbody.AddForce(moveVec);
		
        //m_playerLocation += moveVec * MovementSpeed * Time.deltaTime;
        //transform.position = m_playerLocation;
    }
}
