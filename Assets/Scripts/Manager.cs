using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoreBourne;

public class Manager : MonoBehaviour
{
    public static Manager instance;


    


    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
