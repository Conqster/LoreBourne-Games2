using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshRayHitTeasting : MonoBehaviour
{

    [SerializeField, Range(0, 20)] private float rayLength;
    [SerializeField] private bool postRay, move;

    [SerializeField] private Transform target;
    NavMeshAgent agent;
    private NavMeshPath path;
    private float elapsed = 0.0f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        elapsed = 0.0f;
    }


    private void Update()
    {
        //NewPath();
        OldPath();
    }

    private void OldPath()
    {
        //NavMeshPath path = new NavMeshPath();
        postRay = NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
        //Debug.DrawLine(transform.position, target.position, postRay ? Color.red : Color.black);

        for (int i = 0; i < path.corners.Length - 1; i++)
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);

    }
    private void NewPath()
    {
        // Update the way to the goal every second.
        elapsed += Time.deltaTime;
        if (elapsed > 1.0f)
        {
            elapsed -= 1.0f;
            NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
        }
        for (int i = 0; i < path.corners.Length - 1; i++)
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
    }

    private void MoveTowardTarget()
    {
        agent.isStopped = false;
    }
}
