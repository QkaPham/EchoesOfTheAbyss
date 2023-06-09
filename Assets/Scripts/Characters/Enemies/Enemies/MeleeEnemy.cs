using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : BasePoolableEnemy
{
    [SerializeField]
    private MeleeEnemyConfig config;

    protected bool canAttack => Time.time >= lastAttackTime + config.attackCooldownTime;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
        switch (CurrentState)
        {
            case EnemyState.Idle:
                HandleIdleState();
                break;
            case EnemyState.Move:
                HandleMoveState();
                break;
            case EnemyState.Attack:
                HandleAttackState();
                break;
            case EnemyState.Hurt:
                HandleHurtState();
                break;
            case EnemyState.Death:
                HandleDeathState();
                break;
            default:
                break;
        }
    }

    protected virtual void HandleIdleState()
    {
        rb.velocity = Vector3.zero;

        if (playerDistance > config.attackRange)
        {
            NextState = EnemyState.Move;
            return;
        }
        if (playerDistance <= config.attackRange && canAttack)
        {
            NextState = EnemyState.Attack;
        }
    }

    protected virtual void HandleMoveState()
    {
        rb.velocity = playerDirection * config.moveSpeed;

        if (playerDistance <= config.attackRange && canAttack)
        {
            NextState = EnemyState.Attack;
        }
    }

    protected virtual void HandleAttackState()
    {
        rb.velocity = Vector3.zero;
        lastAttackTime = Time.time;

        if (playerDistance > config.attackRange)
        {
            NextState = EnemyState.Move;
            return;
        }
        if (playerDistance <= config.attackRange)
        {
            NextState = EnemyState.Idle;
        }
    }

    protected virtual void HandleHurtState()
    {
        rb.velocity = Vector3.zero;
    }

    protected virtual void HandleDeathState()
    {
        rb.velocity = Vector3.zero;
    }

    protected void DealDamage()
    {
        if (playerDistance <= config.damageRange)
        {
            player.health.TakeDamage(stats.totalAttack);
        }
    }
}
