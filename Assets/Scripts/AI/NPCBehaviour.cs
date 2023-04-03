using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConqsterAI;
using LoreBourne;
using LoreBourne.AI;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(NPCVision))]
[RequireComponent(typeof(NavMeshAgent))]
public class NPCBehaviour : Damagable
{
    private NavMeshAgent npc;
    
    
    [Header("Behaviour")]
    [SerializeField] private BehaviourSkill npcBehaviour;
    [SerializeField] private Action npcAction;
    private NPCVision npcVision;
    private GameObject player;


    [Space][Space][Header("MovementSpeed")]
    [SerializeField, Range(1, 10)] private float partolSpeed = 2.5f;
    [SerializeField, Range(1, 10)] private float searchMoveSpeed = 2f;
    [SerializeField, Range(1, 10)] private float combatSpeed = 3f;
    [SerializeField, Range(1, 10)] private float ambushSpeed = 3.5f;
    [SerializeField, Range(1, 10)] private float defendSpeed = 1f;

    [Space][Space][Header("Waypoints")]
    [SerializeField, Range(0, 5)] private float reachedWaypointRange = 1.5f;
    [SerializeField] private Transform patrolRoute;
    [SerializeField] private List<Transform> waypoints;
    private int currentWaypoint;

    [Space][Space][Header("Durations")]
    [SerializeField] private float searchLocationSpeed;
    [SerializeField, Range(0f,5f), Min(0.1f)] private float patrolIdleDuration = 1f;
    private float idleTimer = 0f;

    [Space][Space][Header("Combat")]
    [SerializeField] private bool shoot;
    [SerializeField, Range(0.1f, 50f)] private float shootingDistance = 1f;
    private WeaponBehaviour weapon;
    [SerializeField, Range(0.5f, 5f)] private float delayStartWalking = 0.5f;


    [Space][Space][Header("TEST TEST !!!!!")]
    [SerializeField] private bool trigger;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        weapon = GetComponent<WeaponBehaviour>();
        npc = GetComponent<NavMeshAgent>();
        npcVision = GetComponent<NPCVision>();
        npcAction = Action.walk;                                //defaults npc action to idle

        SetPatrolRoute(patrolRoute);

    }

    private void Update()
    {
        NPCBehaviourState();

        if (npcVision.PlayerInSight())
            npcBehaviour = BehaviourSkill.combatSkill;

        

        //if(trigger)
        //{
        //    IdleAction(Action.walk, patrolIdleDuration);
        //}
    }

    private void NPCBehaviourState()
    {
        switch(npcBehaviour)
        {
            case BehaviourSkill.patrolSkill:
                weapon.TryToShot(false);                           // here for testing 
                //in patrol, walks between waypoints 
                // if waypoints is null npc remains idle 
                // 
                npc.speed = partolSpeed;
                npc.stoppingDistance = 0f;
                if(waypoints != null)
                {
                    if(npc.remainingDistance < reachedWaypointRange)
                    {
                        StartIdle(patrolIdleDuration);
                        MoveToNextWaypoint(currentWaypoint);
                    }
                }
                else
                {
                    //remain in idle and request for destnation
                }

                if(npcAction == Action.idle)
                {
                    //triggers an idle duration to get into walking action
                    npc.speed = 0f;
                    npc.stoppingDistance = 0f;
                    IdleDuration(Action.walk, patrolIdleDuration);

                }

                break;
            case BehaviourSkill.searchSkill:
                npc.speed = searchMoveSpeed;
                //in search, 
                // walks in the search zones for player
                // or walks to last known location
                // when at last know location npc waits few seconds 
                break;
            case BehaviourSkill.combatSkill:
                //npc is alert and aware of player woundnt shoot unilt at shooting distance
                //in combat,
                npc.speed = combatSpeed;
                npc.SetDestination(player.transform.position);
                npc.stoppingDistance = shootingDistance;
                RequestToShoot();
                //npc shoots at player if in shoot action
                if(npcAction == Action.shoot)
                {
                    weapon.TryToShot(true);
                }
                else if(npcAction == Action.walk)
                {
                    weapon.TryToShot(false);
                }
                else if (npcAction == Action.idle)
                {
                    weapon.TryToShot(false);
                }


                //in grenade action, throws grenade at player
                //in reposition, looks for the best position to move to 
                //in cover, moves towards cover
                // goes to idle, if not shooting and if taking damages moves to cover
                //if in close range with player, does melee attack
                break;
            case BehaviourSkill.ambushSkill:
                npc.speed = ambushSpeed;
                //in walk, moves to ambush point
                // if at ambush point, cover/idle
                // if allowed to shoot, shoots from cover/ambush point
                //repositions is needs new ambush point
                break;
            case BehaviourSkill.defendSkill:
                npc.speed = defendSpeed;
                break;
        }
    }




    private void StartIdle(float duration)
    {
        npcAction = Action.idle;
        switch(npcBehaviour)
        {
            case BehaviourSkill.patrolSkill:
                patrolIdleDuration = duration;
                break;
        }
    }

    /// <summary>
    /// a function to check how many shooters in scene to 
    /// detmine if current NPC can shoot
    /// </summary>
    private void RequestToShoot()
    {
        // get request
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= shootingDistance)
        {
            npcAction = Action.shoot;
        }
        else
        {
            Invoke("StartWalking", delayStartWalking);
        }

    }

    private void IdleDuration(Action currentAction, float duraion)
    {
        idleTimer += Time.deltaTime;
        if (idleTimer > duraion)        // would want to modify duration of idleness 
        {
            idleTimer = 0;
            npcAction = currentAction;
        }
        //print(idleTimer);
    }



    /// <summary>
    /// Funtction to set waypoints for patrol
    /// </summary>
    /// <param name="patrolRoute"></param>
    private void SetPatrolRoute(Transform patrolRoute)
    {
        foreach (Transform child in patrolRoute)
        {
            waypoints.Add(child);
        }
    }

    private void MoveToNextWaypoint(int current)
    {
        if (current >= waypoints.Count)
            current = 0;

        //print(current);
        npc.SetDestination(waypoints[current].position);
        current++;
        currentWaypoint = current;
    }


    private void StartWalking()
    {
        npcAction = Action.walk;
    }

    public bool WantToShoot()
    {
        return shoot;
    }


    public BehaviourSkill NPCState()
    {
        return npcBehaviour;
    }

    public Action NPCCurrentAction()
    {
        return npcAction;
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(npcBehaviour == BehaviourSkill.combatSkill)
        {
            if(player != null)
            {
                Handles.color = Color.blue;
                Handles.DrawWireDisc(player.transform.position - Vector3.up, Vector3.up, shootingDistance);
            }
        }
    }

#endif
}
