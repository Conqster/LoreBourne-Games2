using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoreBourne;

public class EndReload : MonoBehaviour
{
    private bool reloaded;
    [SerializeField] private Gun gun;

    public void Reloaded()
    {
        reloaded = true;
        if (reloaded)
            gun.ReloadingOver();
    }
}
