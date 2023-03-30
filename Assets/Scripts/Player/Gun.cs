using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoreBourne;

public class Gun : MonoBehaviour
{
    [SerializeField, Range(0.5f, 1.5f)] private float fireRate = 1;
    [SerializeField, Range(0.5f, 1.5f)] private int damage = 1;
    private ICharacter aCharacter;
    private CharactersAnimations animate;

    [SerializeField] private Transform firePoint;
    //[SerializeField] private Transform bulletPrefab;

    [Space][Space][Header("Ammunition")]
    [SerializeField, Range(0,30)] private int bulletLeft;
    [SerializeField, Range(0, 10)] private int magLeft;
    private int maxBulletFullMag;
    [SerializeField] private bool reloading;

    [SerializeField] private bool testing;

    public float timer;
    private bool aiming;

    private void Start()
    {
        aCharacter = GetComponent<ICharacter>();
        animate = GetComponent<CharactersAnimations>();
    }

    private void Update()
    {
        CheckUserIsAiming();

        if (testing)
            Reload();

        timer += Time.deltaTime;
        if(timer >= fireRate)
        {
            if(Input.GetButton("Fire1") && aiming)
            {
                timer = 0f;
                FireGun();
            }
        }
    }

    private void CheckUserIsAiming()
    {
        if(TryGetComponent<ICharacter>(out aCharacter))
        {
            aiming = aCharacter.IsAiming();
        }
    }


    private void FireGun()
    {
        if(!reloading)
        {
            if (bulletLeft > 0)
            {
                Debug.DrawRay(firePoint.position, transform.forward * 100, Color.red, 2f);
                Ray ray = new Ray(firePoint.position, firePoint.forward);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo, 100))
                {
                    //Destroy(hitInfo.collider.gameObject);
                }
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
        if(magLeft > 0)
        {
            animate.ReloadGun();
        }
    }

    public void ReloadingOver()
    {
        reloading = false;
    }


}
