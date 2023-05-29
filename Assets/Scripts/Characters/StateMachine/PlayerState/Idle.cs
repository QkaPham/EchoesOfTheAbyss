using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    public Idle(StateMachine stateMachine, Animator animator, Player player) : base(stateMachine, animator, player)
    {
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        animator.SetInteger("State", (int)PlayerAnimatorParameters.Idle);
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        player.SetVelocity(Vector2.zero);
    }
}
