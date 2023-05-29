using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemy : BasePoolableEnemy
{
    [Header("Additonal References")]
    [SerializeField]
    protected LineRenderer lineRenderer;

    [SerializeField]
    protected Transform ShotPoint;

    [Header("Movement")]
    [SerializeField]
    protected float moveSpeed = 2f;

    [SerializeField]
    protected float fleeDistance = 2f;

    [Header("Aiming")]
    [SerializeField]
    protected float aimingRange = 5f;

    [SerializeField]
    protected float aimingTime = 2;
    protected float elapsedAimingTime = 0;

    [SerializeField]
    protected float delayAttackTime = 2;
    protected float elapsedDelayAttackTime = 0;
    protected Vector3 shotDirection;

    [Header("Attack")]
    [SerializeField]
    protected LayerMask PlayerLayerMask;

    [SerializeField]
    protected float laserRange = 15f;

    [SerializeField]
    protected float attackDamage = 1;

    [SerializeField]
    protected float attackCooldownTime = 0.5f;
    protected bool canAttack => Time.time >= lastAttackTime + attackCooldownTime;
    protected float lastAttackTime = float.MinValue;


    [SerializeField]
    protected float attackTimeDelayByHurt = 2f;
    protected override void Awake()
    {
        base.Awake();
        //if (player == null)
        //{
        //    player = FindObjectOfType<Player>();
        //}
        //health.Init(this);
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

        if (playerDistance < fleeDistance || playerDistance > aimingRange)
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
        else if (playerDistance < fleeDistance)
        {
            rb.velocity = -playerDirection * moveSpeed;
        }


        if (playerDistance >= fleeDistance && playerDistance <= aimingRange)
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
        rb.velocity = Vector3.zero;
        lastAttackTime = Time.time;
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
            lineRenderer.SetPosition(0, ShotPoint.position);
            lineRenderer.SetPosition(1, (player.TargetPoint.position - ShotPoint.position).normalized * laserRange + ShotPoint.position);
            elapsedAimingTime += Time.deltaTime;
        }
        else
        {
            elapsedDelayAttackTime += Time.deltaTime;
        }

        if (elapsedAimingTime >= aimingTime && elapsedDelayAttackTime == 0)
        {
            shotDirection = playerDirection;
        }

        if (elapsedDelayAttackTime >= delayAttackTime)
        {
            elapsedDelayAttackTime = 0;
            elapsedAimingTime = 0;
            NextState = EnemyState.Attack;
        }
    }

    protected virtual void LaserShot()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, shotDirection, laserRange, PlayerLayerMask);
        if (!hit)
        {
            return;
        }

        if (hit.transform.TryGetComponent<Player>(out Player player))
        {
            player.health.TakeDamage(attackDamage);
        }
    }

    public override void Hurt()
    {
        if (CurrentState == EnemyState.Attack)
        {
            return;
        }
        if (CurrentState == EnemyState.Aiming)
        {
            elapsedDelayAttackTime = 0f;
            elapsedAimingTime = 0f;
            DelayNextAttack(attackTimeDelayByHurt);
        }
        base.Hurt();
    }

    private void DelayNextAttack(float timedelay)
    {
        lastAttackTime = Time.time - attackCooldownTime + timedelay;
    }

    public override void Death()
    {
        base.Death();
    }

    protected override void Flip()
    {
        if (CurrentState == EnemyState.Move && playerDistance < fleeDistance)
        {
            ReverseFlip();
            return;
        }
        if (elapsedDelayAttackTime != 0 || CurrentState == EnemyState.Attack || NextState == EnemyState.Attack)
        {
            return;
        }
        base.Flip();
    }
}
