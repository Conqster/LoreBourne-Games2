using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoreBourne;

public class AiGun : MonoBehaviour
{
    [SerializeField, Range(0.5f, 3f)] private float fireRate = 1;
    [SerializeField, Range(0.5f, 1.5f)] private int damage = 1;
    private ICharacter user;
    private ICharacterAnimation animate;

    [SerializeField] private Transform firePoint; 

    [Space][Space][Header("Ammunition")]
    [SerializeField, Range(0,30)] private int bulletLeft;
    [SerializeField, Range(0,30)] private int magLeft;
    private int maxBulletFullMag;
    [SerializeField] private bool reloading;

    private float timer;
    private bool aiming;
    [SerializeField] private bool wantToShoot;

    private void Start()
    {
        user = GetComponent<ICharacter>();
        animate = GetComponent<ICharacterAnimation>();

        //intial bullet in one mag
        maxBulletFullMag = bulletLeft; 
    }

    private void Update()
    {
        CheckUserIsAiming();
        CheckUserWantToShoot();

        TriggerShot();
    }

    private void TriggerShot()
    {
        timer += Time.deltaTime;
        if (timer >= fireRate)
        {
            if (wantToShoot && aiming)
            {
                timer = 0f;
                FireGun();
            }
        }
    }

    private void CheckUserIsAiming()
    {
        if (TryGetComponent<ICharacter>(out user))
        {
            aiming = user.IsAiming();
        }
    }

    private void CheckUserWantToShoot()
    {
        if (TryGetComponent<ICharacter>(out user))
        {
            wantToShoot = user.ShootConfirm();
        }
    }


    private void FireGun()
    {
        if (!reloading)
        {
            if (bulletLeft > 0)
            {
                //Debug.DrawRay(firePoint.position, transform.forward * 100, Color.red, 2f);
                //Ray ray = new Ray(firePoint.position, firePoint.forward);
                Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo, 100))
                {
                    //Destroy(hitInfo.collider.gameObject);
                }
                animate.ShootingGun(fireRate);
                bulletLeft--;
            }
            else if (bulletLeft == 0 && magLeft > 0)
            {
                Reload();
            }
        }


    }

    private void Reload()
    {
        reloading = true;
        if (magLeft > 0)
        {
            animate.ReloadGun();
            magLeft--;
            bulletLeft = maxBulletFullMag;
        }
    }


    //might have to modify, currently using trigger to animate
    // but for the character animation using bool to animate reload

    public void Reloaded()
    {
        reloading = false;
    }

}
