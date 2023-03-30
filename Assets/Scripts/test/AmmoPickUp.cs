using LoreBourne;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AmmoPickUp : Collectable
{

    private PlayerWeaponBehaviour weapon;

    


    protected override void Start()
    {
        base.Start();
        weapon = target.GetComponent<PlayerWeaponBehaviour>();
    }


    protected override void GiveItem()
    {
        //weapon.UpdateAmmo(value);
        base.GiveItem();
    }


    
}
