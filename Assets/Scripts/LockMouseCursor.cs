using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockMouseCursor : MonoBehaviour
{
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

}
