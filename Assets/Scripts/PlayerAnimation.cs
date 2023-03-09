using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoreBourne;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerBehaviour player;
    private Animator animator;
    private Rigidbody playerRb;

    private Vector2 movementVector;
    private float jumpVelocity;

    private int moveForwardHash, moveSideWaysHash, jumpHash;
    private int backOnGroundHash, jumpUpVelocityHash, dropHash;
    private bool jump, dropping;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerBehaviour>();
        playerRb = GetComponent<Rigidbody>();

        SetPlayerAnimationHash();
    }

    private void Update()
    {
        UpdatePlayerMovement();

        if(animator != null)
        {
            AnimateMovement();
        }
    }


    private void AnimateMovement()
    {
        animator.SetFloat(moveForwardHash, movementVector.y, 0.1f, Time.deltaTime);
        animator.SetFloat(moveSideWaysHash, movementVector.x, 0.1f, Time.deltaTime);

        animator.SetBool(jumpHash, jump);
        animator.SetFloat(jumpUpVelocityHash, jumpVelocity);

        animator.SetBool(dropHash, dropping);
        //animator.SetTrigger(jumpHash);
    }


    


    private void SetPlayerAnimationHash()
    {
        moveForwardHash = Animator.StringToHash("moveForward");
        moveSideWaysHash = Animator.StringToHash("moveSideWays");

        jumpHash = Animator.StringToHash("jump");
        jumpUpVelocityHash = Animator.StringToHash("jumpVelocity");
        backOnGroundHash = Animator.StringToHash("backOnGround");

        dropHash = Animator.StringToHash("dropping");
    }

    private void UpdatePlayerMovement()
    {
        movementVector = player.GetMovement();
        jump = player.Jumped();

        jumpVelocity = playerRb.velocity.y;

        dropping = player.IsDropping();
        //print(jump);
    }
}
