using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class RushEnemy : BasePoolableEnemy
{
    [SerializeField] protected LineRenderer lineRenderer;
    [SerializeField] protected RushEnemyConfig config;
    protected float elapsedAimingTime = 0;
    protected float elapsedDelayAttackTime = 0f;
    protected Vector3 rushDirection;
    protected Vector3 rushPositon;
    protected bool canAttack => Time.time >= lastAttackTime + config.attackCooldownTime;
    protected float elapsedTimeBetweenEachDamage;
    protected float attackTime;

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
            case EnemyState.Aiming:
                HandleAimingState();
                break;
            default:
                break;
        }
    }

    protected virtual void HandleIdleState()
    {
        rb.velocity = Vector3.zero;

        if (playerDistance > config.aimingRange)
        {
            NextState = EnemyState.Move;
            return;
        }
        if (playerDistance <= config.aimingRange && canAttack)
        {
            NextState = EnemyState.Aiming;
        }
    }

    protected virtual void HandleMoveState()
    {
        if (playerDistance > config.aimingRange)
        {
            rb.velocity = playerDirection * config.moveSpeed;
        }

        if (playerDistance <= config.aimingRange)
        {
            NextState = EnemyState.Idle;
            return;
        }
        if (playerDistance <= config.aimingRange && canAttack)
        {
            NextState = EnemyState.Aiming;
        }
    }


    protected virtual void HandleAttackState()
    {
        rb.velocity = rushDirection * config.rushSpeed;
        attackTime += Time.deltaTime;

        if (playerDistance < config.damageDistance && elapsedTimeBetweenEachDamage >= config.timeBetweenEachDamage)
        {
            player.health.TakeDamage(stats.totalAttack);
            elapsedTimeBetweenEachDamage = 0;
        }

        if (elapsedTimeBetweenEachDamage < config.timeBetweenEachDamage)
        {
            elapsedTimeBetweenEachDamage += Time.deltaTime;
        }

        if (attackTime >= config.rushRange / config.rushSpeed)
        {
            attackTime = 0;
            lastAttackTime = Time.time;
            NextState = EnemyState.Idle;
        }

        if (!canAttack)
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

    protected virtual void HandleAimingState()
    {
        if (elapsedAimingTime < config.aimingTime)
        {
            rb.velocity = Vector3.zero;
            elapsedAimingTime += Time.deltaTime;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + playerDirection * config.rushRange);
        }
        else
        {
            elapsedDelayAttackTime += Time.deltaTime;
        }

        if (elapsedAimingTime >= config.aimingTime && elapsedDelayAttackTime == 0)
        {
            rushPositon = transform.position + playerDirection * config.rushRange;
            rushDirection = playerDirection;
        }

        if (elapsedDelayAttackTime >= config.delayAttackTime && canAttack)
        {
            elapsedTimeBetweenEachDamage = config.timeBetweenEachDamage;
            elapsedDelayAttackTime = 0;
            elapsedAimingTime = 0;
            NextState = EnemyState.Attack;
        }
    }

    public override void Hurt()
    {
        if (CurrentState == EnemyState.Attack || CurrentState == EnemyState.Aiming)
        {
            return;
        }
        base.Hurt();
    }

    public override void Death()
    {
        base.Death();
    }

    public override void Destroy(float time = 0)
    {
        elapsedAimingTime = 0;
        elapsedDelayAttackTime = 0f;
        base.Destroy(time);
    }

    protected override void Flip()
    {
        if (elapsedDelayAttackTime != 0 || CurrentState == EnemyState.Attack || NextState == EnemyState.Attack)
        {
            return;
        }
        base.Flip();
    }
}
