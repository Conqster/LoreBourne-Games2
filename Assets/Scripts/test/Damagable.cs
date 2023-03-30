using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;


namespace LoreBourne
{
    public class Damagable : MonoBehaviour 
    {
        [Header("Health System")]
        [SerializeField, Range(0,30)] protected float health = 20;
        [SerializeField, Range(0, 30)] protected float fullHealth = 20;
        [SerializeField] UISystem uiSystem;

        protected virtual void RecieveDamage(DealDamage dmg)
        {
            health -= dmg.damage;

            if (health < 0)
                Death();
            else
                Hurt();
        }

        protected virtual void ReceivePickUp(TakePickUp pickUp)
        {
            //print("testing");
            if (pickUp.type == PickUpType.health)
            {
                health += pickUp.value;
                UpdateUI();
                print("health potion is been picked Up : " + pickUp.value);
            }
        }

        protected virtual void Death()
        {
            if(!gameObject.CompareTag("Player"))
                Destroy(gameObject);
        }

        protected virtual void Hurt()
        {
            //do something 
            // do Specific sound
        }

        protected virtual void UpdateUI()
        {
            uiSystem.HealthGuage(health, fullHealth);
        }

    }
}

