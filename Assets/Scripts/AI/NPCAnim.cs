using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ConqsterAI;
using LoreBourne.AI;


public class NPCAnim : MonoBehaviour
{

    Animator animator;
    NavMeshAgent agent;
    NPCBehaviour npcBehaviour;

    public bool shoot;
    [SerializeField] private bool reloadWeapon;

    private int moveSpeedHash, reloadHash, shootHash;

    [SerializeField] private KeyCode testKey;

    private BehaviourSkill npcState;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        npcBehaviour = GetComponent<NPCBehaviour>();

        SetHash();
    }



    private void SetHash()
    {
        moveSpeedHash = Animator.StringToHash("moveSpeed");
        reloadHash = Animator.StringToHash("reload");
        shootHash = Animator.StringToHash("shoot");


    }




    // Update is called once per frame
    void Update()
    {

        GetNPCState();
        MovementLayer();



        TestingReloadAnimation();


        //SetParameters();

        animator.SetFloat(moveSpeedHash, agent.velocity.magnitude);
        //print("agent Speed: " + agent.velocity.magnitude);


        animator.SetBool(reloadHash, reloadWeapon);
        //animator.SetBool("reload", reloadWeapon);

        animator.SetBool(shootHash, shoot);
    }

    private void ShootAtPlayer()
    {
        if(shoot)
        {
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f)); 
        }
        else
        {
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }
    }


    private void GetNPCState()
    {
        npcState = npcBehaviour.NPCState();
    }


    private void MovementLayer()
    {
        switch(npcState)
        {
            case BehaviourSkill.patrolSkill:
                animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
                animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 1f, Time.deltaTime * 10f));
                break;
            case BehaviourSkill.combatSkill:
                animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 0f, Time.deltaTime * 10f));
                animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
                break;
        }
    }



    
    private void TestingReloadAnimation()
    {
        if(Input.GetKeyDown(testKey) && reloadWeapon)
        {
            print("I am trying to reload using Space Bar");
            TriggerReload();
        }
    }





    /// <summary>
    /// Function used to reload weapon
    /// </summary>
    public void ReloadAnimation()
    {
        reloadWeapon = true;
    }


    public void TriggerReload()
    {
        // may need a if statement to prevent reload when weapon is reloading 
        reloadWeapon = true;
        animator.SetTrigger(reloadHash);
    }


    public bool IsWeaponReloading()
    {
        return reloadWeapon;
    }


    /// <summary>
    /// Function used to confirm reloading is over
    /// </summary>
    public void Reloaded()
    {
        reloadWeapon = false;
    }
}
