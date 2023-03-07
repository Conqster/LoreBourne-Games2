using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class WeaponBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform spawnPoint;
    [SerializeField, Range(1, 100)] private int bulletForce;
    [SerializeField, Range(1, 100)] private int bulletMagFull;
    [SerializeField, Range(0,100)] private int bulletLeft;
    [SerializeField, Range(0, 20)] private int magazineLeft;
    [SerializeField] private bool reloading; //to prevent shooting while reloading

    [SerializeField] private Transform target;

    private NPCAnim npcAnimator;
    private enum orientation { Up, Down, Left, Right};


    [Space][Space][Header("For Testing")]
    [SerializeField] private orientation _orientation;
    private void Start()
    {
        npcAnimator = GetComponent<NPCAnim>();
    }

    private void Update()
    {


    }

    private void OnSceneGUI()
    {
        Handles.color = Color.red;
        Handles.Label(transform.position, "bullet left:" + bulletLeft.ToString() + "\n mag left: " + magazineLeft.ToString());
    }

    public void Fire()
    {
       
        if(bulletLeft > 0 && !reloading)
        {
            GameObject newBullet = Instantiate(bullet, spawnPoint.position, bullet.transform.rotation);

            bulletLeft--;
            Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();

            //Vector3 bulletDirection = spawnPoint.rotation;
            //bulletRb.velocity = spawnPoint.TransformDirection(Vector3.forward * bulletForce);
            bulletRb.AddForce(spawnPoint.forward * bulletForce * 100);

            //bulletRb.velocity = Vector3.forward * bulletForce;
        }
        else if(bulletLeft <= 0 && magazineLeft > 0)
        {
            Reload();

        }



    }

    public void TryToShot(bool value)
    {
        npcAnimator.shoot = value;
    }
   
    private void Reload()
    {
        reloading = true;           //going to set to false in animation when reloading has finished
        if(magazineLeft > 0)
        {
            npcAnimator.TriggerReload();             //play reload animation
            bulletLeft = bulletMagFull;
            magazineLeft--;
            ReadyToShoot();
        }
    }



    public void ReadyToShoot()
    {
        reloading = false;
    }


    //probably going to have an animation trigger
    // when there is no more ammo


    public int BulletLeft()
    {
        return bulletLeft;
    }

    public int MagLeft()
    {
        return magazineLeft;
    }

}
