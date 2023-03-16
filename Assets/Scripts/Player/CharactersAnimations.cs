using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoreBourne;

public class CharactersAnimations : MonoBehaviour
{


    //[SerializeField] private GameObject character;   //character the animation is been applied to
    //going to refactories this varaiable 
    [SerializeField] private PlayerMovement player;
    private Vector2 movementVector;                  //stores the magnitude and direction of movement 

    private Animator animator;

    private int moveXHash, moveYHash;   //to make sure things are optimaied, string costs more 



    private void Start()
    {
        animator = GetComponent<Animator>();

        SetHash();
    }

    private void Update()
    {
        GetValues();
        AnimateCharacter();
    }


    private void GetValues()
    {
        movementVector = player.GetMovement();
    }


    private void AnimateCharacter()
    {
        if (animator != null)
        {
            animator.SetFloat(moveXHash, movementVector.x, 0.1f, Time.deltaTime);
            animator.SetFloat(moveYHash, movementVector.y, 0.1f, Time.deltaTime);
        }
    }


    private void SetHash()
    {
        moveXHash = Animator.StringToHash("XMovement");
        moveYHash = Animator.StringToHash("YMovement");
    }
}
