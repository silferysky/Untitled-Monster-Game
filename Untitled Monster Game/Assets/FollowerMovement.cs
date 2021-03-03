using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerMovement : MonoBehaviour
{
    Rigidbody2D rigidbody;

    public GameObject FollowerModel;
    public float MovementSpeed = 20.0f;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.gameState != GameState.Running)
            return;
        
        if (Input.GetKey(KeyCode.A))
        {
            FollowerModel.GetComponent<ShittyAIScript>().moveVec = Vector3.zero;
            FollowerModel.GetComponent<ShittyAIScript>().ResetMovementTimers();
            FollowerModel.GetComponent<ShittyAIScript>().isMovingRight = false;
        }
        if (Input.GetKey(KeyCode.D))
        {
            FollowerModel.GetComponent<ShittyAIScript>().moveVec = Vector3.zero;
            FollowerModel.GetComponent<ShittyAIScript>().ResetMovementTimers();
            FollowerModel.GetComponent<ShittyAIScript>().isMovingRight = true;
        }
    }
}
