using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShittyAIScript : MonoBehaviour
{
    Rigidbody2D rigidbody;

    public float MovementSpeed = 20.0f;

    public float movementTimer;
    public float duration;
    bool isMovingRight;

    // Start is called before the first frame update
    void Start()
    {
		rigidbody = gameObject.GetComponent<Rigidbody2D>();

        isMovingRight = Random.Range(0, 1) != 0; // i.e. 1 means moving right at start
        ResetMovementTimers(); // get a random timer for moving in a direction
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();

        // TODO: take damage
    }

    void UpdateMovement()
    {
        Vector3 moveVec = Vector3.zero;

        movementTimer += Time.deltaTime;

        if (movementTimer >= duration)
        {
            moveVec.x = 0.0f;
            ResetMovementTimers();
            isMovingRight = !isMovingRight;
        }

        if (!isMovingRight)
        {
            moveVec.x -= 1.0f;
        }
        else if (isMovingRight)
        {
            moveVec.x += 1.0f;
        }

        rigidbody.AddForce(moveVec * Time.deltaTime * MovementSpeed, ForceMode2D.Impulse);

        //For Counterforce
        rigidbody.AddForce(rigidbody.velocity * -0.5f, ForceMode2D.Force);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    ResetMovementTimers();
    //    isMovingRight = !isMovingRight;
    //}

    void ResetMovementTimers()
    {
        movementTimer = 0.0f;
        duration = Random.Range(3, 5);
    }
}
