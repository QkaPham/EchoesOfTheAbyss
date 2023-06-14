using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class RushEnemy : BasePoolableEnemy
{
    [SerializeField]
    protected LineRenderer lineRenderer;
    [SerializeField]
    protected RushEnemyProfile profile;
    protected float elapsedAimingTime = 0;
    protected float elapsedDelayAttackTime = 0f;
    protected Vector3 rushDirection;
    protected Vector3 rushPositon;
    protected bool canAttack => Time.time >= lastAttackTime + profile.attackCooldownTime;
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

        if (playerDistance > profile.aimingRange)
        {
            NextState = EnemyState.Move;
            return;
        }
        if (playerDistance <= profile.aimingRange && canAttack)
        {
            NextState = EnemyState.Aiming;
        }
    }

    protected virtual void HandleMoveState()
    {
        if (playerDistance > profile.aimingRange)
        {
            rb.velocity = playerDirection * profile.moveSpeed;
        }

        if (playerDistance <= profile.aimingRange)
        {
            NextState = EnemyState.Idle;
            return;
        }
        if (playerDistance <= profile.aimingRange && canAttack)
        {
            NextState = EnemyState.Aiming;
        }
    }


    protected virtual void HandleAttackState()
    {
        rb.velocity = rushDirection * profile.rushSpeed;
        attackTime += Time.deltaTime;

        if (playerDistance < profile.damageDistance && elapsedTimeBetweenEachDamage >= profile.timeBetweenEachDamage)
        {
            player.health.TakeDamage(stats.totalAttack);
            elapsedTimeBetweenEachDamage = 0;
        }

        if (elapsedTimeBetweenEachDamage < profile.timeBetweenEachDamage)
        {
            elapsedTimeBetweenEachDamage += Time.deltaTime;
        }

        if (attackTime >= profile.rushRange / profile.rushSpeed)
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
        if (elapsedAimingTime < profile.aimingTime)
        {
            rb.velocity = Vector3.zero;
            elapsedAimingTime += Time.deltaTime;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + playerDirection * profile.rushRange);
        }
        else
        {
            elapsedDelayAttackTime += Time.deltaTime;
        }

        if (elapsedAimingTime >= profile.aimingTime && elapsedDelayAttackTime == 0)
        {
            rushPositon = transform.position + playerDirection * profile.rushRange;
            rushDirection = playerDirection;
        }

        if (elapsedDelayAttackTime >= profile.delayAttackTime && canAttack)
        {
            elapsedTimeBetweenEachDamage = profile.timeBetweenEachDamage;
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

    protected override void Flip()
    {
        if (elapsedDelayAttackTime != 0 || CurrentState == EnemyState.Attack || NextState == EnemyState.Attack)
        {
            return;
        }
        base.Flip();
    }
}
