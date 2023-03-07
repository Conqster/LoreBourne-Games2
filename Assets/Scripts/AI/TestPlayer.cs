using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class TestPlayer : MonoBehaviour
{
    private NavMeshAgent player;
    [SerializeField] private Camera mainCam;


    private void Start()
    {
        player = GetComponent<NavMeshAgent>();
    }


    private void Update()
    {
        ModifyPlayerPosition();
    }

    private void ModifyPlayerPosition()
    {
        if(Input.GetMouseButton(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                player.SetDestination(hit.point);
            }
        }
    }
}
