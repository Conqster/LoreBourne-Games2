using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using Cinemachine;



namespace ConqsterAI1
{
    enum BehaviourState { idle, chase, attack, wander, patrol};

    //[RequireComponent(typeof(Sight))]
    public class ZombieBehaviour : MonoBehaviour
    {
        [SerializeField] private BehaviourState behaviourState = BehaviourState.idle;

        private NavMeshAgent agent;

        private GameObject playerTarget;
        private float alertMeter = 0f;
        [SerializeField] private float alertTarget = 1f;
        [SerializeField]private float idleTimer = 0;
        [SerializeField] private float idleDuration;


        [Space][Space][Header("Way points")]
        [SerializeField, Range(0,5)] public float reachTargetRange = 2f;
        [SerializeField] private Transform patrolRoute;
        [SerializeField] private List<Transform> locations;
        private int currentWaypoint = 0;

        private float currentSightAngle = 0f;
        [Space][Space][Header("Sight")]
        [SerializeField, Range(45, 360)] private int maxScanAngle = 90;
        [SerializeField] bool clampAngle;
        [SerializeField, Range(1f, 500f)] private float scanSpeed = 10f;
        [SerializeField, Range(10, 100)] private float sightDistance = 20f;
        private Vector3 raycastDirection;

        [Space][Space][Header("Movement")]
        [SerializeField, Range(1, 10)] private float wanderSpeed;
        [SerializeField, Range(1, 10)] private float chaseSpeed;
        [SerializeField, Range(1, 10)] private float patrolSpeed;
        [SerializeField, Range(0, 5), Min(0.001f)] private float movementIdle = 2f;
        [SerializeField] private bool deranged;                //not implememted yet going to use to make zombie remain in spot for a longer period of time 
        [SerializeField, Range(0, 20), Min(0.001f)] private float derangedIdle = 7f;


        [Space] [Space]
        [Header("Attack")]
        [Tooltip("DEBUG Red Gizmos")]
        [SerializeField, Range(0.1f, 5f)] private float attackRange;
        [Tooltip("DEBUG Blue Gizmos, keep distance is the distance from target for attack, and should be greater that attack range")]
        [SerializeField, Range(0.1f, 4f)] private float keepDistanceForAttack;
        [SerializeField, Range(0, 10f)] private float attackSpeed;

    #region Testing variables 
        [Space][Space][Header("Tesing")]
        [SerializeField] private GameObject _tTarget;
        #endregion


        [SerializeField] private CinemachineVirtualCamera cam2;




        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            IntializePatrolRoute();
            idleDuration = movementIdle;                // intializes the duration of idle

            if(behaviourState == BehaviourState.idle)
            {
                StartPatrolling();
            }
        }

        /// <summary>
        /// Function to return attacking state to external entity
        /// </summary>
        /// <returns>Zombie Behaviour, if attacking</returns>
        public bool ZombieAttacking()
        {
            if (behaviourState == BehaviourState.attack)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Might want to modify later to support when patrol changes procedurally 
        /// </summary>
        private void IntializePatrolRoute()
        {
            foreach(Transform child in patrolRoute)
            {
                locations.Add(child);
            }
        }

        private void GoToNextLocation()
        {
            currentWaypoint++;
            if(currentWaypoint >= locations.Count)
                currentWaypoint = 0;
            agent.SetDestination(locations[currentWaypoint].position);
        }


        private void Update()
        {
            if(playerTarget != null)
                PromotionVideoCam();


            if(clampAngle)
                maxScanAngle = ClampAngleOfVision(maxScanAngle);

            if (deranged)
                StartIdle(derangedIdle);


            if(playerTarget != null)
            {
                float distanceToTarget = Vector3.Distance(playerTarget.transform.position, transform.position);
                //print(distanceToTarget);
                if (distanceToTarget < attackRange)
                    StartAttack();
                else
                {
                    if(alertMeter != 0)
                        StartChasing();
                
                    //will change to wandering/ maybe a condition


                }
                print(distanceToTarget);
            }

            //CheckZombieState();
            Observe();
            CheckState();



        }


        public void PromotionVideoCam()
        {

            float distanceToTarget = Vector3.Distance(playerTarget.transform.position, transform.position);
            if (distanceToTarget < attackRange)
                cam2.Priority = 50;

        }

        #region CheckState

        private void CheckState()
        {
            //properly going to need a boolean to know if 
            //ai is jumping through offmeshlink to play jump animation 

            switch(behaviourState)
            {
                case BehaviourState.idle:
                    agent.speed = 0;
                    agent.stoppingDistance = 0;
                    agent.ResetPath();           //reset current path to a destination, might want to change the logic
                    IdleDuration();
                    //stand idle in position
                    //can easily get triggered (very sensitive) 
                    break;
                case BehaviourState.attack:
                    // stops at range to player 
                    agent.stoppingDistance = keepDistanceForAttack;
                    // maintains the specific range between player 
                    agent.SetDestination(playerTarget.transform.position);        //but is want to move towards player
                    // triggers attack animation 
                    // set hand gameObject true to deal damage
                    //probaly going to check the distance between enemy hand and players position 
                    //using Vector3.Distance(enemies hand and players position)
                    break;
                case BehaviourState.chase:
                    agent.speed = chaseSpeed;
                    agent.stoppingDistance = keepDistanceForAttack;      // used to create a distance between player and the agent 
                    //Triggers when player is in sight 
                    //update every frame to player location so far player is still in sight
                    agent.SetDestination(playerTarget.transform.position);
                    //when player is out of sight use last known location (~ set to wander toward last known) 
                    break;
                case BehaviourState.wander:
                    agent.speed = wanderSpeed;
                    agent.stoppingDistance = 0f;
                    //Triggers when not chasing or just finished chasing
                    //used for special action
                    // when alert and player is not in sight, wanders to the disturbed location 
                    // for player last known location
                    // to move AI to a specific location
                    break;
                case BehaviourState.patrol:
                    // moves along waypoint for a given path
                    agent.speed = patrolSpeed;
                    agent.stoppingDistance = 0f;
                    if(locations != null)
                    {
                        if (agent.remainingDistance < reachTargetRange)
                        {
                            GoToNextLocation();
                        }
                    }
                    break;
            }
        }


    #endregion
        private void Observe()
        {
            //make current sight angle change 
            currentSightAngle += scanSpeed * Time.deltaTime;
            currentSightAngle = currentSightAngle % maxScanAngle;

            float angle = (currentSightAngle * 2) - maxScanAngle;
            raycastDirection = transform.TransformDirection(Quaternion.Euler(0, angle, 0) * Vector3.forward)
                                                                                            * sightDistance;
            Vector3 offset = Vector3.up;

            RaycastHit hit;
            if (Physics.Raycast(transform.position + offset, raycastDirection, out hit, sightDistance) && hit.collider.tag == "Player")
            {
                Debug.DrawRay(transform.position + offset, raycastDirection, Color.red);
                playerTarget = hit.transform.gameObject;
                //Debug.Log(hit);
                alertMeter = Mathf.Clamp(alertMeter + (30f * Time.deltaTime), 0, alertTarget);
            }
            else
            {
                Debug.DrawRay(transform.position + offset, raycastDirection, Color.green);
                alertMeter = Mathf.Clamp(alertMeter - (0.4f * Time.deltaTime), 0, alertTarget);
            }
            //print("alert meter: " +  alertMeter);

            if (alertMeter >= alertTarget)
            {
                //start chase
                //StartChasing();
                if (behaviourState != BehaviourState.attack)
                    StartChasing();

                //going into chase mode
                //behaviourState = BehaviourState.chase;

            }
            else if (behaviourState == BehaviourState.chase && alertMeter <= 0)
            {
                //resume patrolling
                //StartPatrolling();
                //agent.ResetPath();
                behaviourState = BehaviourState.idle;
                StartIdle(movementIdle);
            }

        }




        /// <summary>
        /// function to start idle and duration of idleness
        /// </summary>
        /// <param name="value"></param>
        private void StartIdle(float duration)
        {
            idleDuration = duration;
            behaviourState = BehaviourState.idle;
        }

        private void IdleDuration()
        {
            idleTimer += Time.deltaTime;
            if(idleTimer > idleDuration)        // would want to modify duration of idleness 
            {
                idleTimer = 0;
                StartPatrolling();
            }
        }

        private void StartPatrolling()
        {
            behaviourState = BehaviourState.patrol; 
        }

        private void StartChasing()
        {
            behaviourState = BehaviourState.chase;
        }

        private void StartWandering()
        {
            behaviourState = BehaviourState.wander;
        }

        private void StartAttack()
        {
            behaviourState = BehaviourState.attack;
        }

        private int ClampAngleOfVision(int visionAngle)
        {

            int[] angles = new int[9] { 0, 45, 90, 135, 180, 225, 270, 315, 360 };

            var closest = int.MaxValue;
            var minDifference = int.MaxValue;

            foreach (var angle in angles)
            {
                var difference = Mathf.Abs((long)angle - visionAngle);
                if (minDifference > difference)
                {
                    minDifference = (int)difference;
                    closest = angle;
                }
            }

            return closest;
        }

        private void OnDrawGizmos()
        {

            Handles.color = Color.blue;
            Handles.DrawWireDisc(transform.position, transform.up, keepDistanceForAttack);

            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, transform.up, attackRange);
        }

        #region    OLD STATECHECK
        private void CheckZombieState()
        {
            switch(behaviourState)
            {
                case BehaviourState.patrol:
                    agent.speed = patrolSpeed;
                    if (agent.remainingDistance < reachTargetRange)
                    {
                        GoToNextLocation();
                    }
                    break;
                case BehaviourState.idle:
                    GoIdle();
                    agent.speed = 0;
                    break;
                case BehaviourState.chase:
                    agent.speed = chaseSpeed;
                    agent.stoppingDistance = 1f;
                    agent.SetDestination(playerTarget.transform.position);
                    break;
                case BehaviourState.wander:
                    agent.speed = wanderSpeed;
                    break;
            }
        }

        #endregion  
        /// <summary>
        /// Obselete Idle function 
        /// </summary>
        private void GoIdle()
        {
            agent.ResetPath();            //TESTING might want to change later
            idleTimer += Time.deltaTime;
            if(idleTimer > idleDuration)
            {
                idleTimer = 0;
                StartPatrolling();  
            }
        }
    }

}

