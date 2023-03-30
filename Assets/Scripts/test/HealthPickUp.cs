using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoreBourne;

public class HealthPickUp : Collectable
{
    //private PlayerBehaviour player;


    protected override void Start()
    {
        base.Start();
        //player = target.GetComponent<PlayerBehaviour>();
    }


    protected override void GiveItem()
    {
        //player.UpdateAmmo(value);
        base.GiveItem();
    }

    
}
