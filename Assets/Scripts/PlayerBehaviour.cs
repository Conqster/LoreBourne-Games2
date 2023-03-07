using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour
{
    private PlayerAnimation playerAnim;

    [Header("Movement")]
    [SerializeField, Range(0, 10)] private float movementSpeed = 5f;
    private Vector3 movementInput;
    private Rigidbody playerRigidbody;
    [SerializeField, Range(0, 100)] private float jumpForce = 50;
    private bool jump;
    [SerializeField] private bool jumping;
    [SerializeField] ForceMode jumpForceMode;

    [Space][Space]
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField, Range(0.1f, 5f)] private float allowDistanceToGround;
    //[SerializeField, Range(-0.1f, -10f)] private float gravity = -9.87f;
    [SerializeField] LayerMask groundedLayer;
    private bool onGround;
    [SerializeField, Range(0, 5)] private float invokeWhenGrounded;

    [Space][Space]
    [Header("Multipilers")]
    [SerializeField, Range(1,10)] private int jumpMultiplier = 2;
    
    



    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnim = GetComponent<PlayerAnimation>();
    }


    private void Update()
    {
        GetPlayerInput();
        ApplyMovement();

        IsGrounded();

        

        if(jump)
            ApplyJump();

        if (jumping && !jump)
            BackOnGround();

        //print(jumping);
    }


    private void GetPlayerInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        jump = Input.GetButtonDown("Jump");

        movementInput = new Vector3(horizontalInput, 0, verticalInput);
    }


    private void ApplyJump()
    {
        onGround = IsGrounded();
        Vector3 upForce = new Vector3(0, jumpForce * jumpMultiplier,0);

        if (onGround && !jumping)   // && !jumping
        {
            jumping = true;
            playerRigidbody.AddForce(upForce, jumpForceMode);
        }
        
    }

    private void BackOnGround()
    {
        if(IsGrounded())
        {
            jumping = false;
            //print("this function has been accessed");
        }
       
    }


    


    private void ApplyMovement()
    {
        if(playerRigidbody != null)
        {
            Vector2 movement = movementInput.normalized;

            playerRigidbody.MovePosition(transform.position + movementInput.normalized * Time.deltaTime * movementSpeed);
        }
    }

    private bool IsGrounded()
    {
        if(groundCheck != null)
        {
            bool onGround = Physics.CheckSphere(groundCheck.position, allowDistanceToGround, groundedLayer);

            if(onGround)
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

        if(physicsVelY < 0)
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

}
