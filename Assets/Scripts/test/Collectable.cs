using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoreBourne;

public class Collectable : MonoBehaviour
{
    //ref to player inventory 
    //ref to manager 
    [SerializeField] Transform target;
    [SerializeField, Range(0,10)] protected  float timeToSelfDestory;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == target.transform)
        {
            GiveItem();
        }
    }


    protected void GiveItem()
    {
        Invoke("Disable", timeToSelfDestory); 
    }

    private void DisableMe()
    {
        Destroy(gameObject);
    }

}
