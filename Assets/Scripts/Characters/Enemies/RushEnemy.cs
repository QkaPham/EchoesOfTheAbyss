using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class RushEnemy : BasePoolableEnemy
{
    [Header("Addional References")]
    [SerializeField]
    protected LineRenderer lineRenderer;

    [Header("Movement")]
    [SerializeField]
    protected float moveSpeed = 2f;

    [Header("Aiming")]
    [SerializeField]
    protected float aimingRange = 5f;

    [SerializeField]
    protected float aimingTime = 2;
    protected float elapsedAimingTime = 0;

    [SerializeField]
    protected float delayAttackTime = 2f;
    protected float elapsedDelayAttackTime = 0f;

    [Header("Attack")]
    [SerializeField]
    protected float rushSpeed = 10;

    [SerializeField]
    protected float rushRange = 3;

    protected Vector3 rushDirection;
    protected Vector3 rushPositon;
    [SerializeField]
    protected float attackDamage = 1;

    [SerializeField]
    protected float attackCooldownTime = 0.5f;
    protected bool canAttack => Time.time >= lastAttackTime + attackCooldownTime;
    protected float lastAttackTime = float.MinValue;

    [SerializeField]
    protected float damageDistance = .8f;

    [SerializeField]
    protected float timeBetweenEachDamage = 1f;
    protected float elapsedTimeBetweenEachDamage;

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

        if (playerDistance > aimingRange)
        {
            NextState = EnemyState.Move;
            return;
        }
        if (playerDistance <= aimingRange && canAttack)
        {
            NextState = EnemyState.Aiming;
        }
    }

    protected virtual void HandleMoveState()
    {
        if (playerDistance > aimingRange)
        {
            rb.velocity = playerDirection * moveSpeed;
        }

        if (playerDistance <= aimingRange)
        {
            NextState = EnemyState.Idle;
            return;
        }
        if (playerDistance <= aimingRange && canAttack)
        {
            NextState = EnemyState.Aiming;
        }
    }

    protected virtual void HandleAttackState()
    {
        rb.velocity = rushDirection * rushSpeed;

        if (playerDistance < damageDistance && elapsedTimeBetweenEachDamage >= timeBetweenEachDamage)
        {
            player.health.TakeDamage(attackDamage);
            elapsedTimeBetweenEachDamage = 0;
        }

        if (elapsedTimeBetweenEachDamage < timeBetweenEachDamage)
        {
            elapsedTimeBetweenEachDamage += Time.deltaTime;
        }

        if (Vector3.Distance(transform.position, rushPositon) <= 0.01f)
        {
            lastAttackTime = Time.time;
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
        if (elapsedAimingTime < aimingTime)
        {
            rb.velocity = Vector3.zero;
            elapsedAimingTime += Time.deltaTime;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + playerDirection * rushRange);
        }
        else
        {
            elapsedDelayAttackTime += Time.deltaTime;
        }

        if (elapsedAimingTime >= aimingTime && elapsedDelayAttackTime == 0)
        {
            rushPositon = transform.position + playerDirection * rushRange;
            rushDirection = playerDirection;
        }

        if (elapsedDelayAttackTime >= delayAttackTime)
        {
            elapsedTimeBetweenEachDamage = timeBetweenEachDamage;
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
