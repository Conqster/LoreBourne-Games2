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

        protected virtual void RecieveDamage(DealDamage dmg)
        {
            health -= dmg.damage;

            if (health < 0)
                Death();
            else
                Hurt();
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

    }
}

