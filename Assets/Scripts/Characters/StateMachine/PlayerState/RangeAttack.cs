using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : State
{
    public bool CanAttack => Time.time >= lastAttackTime + CooldownTime;
    public bool AttackEnd => Time.time >= lastAttackTime + ActiveTime + 0.1f && !InputManager.Instance.Attack;

    public float AttackDamage => player.stats.Attack.Total;
    public float ActiveTime => player.stats.AttackTime;
    public float AttackMoveSpeed => player.stats.AttackMoveSpeed;
    public float CooldownTime => player.stats.AttackCooldownTime;
    private float lastAttackTime = float.MinValue;

    private float manaConsume = 20;
    public RangeAttack(StateMachine stateMachine, Animator animator, Player player) : base(stateMachine, animator, player)
    {

    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        animator.SetInteger("State", (int)PlayerAnimatorParameters.Attack);
        player.mana.Consume(manaConsume);
        lastAttackTime = Time.time;
        player.FireBullet();
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if (CanAttack)
        {
            ContinuouslyAttack();
        }
        player.SetVelocity(InputManager.Instance.MoveDir * AttackMoveSpeed);
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
    }

    private void ContinuouslyAttack()
    {
        player.mana.Consume(manaConsume);
        lastAttackTime = Time.time;
        player.FireBullet();
    }
}
