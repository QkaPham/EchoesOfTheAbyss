using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : State
{
    public float WalkSpeed => player.stats.WalkSpeed;
    public Walk(StateMachine stateMachine, Animator animator, Player player) : base(stateMachine, animator, player)
    {
        
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        animator.SetInteger("State", (int)PlayerState.Walk);
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        player.SetVelocity(InputManager.Instance.MoveDir * player.stats.WalkSpeed);
    }
}

