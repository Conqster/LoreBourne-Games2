using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConqsterAI;

[RequireComponent(typeof(NPCBehaviour))]
public class NPCVision : MonoBehaviour
{

    private NPCBehaviour myBehaviour;
    [SerializeField] Transform head;

    [Header("Vision")]
    [SerializeField, Range(45, 360)] private int maxScanAngle = 90;
    private float currentSightAngle;
    [SerializeField] bool clampAngle;
    [SerializeField, Range(1f, 500f)] private float scanSpeed = 10f;
    [SerializeField, Range(10, 100)] private float sightDistance = 20f;
    private Vector3 raycastDirection;

    private bool playerInSight;

    // For testing
    private float scanStart;

    private void Start()
    {
        myBehaviour = GetComponent<NPCBehaviour>();


        //FOR TEST TEST !!!!!
        scanStart = currentSightAngle;

    }


    private void Update()
    {
        if (clampAngle)
            maxScanAngle = ClampAngleOfVision(maxScanAngle);

        Observe(); 
    }


    private void Observe()
    {
        currentSightAngle += scanSpeed * Time.deltaTime;
        currentSightAngle = currentSightAngle % maxScanAngle;

        float angle = (currentSightAngle * 2) - maxScanAngle;
        raycastDirection = head.TransformDirection(Quaternion.Euler(0, angle, 0) * Vector3.forward * sightDistance);

        
        RaycastHit hit;
        if(Physics.Raycast(head.position, raycastDirection, out hit, sightDistance) && hit.collider.tag == "Player")
        {
            Debug.DrawRay(head.position, raycastDirection, Color.red);
            //return hit.transform.gameObject;
            playerInSight = true;
        }
        else
        {
            Debug.DrawRay(head.position, raycastDirection, Color.green);
            //return null;
            playerInSight = false;
        }
    }


    public bool PlayerInSight()
    {
        return playerInSight;
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

}
