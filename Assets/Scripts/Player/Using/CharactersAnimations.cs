using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoreBourne;

public class CharactersAnimations : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private int moveHash, sideHash, jumpHash;
    private int jumpingHash, inAirHash;
    private int reloadHash, shootHash;




    private void Start()
    {
        SetHash();
    }


    private void SetHash()
    {
        moveHash = Animator.StringToHash("moveForward");
        sideHash = Animator.StringToHash("moveSideWays");

        jumpHash = Animator.StringToHash("jump");
        jumpingHash = Animator.StringToHash("jumpVelocity");
        inAirHash = Animator.StringToHash("jumpAir");

        reloadHash = Animator.StringToHash("reload");
    }



    public void MovementAnimation(Vector2 moveValue)
    {
        animator.SetFloat(moveHash, moveValue.y, 0.1f, Time.deltaTime);
        animator.SetFloat(sideHash, moveValue.x, 0.1f, Time.deltaTime);
    }

    public void Jump(bool value, float velocity)
    {
        animator.SetBool(jumpHash, value);
        animator.SetFloat(jumpingHash, velocity);

    }

    public void InAir(bool value, float rate)
    {
        if(rate != 0)
            animator.SetBool(inAirHash, value);
    }

    public void AimingWeaponLayer(bool aim)
    {
        if (aim)
        {
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
        }
        else
        {
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }
    }


    public void ReloadGun()
    {
        animator.SetBool(reloadHash, true);
        Invoke("EndReload", 0.5f);
    }


    private void EndReload()
    {
        animator.SetBool(reloadHash, false);
        print("trying to play reload animation");
    }

}
