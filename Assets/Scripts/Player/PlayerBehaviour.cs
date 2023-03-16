using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace LoreBourne
{

    enum PlayerInputType { Keyboard, Controller};

    [RequireComponent(typeof(Rigidbody))]
    public class PlayerBehaviour : Damagable
    {
        private PlayerAnimation playerAnim;
        private PlayerWeaponBehaviour weapon;

        [Header("Movement")]
        [SerializeField, Range(0, 10)] private float movementSpeed = 5f;
        private Vector3 movementInput;
        private Rigidbody playerRigidbody;
        [SerializeField, Range(0, 100)] private float jumpForce = 50;
        private bool jump;
        [SerializeField] private bool jumping;
        [SerializeField] ForceMode jumpForceMode;

        [Space]
        [Space]
        [Header("Ground Check")]
        [SerializeField] private Transform groundCheck;
        [SerializeField, Range(0.1f, 5f)] private float allowDistanceToGround;
        //[SerializeField, Range(-0.1f, -10f)] private float gravity = -9.87f;
        [SerializeField] LayerMask groundedLayer;
        private bool onGround;
        [SerializeField, Range(0, 5)] private float invokeWhenGrounded;

        [Space]
        [Space]
        [Header("Multipilers")]
        [SerializeField, Range(1, 10)] private int jumpMultiplier = 2;

        [Space][Header("Combat")]
        [SerializeField] private bool aiming;
        [SerializeField] private bool wantToShoot;

        //
        //
        private float _targetRotation = 0.0f;
        [SerializeField] Camera mainCam;
        public float RotationSmoothTime = 0.12f;
        private float _rotationVelocity;



        private void Start()
        {
            playerRigidbody = GetComponent<Rigidbody>();
            playerAnim = GetComponent<PlayerAnimation>();
            weapon = GetComponent<PlayerWeaponBehaviour>();
        }


        private void Update()
        {
            GetPlayerInput();
            ApplyMovement();

            IsGrounded();



            if (jump)
                ApplyJump();

            if (jumping && !jump)
                BackOnGround();

            //if (wantToShoot && aiming)
            //    weapon.Shoot();
            if (wantToShoot && aiming)
                weapon.TryToShoot();

            //print(jumping);
        }


        private void GetPlayerInput()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            jump = Input.GetButtonDown("Jump");

            //aiming = Input.GetButtonDown("JoyStick LT");
            float aim = Input.GetAxis("JoyStick LT");
            if (aim > 0.1)
                aiming = true;
            else
                aiming = false;

            float shootPressRT = Input.GetAxis("JoyStick RT");
            bool shootPress = Input.GetButtonDown("Fire1");

            if (shootPressRT > 0.1 || shootPress)
                wantToShoot = true;
            else
                wantToShoot = false;
            //if (aim > 0.1) ? aiming = true : aiming = false;

            movementInput = new Vector3(horizontalInput, 0, verticalInput);
        }


        private void ApplyJump()
        {
            onGround = IsGrounded();
            Vector3 upForce = new Vector3(0, jumpForce * jumpMultiplier, 0);

            if (onGround && !jumping)   // && !jumping
            {
                jumping = true;
                playerRigidbody.AddForce(upForce, jumpForceMode);
            }

        }

        private void BackOnGround()
        {
            if (IsGrounded())
            {
                jumping = false;
                //print("this function has been accessed");
            }

        }

        private void ApplyMovement()
        {
            Vector2 movement = new Vector2(movementInput.x, movementInput.z).normalized;

            if(movement != Vector2.zero)
            { 
                _targetRotation = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + mainCam.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                            RotationSmoothTime);
               //transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                Quaternion bodyRotation = Quaternion.Euler(0.0f, rotation, 0.0f);

                playerRigidbody.MoveRotation(bodyRotation);

                Vector3 targetDir = Quaternion.Euler(0, _targetRotation, 0) * Vector3.forward;
                playerRigidbody.MovePosition(transform.position + targetDir.normalized * Time.deltaTime * movementSpeed);
            }

            //Vector3 targetMovement = new Vector3(movement.x, 0, movement.y);

            

            //playerRigidbody.MovePosition(Vector3.forward);
            //a method of making the player look toward cameras direction 

        }


        



        private void ApplyMovement2()
        {
            if (playerRigidbody != null)
            {
                //Vector2 movement = movementInput.normalized;

                //playerRigidbody.MovePosition(transform.position + movementInput.normalized * Time.deltaTime * movementSpeed);

                // normalise input direction
                Vector3 inputDirection = new Vector3(movementInput.x, 0.0f, movementInput.y).normalized;

                // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
                // if there is a move input rotate player when the player is moving
                if (movementInput != Vector3.zero)
                {
                    _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                      mainCam.transform.eulerAngles.y;
                    float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                        RotationSmoothTime);

                    // rotate to face input direction relative to camera position
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                }


                Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

                playerRigidbody.MovePosition(transform.position + targetDirection.normalized * (movementSpeed * Time.deltaTime));
            }
        }

        private bool IsGrounded()
        {
            if (groundCheck != null)
            {
                bool onGround = Physics.CheckSphere(groundCheck.position, allowDistanceToGround, groundedLayer);

                if (onGround)
                {
                    Debug.DrawLine(groundCheck.position, (groundCheck.position - new Vector3(0, allowDistanceToGround, 0)), Color.green);
                    //print("grounded");
                }
                else
                {
                    Debug.DrawLine(groundCheck.position, (groundCheck.position - new Vector3(0, allowDistanceToGround, 0)), Color.red);
                    //print("not grounded");
                }

                return onGround;

            }
            else
            {
                print("Please add groundcheck gameObject to ref");
                return false;
            }
        }

        public bool IsDropping()
        {
            float physicsVelY = playerRigidbody.velocity.y;

            if (physicsVelY < 0)
            {
                return true;
            }
            return false;
        }


        public Vector2 GetMovement()
        {
            Vector2 movement = new Vector2(movementInput.x, movementInput.z);
            return movement;
        }

        public bool Jumped()
        {
            return jumping;
        }

        public bool Aiming()
        {
            return aiming;
        }

    }
}


