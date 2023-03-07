using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LoreBourne
{
    public class PlayerMovement : MonoBehaviour
    {

        //player rigidbody
        //player GameObject

        //horizontal and vertical inputs

        //movementSpeed

        //isgrounded
        //groundDistance

        private GameObject player;
        private Rigidbody playerRigidbody;
        private Transform playerTransform;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask ground;

        private float horizontalInput, verticalInput;
        private Vector3 movementInput, gravityInput;
        [SerializeField] private bool isgrounded;
        [SerializeField, Range(0,10)] private float groundCheckHeight;
        [SerializeField, Range(0, -10)] private float gravity = -9.81f;

        private Vector3 movementDirection;
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField, Range(-1, 1)] private float rotationTiming = 0.1f;
        private float rotationDampingSpeed;
        [SerializeField] private Transform followCamTransform;

        

        private void Start()
        {

            //player = GetComponent<GameObject>(); 
            //playerTransform = player.transform;
            playerRigidbody = GetComponent<Rigidbody>();
            //groundCheck = GetComponent<Transform>();
            
        }

        private void Update()
        {
            GetPlayerInputs();

            isgrounded = CheckGrounded();
        }
        private void FixedUpdate()
        {
            ApplyMovement(); 
            //ApplyRotation();
            ApplyGravity();
        }


        private void GetPlayerInputs()
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");


            //InternalInpnt like gravity;
            //if(!isgrounded)
            //{
            //    gravityInput = gravity;
            //}
            //else
            //{
            //    gravityInput = 0f;
            //}

            //print(gravityInput);

            movementInput = new Vector3(horizontalInput, 0f, verticalInput);
        }


        /// <summary>
        /// Applys rotation and  movementDirection
        /// </summary>
        private void ApplyMovement()
        {
            Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized;   //movement stores the player locally in this function 

            if(movement.magnitude >= 0.1f && playerRigidbody != null)   
            {
                float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg  + followCamTransform.eulerAngles.y;

                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,
                    ref rotationDampingSpeed, rotationTiming);
                
                movementDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
                playerRigidbody.MoveRotation(rotation);

                playerRigidbody.MovePosition(transform.position + movementDirection.normalized * Time.deltaTime * movementSpeed);
            }
        }

        //private void ApplyMovement()
        //{

        //    if(playerRigidbody != null)
        //    {
        //        //playerRigidbody.MovePosition(transform.position + movementInput * Time.deltaTime * movementSpeed);
        //    }
        //    else
        //    {
        //        Debug.Log("Please attach Rigidbody");
        //    }

        //}

        private void ApplyGravity()
        {
            if(!isgrounded)
            {
                gravityInput = new Vector3(0f, gravity, 0f);
                playerRigidbody.MovePosition(transform.position + Time.deltaTime * gravityInput);
            }

        }


        private bool CheckGrounded()
        {
            if(groundCheck != null)
            {
                bool onGround = Physics.CheckSphere(groundCheck.position, groundCheckHeight, ground);

                //print(ground);
                return onGround;    
            }
            else
            {
                Debug.Log("Please add groundcheck gameObject to ref");
                return false;
            }
        }


        public Vector2 GetMovement()
        {
            Vector2 movement  = new Vector2 (horizontalInput,verticalInput);

            return movement;
        }




    }

}

