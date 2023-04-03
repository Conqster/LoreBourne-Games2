using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoreBourne;
using LoreBourne.AI;
using UnityEngine.AI;
using UnityEditor;



[RequireComponent(typeof(NPCVision))]
[RequireComponent(typeof(NavMeshAgent))]
public class AiLocomotion : MonoBehaviour, ICharacter
{
    private NavMeshAgent agent;
    [Header("Behaviour")]
    [SerializeField] private BehaviourSkill npcBehaviour;
    private NPCVision npcVision;
    [SerializeField] private BehaviourSkill previousBehaviour;

    [Space] [Space] [Header("Movement")]
    [SerializeField, Range(1, 10)] private float patrolSpeed = 2.5f;
    [SerializeField, Range(1, 10)] private float combatSpeed = 2;
    [SerializeField, Range(1, 10)] private float searchMoveSpeed = 3f;

    [Space][Space][Header("Patrol waypoints")]
    [SerializeField, Range(1,5)] private float reachedWaypointRange = 1.5f;
    [SerializeField] private Transform patrolRoute;
    [SerializeField] private List<Transform> waypoints;
    private int currentWaypoint;

    [Space][Space][Header("Durations")]
    [SerializeField, Range(0f, 5f), Min(0.1f)] private float patrolIdleDuration = 1f;
    [SerializeField] private float duration;
    [SerializeField] private float idleTimer;
    

    [Space][Space][Header("Combat")]
    [SerializeField, Range(0.1f, 10f)] private float shootingDistance = 1f;
    [SerializeField, Range(0.1f, 10f)] private float maxAlertMeter;
    [SerializeField] private bool aim;
    [SerializeField] private bool shoot;
    private bool engageTarget;

    private NPCAnimation animate;
    private Transform target;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        npcVision = GetComponent<NPCVision>();
        animate = GetComponent<NPCAnimation>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        //hard code might change later
        npcBehaviour = BehaviourSkill.idleSkill;
        previousBehaviour = BehaviourSkill.patrolSkill;
        SetPatrolRoute(patrolRoute);
    }

    private void Update()
    {
        CheckTargetInSight();
        Behaviour();
        //print(engageTarget + "& player in Sight " + npcVision.PlayerInSight());
    }


    private void Behaviour()
    {
        switch(npcBehaviour)
        {
            case BehaviourSkill.patrolSkill:
                agent.speed = patrolSpeed;
                agent.stoppingDistance = 0f;
                aim = false;

                if (waypoints != null)
                {
                    if (agent.remainingDistance < reachedWaypointRange)
                    {
                        GoIdle(npcBehaviour);
                        MoveToNextWaypoint(currentWaypoint);
                    }
                }
                break;
            case BehaviourSkill.combatSkill:

                agent.stoppingDistance = shootingDistance;
                CheckDistanceAimTarget();
                bool blocked;
                
                if(aim)
                {
                    NavMeshHit hit;
                    blocked = NavMesh.Raycast(transform.position, target.position, out hit, NavMesh.AllAreas);
                    Debug.DrawLine(transform.position + Vector3.up, target.position, blocked ? Color.red : Color.blue);
                    if (blocked)
                        Debug.DrawRay(hit.position, Vector3.up, Color.red);

                    if (!blocked)
                    {
                        ShouldNpcShoot();
                    }
                    else
                    {
                        //going to add more conditions 
                        shoot = false;
                    }
                }

                break;
            case BehaviourSkill.searchSkill:
                aim = false;
                break;
            case BehaviourSkill.idleSkill:
                agent.speed = 0f;
                agent.stoppingDistance = 0f;
                IdleDuration();
                break;
        }
        animate.AimingWeaponLayer(aim);         //might want to change into combat switch case
        animate.MovementAnimation(agent.speed);
    }

    private void GoIdle(BehaviourSkill currentBehaviour)
    {
        previousBehaviour = currentBehaviour;
        npcBehaviour = BehaviourSkill.idleSkill;
    }

    private void IdleDuration()
    {
        switch (previousBehaviour)
        {
            case BehaviourSkill.patrolSkill:
                duration = patrolIdleDuration;
                break;
            case BehaviourSkill.combatSkill:
                break;
            case BehaviourSkill.searchSkill:
                break;
            case BehaviourSkill.idleSkill:
                previousBehaviour = BehaviourSkill.patrolSkill;
                break;
        }
        idleTimer += Time.deltaTime;
        if (idleTimer > duration)        // would want to modify duration of idleness 
        {
            idleTimer = 0;
            npcBehaviour = previousBehaviour;   // to set it back to the behavioutr it was before going idle
        }
    }

    private void MoveToNextWaypoint(int current)
    {
        if (current >= waypoints.Count)
            current = 0;

        //print(current);
        agent.SetDestination(waypoints[current].position);
        current++;
        currentWaypoint = current;
    }



    private void CheckTargetInSight()
    {
        engageTarget = npcVision.FullAlert(maxAlertMeter, npcBehaviour);

        if(engageTarget)
        {
            if (npcBehaviour != BehaviourSkill.combatSkill)
            {
               previousBehaviour = npcBehaviour;
            }
            npcBehaviour = BehaviourSkill.combatSkill;
        }
        else if(npcBehaviour == BehaviourSkill.combatSkill && !engageTarget)
        {
            agent.ResetPath();
            npcBehaviour = previousBehaviour;
        }

    }


    private void ShouldNpcShoot()
    {
        //go to add an algorithm to check if other npcs are shoot 
        //so not all npcs will be shooting a time 
        shoot = true;
    }


    private void SetPatrolRoute(Transform route)
    {
        foreach (Transform child in patrolRoute)
        {
            waypoints.Add(child);
        }
    }
    private void CheckDistanceAimTarget()
    {
        agent.speed = combatSpeed;
        float distance;
        distance = Vector3.Distance(transform.position, target.position);
        agent.SetDestination(target.transform.position);
        print("distance is " + distance + "and stopping distance is " + agent.stoppingDistance);
        
        if((distance - shootingDistance) <= 1)
        {
            agent.speed = 0f;
            aim = true;
        }

       
    }

    public bool IsAiming()
    {
        return aim;
    }

    public bool ShootConfirm()
    {
        return shoot;
    }


    private void OnDrawGizmos()
    {
        if (npcBehaviour == BehaviourSkill.combatSkill)
        {
            if (target != null)
            {
                Handles.color = Color.blue;
                Handles.DrawWireDisc(transform.position - Vector3.up, Vector3.up, shootingDistance);
            }
        }
    }

}
