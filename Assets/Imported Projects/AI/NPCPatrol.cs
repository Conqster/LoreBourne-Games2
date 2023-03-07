using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;


public class NPCPatrol : MonoBehaviour
{

    enum AIstate { patrol, chase, pause};
     AIstate aIstate = AIstate.patrol;



    [SerializeField] GameObject[] waypoints;
    //private GameObject wayObjects;
    NavMeshAgent agent;
    private float reachTargetRange = 2f;
    [SerializeField] private int currentWaypoint = 0;

    private float currentSightAngle = 0f;
    [SerializeField, Range(45,180)] private int maxscanAngle = 90;
    [SerializeField] bool clampAngle;
    [SerializeField, Range(1f, 500f)] private float scanSpeed = 10f;
    [SerializeField, Range(10,100)] private float sightDistance = 20f;
    private Vector3 raycastDirection;


    private GameObject playerTarget;
    private float alertMeter = 0f;
    private float alertTarget = 1f;

    private float pauseTimer = 0f;
    private float pauseDuration;

    private float patrolSpeed = 2f;
    private float chaseSpeed = 4f;


    #region UiTextDebugger
    [Space]
    [Space]
    [Header("UiTextDebugger")]
    [SerializeField] private TextMeshProUGUI _aiState;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if(aIstate == AIstate.patrol)
        {
            //StartingWaypoint();
            StartPatrolling();
        }
        //GoToNextWaypint();
    }

    private void StartingWaypoint()
    {
       currentWaypoint = Random.Range(0, waypoints.Length);
    }



    //i have commented out the chase logic  so the ai wouldnt chase after the player on sight
    // Update is called once per frame
    void Update()
    {
        if (clampAngle)
            maxscanAngle = ClampAngleOfVision(maxscanAngle);

        if (aIstate == AIstate.patrol)
        {

            if(agent.remainingDistance < reachTargetRange)
            {
                GoToNextWaypint();
            }

        }
        else if(aIstate == AIstate.chase)
        {
            //Set the agent destination towards player
            agent.SetDestination(playerTarget.transform.position);
            agent.stoppingDistance = 2.5f;
        }
        else if(aIstate == AIstate.pause)
        {
            TakePause();
        }



        VisionScan();

        if(_aiState != null)
            UIDebugger();
    }


    private void GoToNextWaypint()
    {
        currentWaypoint++;
        if(currentWaypoint >= waypoints.Length)
            currentWaypoint = 0;
        agent.SetDestination(waypoints[currentWaypoint].transform.position);
    }

  

    private void VisionScan()
    {
        //make current sight angle change 
        currentSightAngle += scanSpeed * Time.deltaTime;
        currentSightAngle = currentSightAngle % maxscanAngle;

        float angle = (currentSightAngle * 2) - maxscanAngle;
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

        if(alertMeter >= alertTarget)
        {
            //start chase
            Startchasing(); 
        }
        else if(aIstate == AIstate.chase && alertMeter <= 0)
        {
            //resume patrolling
            //StartPatrolling();
            agent.ResetPath();
            aIstate = AIstate.pause;
        }
    }



    private void Startchasing()
    {
        agent.speed = chaseSpeed;
        aIstate = AIstate.chase;
    }

    private void StartPatrolling()
    {
        agent.speed = patrolSpeed;
        aIstate = AIstate.patrol;
    }

    private void TakePause()
    {
        pauseTimer += Time.deltaTime;
        if(pauseTimer > pauseDuration)
        {
            pauseTimer = 0;
            StartPatrolling();
        }
    }


    private int ClampAngleOfVision(int visionAngle)
    {

        int[] angles = new int[5] {0, 45, 90, 135, 180};

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

    private void UIDebugger()
    {
        _aiState.text = "AI STATE: " + aIstate.ToString() // for ai state
                    + "\n alert meter: " + alertMeter.ToString("0.00"); // alert meter
    }
}
