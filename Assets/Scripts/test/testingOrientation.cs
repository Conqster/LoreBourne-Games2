using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testingOrientation : MonoBehaviour
{

    [SerializeField] private Transform player;

    private void Update()
    {


        transform.LookAt(player, Vector3.left);

    }
}
