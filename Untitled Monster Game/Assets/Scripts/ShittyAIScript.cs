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
    public Transform AttackPos;
    public float AttackRange = 0.0f;

    public LayerMask PlayerMask;

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
        if (GameStateManager.gameState != GameState.Running)
            return;

        if (healthscript.GetAlive())
        {
            if (!isAttacking)
                UpdateMovement();
        }

        CheckAttackRange();
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

    void CheckAttackRange()
    {
        Collider2D[] player = Physics2D.OverlapCircleAll(AttackPos.position, AttackRange, PlayerMask);

        if (player.Length > 0)
        {
            rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            animator.SetFloat("Velocity", 0.0f);

            isAttacking = true;
        }
        else
            isAttacking = false;
    }

    private void FlipSprite(bool face_right_this_frame)
    {
        if (face_right_this_frame && !isFacingRight)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            isFacingRight = true;

            if (canvas)
                canvas.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (!face_right_this_frame && isFacingRight)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            isFacingRight = false;

            if (canvas)
                canvas.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
