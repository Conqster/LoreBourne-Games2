using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace ConqsterAI
{

    public enum BehaviourSkill
    {
        patrolSkill,
        searchSkill,
        combatSkill,
        ambushSkill,
        defendSkill,
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



