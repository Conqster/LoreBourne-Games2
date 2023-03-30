using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LoreBourne
{
    enum PlayerInputType { Keyboard, Controller}
    public class CharacterMovement : MonoBehaviour, ICharacter
    {
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


        [Space]
        [Space]
        [Header("Multipilers")]
        [SerializeField, Range(1, 10)] private int jumpMultiplier = 2;
        [SerializeField] private bool aiming;

        [Space][Header("Input")]
        [SerializeField] private PlayerInputType inputType;

        private float _targetRotation = 0.0f;
        [SerializeField] Camera mainCam;
        public float RotationSmoothTime = 0.12f;
        private float _rotationVelocity;

        private CharactersAnimations animate;


        private void Start()
        {
            playerRigidbody = GetComponent<Rigidbody>();
            animate = GetComponent<CharactersAnimations>();
        }

        private void Update()
        {
            PlayerInputs();

            if (jumping && !jump)
                BackOnGround();

            animate.AimingWeaponLayer(aiming);
        }

        private void FixedUpdate()
        {
            ApplyMovement();

            if (jump)
                ApplyJump();




            JumpAnimationLogic();
        }

        private void PlayerInputs()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            jump = Input.GetButtonDown("Jump");

            movementInput = new Vector3(horizontalInput, 0, verticalInput);

            switch (inputType)
            {
                case PlayerInputType.Controller:
                    float aim = Input.GetAxis("JoyStick LT");
                    if (aim > 0.1)
                        aiming = true;
                    else
                        aiming = false;
                    break;
                case PlayerInputType.Keyboard:
                        aiming = Input.GetButton("Fire2");
                    break;
            }

        }

        private void ApplyMovement()
        {
            Vector2 movement = new Vector2(movementInput.x, movementInput.z).normalized;

            if (movement != Vector2.zero)
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

            animate.MovementAnimation(movement);

            //Vector3 targetMovement = new Vector3(movement.x, 0, movement.y);

            //playerRigidbody.MovePosition(Vector3.forward);
            //a method of making the player look toward cameras direction 

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

        private void BackOnGround()
        {
            if (IsGrounded())
            {
                jumping = false;
                //print("this function has been accessed");
            }

        }

        public bool IsAiming()
        {
            // probably needs a logic to return true only if not jumping
            return aiming;
        }


        private void JumpAnimationLogic()
        {
            float jumpDir = playerRigidbody.velocity.y;
            jumpDir = Mathf.Clamp(jumpDir, -1f, 1f);

            bool playerinAir = !IsGrounded();


            animate.InAir(playerinAir, jumpDir);
            animate.Jump(jump, jumpDir);
        }

    }

}

