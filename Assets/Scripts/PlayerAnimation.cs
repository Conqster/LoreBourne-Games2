using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoreBourne;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerBehaviour player;
    private Animator animator;
    private Rigidbody playerRb;
    private PlayerWeaponBehaviour weapon;

    private Vector2 movementVector;
    private float jumpVelocity;
    private bool aiming;

    private int moveForwardHash, moveSideWaysHash, jumpHash;
    private int backOnGroundHash, jumpUpVelocityHash, dropHash;
    private int reloadHash, shootHash, reloadingHash, shootingHash;
    private bool jump, dropping, reloading, shooting;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerBehaviour>();
        playerRb = GetComponent<Rigidbody>();
        weapon = GetComponent<PlayerWeaponBehaviour>();

        SetPlayerAnimationHash();
    }

    private void Update()
    {
        UpdateParameters();
        UpdatePlayerAnimationVariables();

        ActivateLayer();

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
    private void UpdatePlayerAnimationVariables()
    {
        animator.SetBool(reloadingHash, reloading);
        animator.SetBool(shootingHash, shooting);
    }

    private void ActivateLayer()
    {
        if(aiming)
        {
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
        }
        else
        {
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }

    }
    

    public void ShootAnimation()
    {
        animator.SetTrigger(shootHash);
    }

    public void ReloadAnimation()
    {
        animator.SetTrigger(reloadHash);
    }


    private void SetPlayerAnimationHash()
    {
        moveForwardHash = Animator.StringToHash("moveForward");
        moveSideWaysHash = Animator.StringToHash("moveSideWays");

        jumpHash = Animator.StringToHash("jump");
        jumpUpVelocityHash = Animator.StringToHash("jumpVelocity");
        backOnGroundHash = Animator.StringToHash("backOnGround");

        dropHash = Animator.StringToHash("dropping");
        shootHash = Animator.StringToHash("shoot");
        shootingHash = Animator.StringToHash("shooting");
        reloadHash = Animator.StringToHash("reload");
        reloadingHash = Animator.StringToHash("reloading");
    }

    private void UpdateParameters()
    {
        movementVector = player.GetMovement();
        jump = player.Jumped();

        jumpVelocity = playerRb.velocity.y;

        aiming = player.Aiming();
        dropping = player.IsDropping();

        reloading = weapon.IsReloading();
        shooting = weapon.IsShooting();
        //print(jump);
    }
}
