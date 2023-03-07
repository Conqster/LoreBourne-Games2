using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ConqsterAI1;

public class ZombieAnimation : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;
    ZombieBehaviour zombie;

    enum animateWith { velocity , speed};
    [SerializeField] private animateWith _animWith;

    [SerializeField] private float velocity;
    private int attackHash, moveSpeedHash, moveHash;

    private void Start()
    {
        animator = GetComponent<Animator>();   
        agent = GetComponent<NavMeshAgent>();
        zombie = GetComponent<ZombieBehaviour>();

        SetAnimationHash();
    }

    private void SetAnimationHash()
    {
        moveSpeedHash = Animator.StringToHash("moveSpeed");
        moveHash = Animator.StringToHash("moving");
        attackHash = Animator.StringToHash("attacking");
    }

    private void Update()
    {

        MovementAnim();
        AttackAnim();
    }


    private void MovementAnim()
    {
        velocity = agent.velocity.magnitude;

        switch (_animWith)
        {
            case animateWith.velocity:
                animator.SetFloat(moveSpeedHash, agent.velocity.magnitude);
                break;
            case animateWith.speed:
                if (agent.speed > 0)
                    animator.SetFloat(moveSpeedHash, 1f);
                else if (agent.speed == 0)
                    animator.SetFloat(moveSpeedHash, 0f);
                break;

        }

        if (velocity <= 0) 
            animator.SetBool(moveHash, false);
        else
            animator.SetBool(moveHash, true);
    }

    private void AttackAnim()
    {
        bool attack = zombie.ZombieAttacking();

        animator.SetBool(attackHash, attack);
    }


}
