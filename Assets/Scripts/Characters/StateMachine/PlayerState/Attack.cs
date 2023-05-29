using System;
using UnityEngine;

public class Attack : State
{
    public bool CanAttack => Time.time >= lastAttackTime + CooldownTime;
    public bool AttackEnd => Time.time >= lastAttackTime + ActiveTime + 0.1f && !InputManager.Instance.Attack;

    public float ActiveTime => player.stats.AttackTime;
    public float AttackMoveSpeed => player.stats.AttackMoveSpeed;
    public float CooldownTime => player.stats.AttackCooldownTime;
    private float lastAttackTime = float.MinValue;

    public static event Action<CharacterStats> OnStartAttack;

    public Attack(StateMachine stateMachine, Animator animator, Player player) : base(stateMachine, animator, player)
    {

    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        animator.SetInteger("State", (int)PlayerAnimatorParameters.Attack);
        lastAttackTime = Time.time;
        AudioManager.Instance.PlaySE("Slash");
        OnStartAttack?.Invoke(player.stats);
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
        AudioManager.Instance.PlaySE("Slash");
        lastAttackTime = Time.time;
        OnStartAttack?.Invoke(player.stats);
    }
}