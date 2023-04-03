using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoreBourne;

public class NPCAnimation : MonoBehaviour, ICharacterAnimation
{
    [SerializeField] private Animator animator;

    private int moveHash;
    private int reloadHash, shootHash;


    private void Start()
    {
        SetHash();
    }

    private void SetHash()
    {
        moveHash = Animator.StringToHash("moveSpeed");

        reloadHash = Animator.StringToHash("reload");
        shootHash = Animator.StringToHash("shoot");
    }

    public void MovementAnimation(float moveValue)
    {
        animator.SetFloat(moveHash, moveValue);
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


    public void ShootingGun(float fireRate)
    {
        animator.SetBool(shootHash, true);
        float coolDown = fireRate * 0.5f;
        Invoke("Shot", coolDown);
    }

    private void Shot()
    {
        animator.SetBool(shootHash, false);
    }

    private void EndReload()
    {
        animator.SetBool(reloadHash, false);
        //print("trying to play reload animation");
    }

}
