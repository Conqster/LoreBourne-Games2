using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class DebugWaypoints : MonoBehaviour
{
    [SerializeField] private Transform patrolRoute;
    [SerializeField, Range(0f, 5f), Min(0.000001f)] private float debugRadius = 1f;


    



    private void OnDrawGizmos()
    {
        foreach(Transform child in patrolRoute)
        {
            Gizmos.color = Color.green;

            Gizmos.DrawWireSphere(child.position, debugRadius);
        }

    }




}
