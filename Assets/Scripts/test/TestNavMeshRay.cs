using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestNavMeshRay : MonoBehaviour
{
    [SerializeField] private Transform target;

    void Update()
    {
        NavMeshHit hit;
        bool blocked;

        blocked = NavMesh.Raycast(transform.position, target.position, out hit, NavMesh.AllAreas);

        Debug.DrawLine(transform.position, target.position, blocked ? Color.red : Color.blue);

        if (blocked)
            Debug.DrawRay(hit.position, Vector3.up, Color.red);
    }
}
