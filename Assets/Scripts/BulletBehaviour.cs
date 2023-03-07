using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [HideInInspector] public float damage;
    [HideInInspector] public GameObject target;


    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.tag == "Player")
        {
            print("trying to deal damge to player");
        }


        Destroy(gameObject);
    }


}
