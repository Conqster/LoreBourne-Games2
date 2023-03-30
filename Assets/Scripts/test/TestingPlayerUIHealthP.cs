using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoreBourne;

public class TestingPlayerUIHealthP : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour player;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            TakePickUp pickUp = new TakePickUp();
            pickUp.value = 5;
            pickUp.type = PickUpType.health;
            player.SendMessage("ReceivePickUp", pickUp);

            //DealDamage dmg = new DealDamage();
            //dmg.damage = 30;
            //player.SendMessage("ReceiveDamage", dmg);
        }
    }
}
