using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : State
{
    public bool CanDash => LastDashTime + player.stats.DashCooldownTime <= Time.time;
    public bool DashEnd => Time.time >= LastDashTime + player.stats.DashTime;
    public Vector2 dashDir;
    public float LastDashTime = float.MinValue;

    public float DashSpeed => player.stats.DashSpeed;
    public float ActiveTime => player.stats.DashTime;
    public float CooldownTime => player.stats.DashCooldownTime;
    public Dash(StateMachine stateMachine, Animator animator, Player player) : base(stateMachine, animator, player)
    {

    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        LastDashTime = Time.time;
        dashDir = InputManager.Instance.DashDir;
        player.PlayDashEffect(dashDir);
        player.stamina.Consume(player.stats.DashStaminaConsume);

        animator.SetInteger("State", (int)PlayerState.Dash);
        if (dashDir.x > 0 && InputManager.Instance.MouseOnWorld.x > playerPosition.x || dashDir.x < 0 && InputManager.Instance.MouseOnWorld.x < playerPosition.x)
        {
            player.animator.SetBool("DashForward", true);
        }
        else
        {
            player.animator.SetBool("DashForward", false);
        }
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        StartDash();
    }
    private void StartDash()
    {
        player.SetVelocity(dashDir.normalized * player.stats.DashSpeed);
    }

}
