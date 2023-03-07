using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBaseState : StateMachineBehaviour
{
    private PlayerBehaviour player;

    public PlayerBehaviour GetPlayer(Animator animator)
    {

        if(player == null)
        {
            player = animator.GetComponent<PlayerBehaviour>();
        }

        return player;

    }

}
