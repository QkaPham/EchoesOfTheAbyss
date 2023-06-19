using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class RangeEnemy : BasePoolableEnemy
{
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected RangeEnemyConfig config;
    [HideInInspector] public ObjectPool<EnemyBullet> bulletPool;
    protected float moveTime;
    protected float idleTime;
    protected Vector3 currentDirection;
    protected Vector3 moveDir;
    protected Vector3 targetDirection => (playerPositon + new Vector3(0f, 0.5f, 0f) - transform.position).normalized;
    protected bool canAttack => Time.time >= lastAttackTime + config.attackCooldownTime;

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
        idleTime -= Time.deltaTime;
        rb.velocity = Vector2.zero;

        if (idleTime <= 0)
        {
            idleTime = 2;
            float alpha = Mathf.Clamp((config.attackRange - playerDistance) / (config.attackRange - config.fleeRange) * 180, 0, 180);
            moveDir = Quaternion.Euler(0f, 0f, alpha * (Random.Range(0, 2) * 2 - 1)) * playerDirection;
            NextState = EnemyState.Move;
        }

        if (playerDistance > config.attackRange)
        {
            idleTime = 2;
            moveDir = playerDirection;
            NextState = EnemyState.Move;
            return;
        }

        if (playerDistance < config.fleeRange)
        {
            idleTime = 2;
            moveDir = -playerDirection;
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
        rb.velocity = moveDir * config.moveSpeed;
        moveTime -= Time.deltaTime;

        if (moveTime <= 0)
        {
            if (playerDistance <= config.attackRange && canAttack)
            {
                NextState = EnemyState.Attack;
                moveTime = 1;
                return;
            }
            if (playerDistance <= config.attackRange && !canAttack)
            {
                moveTime = 1;
                NextState = EnemyState.Idle;
            }
        }

    }

    protected virtual void HandleAttackState()
    {
        rb.velocity = Vector3.zero;
        if (canAttack)
        {
            lastAttackTime = Time.time;
            EnemyBullet bullet = bulletPool.Get();
            bullet.Init(firePoint.position, targetDirection, stats.totalAttack, config.bulletSpeed, config.bulletRange);
        }

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
        DelayNextAttack(config.attackTimeDelayByHurt);
    }

    private void DelayNextAttack(float timedelay)
    {
        lastAttackTime = Time.time - config.attackCooldownTime + timedelay;
    }

    protected virtual void HandleDeathState()
    {
        rb.velocity = Vector3.zero;
    }
}
