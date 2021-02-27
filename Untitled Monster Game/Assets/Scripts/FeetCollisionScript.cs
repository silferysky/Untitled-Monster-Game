using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetCollisionScript : MonoBehaviour
{
    GameObject parent;
	
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
	    
    }

    private void OnCollisionStay2D()
    {
        parent.GetComponent<PlayerMovement>().isJumping = false;
    }
}
