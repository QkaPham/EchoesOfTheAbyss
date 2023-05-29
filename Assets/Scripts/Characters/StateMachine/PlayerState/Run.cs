using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : State
{
    public float RunSpeed => player.stats.RunSpeed;
    public Run(StateMachine stateMachine, Animator animator, Player player) : base(stateMachine, animator, player)
    {
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        animator.SetInteger("State", (int)PlayerAnimatorParameters.Run);
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        player.SetVelocity(InputManager.Instance.MoveDir * player.stats.RunSpeed);
        player.stamina.ConsumePerSecond(player.stats.RunStaminaConsume);
        //player.stamina.CurrentStamina -= player.stats.RunStaminaConsume * Time.deltaTime;
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        player.stamina.LastTimeConsumeStamina = Time.time;
    }

}
