using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace LoreBourne.AI
{

    public enum BehaviourSkill
    {
        patrolSkill,
        searchSkill,
        combatSkill,
        ambushSkill,
        defendSkill,
        idleSkill,
    };


    public enum Action
    {
        walk,
        idle, 
        shoot, 
        throwGrenade,
        flank, 
        reposition,
        cover,
        melee,
    };


    public class AIDirector : MonoBehaviour
    {


    }
}



