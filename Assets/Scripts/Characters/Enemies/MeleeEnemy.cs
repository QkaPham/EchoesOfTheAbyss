using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : BasePoolableEnemy
{
    [Header("Movement")]
    [SerializeField]
    protected float moveSpeed = 3f;

    [Header("Attack")]
    [SerializeField]
    protected float attackRange = 1f;

    [SerializeField]
    protected float damageRange = 1.5f;

    [SerializeField]
    protected float attackDamage = 1;

    [SerializeField]
    protected float attackCooldownTime = 1f;

    protected bool canAttack => Time.time >= lastAttackTime + attackCooldownTime;
    protected float lastAttackTime = float.MinValue;
    protected override void Awake()
    {
        base.Awake();
        //Init();
        //if (player == null)
        //{
        //    player = FindObjectOfType<Player>();
        //}
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
        lastAttackTime = Time.time;

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

    protected void DealDamage()
    {
        if (playerDistance <= damageRange)
        {
            player.health.TakeDamage(attackDamage);
        }
    }

    public override void Release()
    {
        base.Release();
    }
}
