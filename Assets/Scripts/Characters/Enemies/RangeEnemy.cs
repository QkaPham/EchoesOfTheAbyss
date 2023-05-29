using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class RangeEnemy : BasePoolableEnemy
{
    [Header("Addional References")]
    [SerializeField]
    protected Transform bulletStartPoint;
    public ObjectPool<EnemyBullet> bulletPool;

    [Header("Movement")]
    [SerializeField]
    protected float moveSpeed = 3f;

    [Header("Attack")]
    [SerializeField]
    protected float attackRange = 4f;

    [SerializeField]
    protected float attackDamage = 1;

    [SerializeField]
    protected float CooldownTime = 1f;

    protected bool canAttack => Time.time >= lastAttackTime + CooldownTime;
    protected float lastAttackTime = float.MinValue;
    protected Vector3 targetDirection => (playerPositon + new Vector3(0f, 0.5f, 0f) - transform.position).normalized;

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

        if (playerDistance > attackRange)
        {
            NextState = EnemyState.Move;
            return;
        }
        if (playerDistance <= attackRange && canAttack)
        {
            NextState = EnemyState.Attack;
        }
    }

    protected virtual void HandleMoveState()
    {
        rb.velocity = playerDirection * moveSpeed;

        if (playerDistance <= attackRange && canAttack)
        {
            NextState = EnemyState.Attack;
        }
    }

    protected virtual void HandleAttackState()
    {
        rb.velocity = Vector3.zero;
        if (canAttack)
        {
            lastAttackTime = Time.time;
            EnemyBullet bullet = bulletPool.Get();
            bullet.Init(bulletStartPoint.position, targetDirection, attackDamage);
        }

        if (playerDistance > attackRange)
        {
            NextState = EnemyState.Move;
            return;
        }
        if (playerDistance <= attackRange)
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
}
