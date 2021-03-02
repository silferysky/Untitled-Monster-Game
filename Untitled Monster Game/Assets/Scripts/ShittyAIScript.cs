using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShittyAIScript : MonoBehaviour
{
    Rigidbody2D rigidbody;
    HealthScript healthscript;

    public float MovementSpeed = 20.0f;

    public float movementTimer;
    public float duration;
    bool isMovingRight;

    // Sprite Flipping
    public bool isFacingRight = true;

    public bool isAttacking = false;
    public float pauseTimer;
    public float pauseDuration = 0.01f;

    public Canvas canvas;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
		rigidbody = gameObject.GetComponent<Rigidbody2D>();
        healthscript = gameObject.GetComponent<HealthScript>();

        isMovingRight = Random.Range(0, 1) != 0; // i.e. 1 means moving right at start
        ResetMovementTimers(); // get a random timer for moving in a direction
    }

    // Update is called once per frame
    void Update()
    {
        if (healthscript.GetAlive())
        {
            /* if (isAttacking)
            {
                pauseTimer += Time.deltaTime;

                if (pauseTimer >= pauseDuration)
                {
                    isAttacking = false;
                    pauseTimer = 0.0f;
                }
            }
            //else */
                UpdateMovement();
        }
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
            FlipSprite(false);
        }
        else if (isMovingRight)
        {
            moveVec.x += 1.0f;
            FlipSprite(true);
        }

        rigidbody.AddForce(moveVec * Time.deltaTime * MovementSpeed, ForceMode2D.Impulse);

        //For Counterforce
        rigidbody.AddForce(rigidbody.velocity * -0.5f, ForceMode2D.Force);

        animator.SetFloat("Velocity", Mathf.Abs(rigidbody.velocity.x));
    }

    void ResetMovementTimers()
    {
        movementTimer = 0.0f;
        duration = Random.Range(3, 5);
    }

    private void FlipSprite(bool face_right_this_frame)
    {
        if (face_right_this_frame && !isFacingRight)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            isFacingRight = true;

            canvas.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (!face_right_this_frame && isFacingRight)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            isFacingRight = false;

            canvas.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
