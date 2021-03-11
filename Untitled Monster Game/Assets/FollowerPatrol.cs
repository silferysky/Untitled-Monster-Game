using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerPatrol : StateMachineBehaviour
{
    public List<Vector3> waypoints;

    Rigidbody2D rigidbody;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rigidbody = animator.gameObject.GetComponent<Rigidbody2D>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (GameStateManager.gameState != GameState.Running)
            return;


    }

    void CheckAttackRange()
    {
        Collider2D[] other = Physics2D.OverlapCircleAll(AttackPos.position, AttackRange, PlayerMask);

        if (other.Length > 0)
        {
            rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            animator.SetFloat("Velocity", 0.0f);

            isAttacking = true;
        }
        else
            isAttacking = false;

        foreach (Collider2D otherCollider in other)
        {
            if (otherCollider.GetComponent<HealthScript>() != null &&
                !otherCollider.GetComponent<HealthScript>().GetAlive())
            {
                isAttacking = false;
            }
        }
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
