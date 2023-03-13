using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LoreBourne
{
    public class PlayerWeaponBehaviour : MonoBehaviour
    {
        [SerializeField] private LayerMask aimColliderLayer = new LayerMask();
        [SerializeField] private Transform debugTransform;
        [SerializeField] private GameObject crossHair;
        [SerializeField] private Transform mainCam;
        [SerializeField] private Transform bullet;
        [SerializeField] private Transform spawnBulletPos;
        [SerializeField, Range(1, 10)] private float bulletForceMultipler;

        //ammunition
        [Space][Space]
        [Header("Weapon Ammunition")]
        [SerializeField, Range(0,30)] private int bulletLeft;
        private int bulletFullMag;
        [SerializeField, Range(0, 10)] private int magLeft;

        private int bulletPerShot = 0;

        private bool aiming, shooting;
        private bool reloading = false;
        private PlayerBehaviour player;
        private PlayerAnimation playerAnim;


        [Space][Space]
        [Header("Test debugging")]
        [SerializeField, Range(0f,5f)] private float debuggerDisableTimer;
        [SerializeField] private Vector3 debugCurrentPos;
        private float currentTime;

        private void Start()
        {
            //aimColliderLayer = new LayerMask();
            player = GetComponent<PlayerBehaviour>();
            playerAnim = GetComponent<PlayerAnimation>();
            bulletFullMag = bulletLeft;
        }

        private void Update()
        {
            aiming = player.Aiming();
            EnableAim();
            if(aiming)
                AimRayCast();

        }


        public void TryToShoot()
        {
            if(bulletLeft >= 0)
            {
                shooting = true;
                playerAnim.ShootAnimation();
            }
        }

        private void Shoot()
        {
            if(bulletLeft > 0 && !reloading)
            {
                if(bulletPerShot == 0)
                {
                    //shooting = true;
                    bulletPerShot = 10;
                    Vector3 mouseWorldPosition = GetMousePos();
                    Vector3 aimDirection = (mouseWorldPosition - spawnBulletPos.position).normalized;

                    Transform newBullet = Instantiate(bullet, spawnBulletPos.position, Quaternion.LookRotation(aimDirection, Vector3.up));
                    BulletBehaviour behaviour = newBullet.GetComponent<BulletBehaviour>();
                    behaviour.refDirection = spawnBulletPos;
                    Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();
                    bulletLeft--;
                    bulletRb.AddForce(newBullet.forward * bulletForceMultipler * 1000);
                    shooting = false;

                }
            }
            else if(bulletLeft <= 0 && magLeft > 0)
            {
                Reload();
            }
            
        }

        public void RecaliberateWeapon()
        {
            bulletPerShot = 0;
        }

        private void Reload()
        {
            reloading = true;    //going to set to false in animation when reloading has finished
            if(magLeft > 0)
            {
                playerAnim.ReloadAnimation();
                bulletLeft = bulletFullMag;
                magLeft--;
            }
        }

        public void FinishedReloading()
        {
            reloading = false;
        }


        private void EnableAim()
        {
            bool activate;
            activate = aiming ? true : false;

            crossHair.SetActive(activate);
            debugTransform.gameObject.SetActive(activate);



            if (debugTransform.position != Vector3.zero)
            {
                currentTime += Time.deltaTime;
                debugCurrentPos = debugTransform.position;
            }
            if (currentTime >= debuggerDisableTimer)
            {
                if (debugTransform.position == debugCurrentPos)
                {
                    debugCurrentPos = Vector3.zero;
                    debugTransform.position = Vector3.zero;
                }
                currentTime = 0.0f;
            }

        }

        private void AimRayCast()
        {
            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Vector3 mouseWorldPosition = Vector3.zero;

            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if(Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayer))
            {
                debugTransform.position = raycastHit.point;
                mouseWorldPosition = raycastHit.point;
            }

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = player.transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(player.transform.forward, aimDirection, Time.deltaTime * 20f);

            //RotatePlayer();
        }

        private Vector3 GetMousePos()
        {
            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Vector3 point;
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayer))
            {
                point = raycastHit.point;
            }
            else
            {
                point = spawnBulletPos.position + new Vector3(2, 2, 2);
            }

            return point;
        }

        private void RotatePlayer()
        {
            float _targetRot = Mathf.Atan2(player.transform.eulerAngles.y, mainCam.transform.eulerAngles.y);

            transform.forward = Vector3.Lerp(player.transform.forward, mainCam.transform.forward, Time.deltaTime * 20);


            
        }

        public bool IsReloading()
        {
            return reloading;
        }

        public bool IsShooting()
        {
            return shooting;
        }
       
    }
}
