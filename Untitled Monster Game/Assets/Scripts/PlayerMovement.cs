﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rigidbody;
    public GameObject PlayerModel;
    public GameObject Follower;
    public GameObject FollowerTag;
    Vector3 m_playerLocation;

    public float MovementSpeed = 20.0f;
    public float SprintMovSpdMultiplier = 2.0f;
    public float SprintDuration = 5.0f; // How long to sprint for at one time
    public float SprintRegenRate = 2.0f; // Rate of "stamina" regen compared to stamina drain. e.g. 2.0f means its takes half the time to regen
    float sprintTimer;

    public float jumpForce = 2.0f;
    bool isJumping = false;

    bool wasFacingRightLastFrame = true;

    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        m_playerLocation = new Vector3(0.0f, 0.0f, -5.0f);
        rigidbody = gameObject.GetComponent<Rigidbody2D>();

        sprintTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.gameState != GameState.Running)
            return;

        Vector3 moveVec = Vector3.zero;
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) && !isJumping)
        {
            isJumping = true;
            rigidbody.AddForce(new Vector3(0.0f, jumpForce, 0.0f), ForceMode2D.Impulse);

            Follower.GetComponent<Rigidbody2D>().AddForce(new Vector3(0.0f, jumpForce, 0.0f), ForceMode2D.Impulse);

            animator.SetTrigger("IsJumping");

            Follower.GetComponent<Animator>().SetTrigger("IsJumping");

            //moveVec.y += 1.0f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            //moveVec.y -= 1.0f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveVec.x -= 1.0f;
            //PlayerModel.GetComponent<PlayerScript>().FlipSprite(false);
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveVec.x += 1.0f;
            //PlayerModel.GetComponent<PlayerScript>().FlipSprite(true);
        }

        float newMoveSpd = MovementSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) // Sprint
        {
            if (sprintTimer < SprintDuration)
            {
                newMoveSpd *= SprintMovSpdMultiplier;
                sprintTimer += Time.deltaTime;
            }
        }
        else
        {
            if (sprintTimer > 0.0f)
                sprintTimer -= SprintRegenRate * Time.deltaTime;
            else if (sprintTimer < 0.0f)
                sprintTimer = 0.0f;
        }

        rigidbody.AddForce(moveVec * Time.deltaTime * newMoveSpd, ForceMode2D.Impulse);

        Follower.GetComponent<Rigidbody2D>().AddForce(moveVec * Time.deltaTime * newMoveSpd, ForceMode2D.Impulse);

        //For Counterforce
        rigidbody.AddForce(rigidbody.velocity * -0.5f, ForceMode2D.Force);

        Follower.GetComponent<Rigidbody2D>().AddForce(rigidbody.velocity * -0.5f, ForceMode2D.Force);

        UpdateSpriteDirection();
    }

    public float GetStaminaAsFraction()
    {
        return 1.0f - (sprintTimer / SprintDuration);
    }

    public bool GetIsJumping()
    {
        return isJumping;
    }

    public void SetIsJumping(bool set)
    {
        isJumping = set;
    }

    void UpdateSpriteDirection()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;

        bool isFacingRight = IsFacingRight();

        if (isFacingRight)
        {
            if (wasFacingRightLastFrame != isFacingRight) // Now face right
            {
                PlayerModel.transform.localRotation = Quaternion.Euler(0, 0, 0);
                Follower.transform.localRotation = Quaternion.Euler(0, 0, 0);
                FollowerTag.transform.localRotation = Quaternion.Euler(0, 0, 0);
                wasFacingRightLastFrame = isFacingRight;
            }
        }
        else if (!isFacingRight) // Flip sprite
        {
            if (wasFacingRightLastFrame != isFacingRight) // Now face left
            {
                PlayerModel.transform.localRotation = Quaternion.Euler(0, 180, 0);
                Follower.transform.localRotation = Quaternion.Euler(0, 180, 0);
                FollowerTag.transform.localRotation = Quaternion.Euler(0, 180, 0);
                wasFacingRightLastFrame = isFacingRight;
            }
        }

        animator.SetFloat("Velocity", Mathf.Abs(rigidbody.velocity.x));

        Follower.GetComponent<Animator>().SetFloat("Velocity", Mathf.Abs(rigidbody.velocity.x));

    }

    public bool IsFacingRight()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;
        return (cursorPos.x > playerPos.x);
    }
}
