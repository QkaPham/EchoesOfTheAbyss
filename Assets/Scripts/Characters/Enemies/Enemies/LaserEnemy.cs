using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemy : BasePoolableEnemy
{
    [SerializeField] protected LineRenderer lineRenderer;
    [SerializeField] protected Transform ShotPoint;
    [SerializeField] protected LaserEnemyConfig config;

    protected float elapsedAimingTime = 0;
    protected float elapsedDelayAttackTime = 0;
    protected Vector3 shotDirection;
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

        if (playerDistance < config.fleeDistance || playerDistance > config.aimingRange)
        {
            NextState = EnemyState.Move;
            return;
        }
        if (playerDistance <= config.aimingRange && canAttack)
        {
            NextState = EnemyState.Aiming;
            PreventPushing(true);
        }
    }

    protected virtual void HandleMoveState()
    {
        if (playerDistance > config.aimingRange)
        {
            rb.velocity = playerDirection * config.moveSpeed;
        }
        else if (playerDistance < config.fleeDistance)
        {
            rb.velocity = -playerDirection * config.moveSpeed;
        }


        if (playerDistance >= config.fleeDistance && playerDistance <= config.aimingRange)
        {
            NextState = EnemyState.Idle;
            return;
        }
        if (playerDistance <= config.aimingRange && canAttack)
        {
            NextState = EnemyState.Aiming;
            PreventPushing(true);
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
        if (elapsedAimingTime < config.aimingTime)
        {
            rb.velocity = Vector3.zero;
            lineRenderer.SetPosition(0, ShotPoint.position);
            lineRenderer.SetPosition(1, (player.TargetPoint.position - ShotPoint.position).normalized * config.laserRange + ShotPoint.position);
            elapsedAimingTime += Time.deltaTime;
        }
        else
        {
            elapsedDelayAttackTime += Time.deltaTime;
        }

        if (elapsedAimingTime >= config.aimingTime && elapsedDelayAttackTime == 0)
        {
            shotDirection = playerDirection;
        }

        if (elapsedDelayAttackTime >= config.delayAttackTime)
        {
            elapsedDelayAttackTime = 0;
            elapsedAimingTime = 0;
            NextState = EnemyState.Attack;
        }
    }

    protected virtual void LaserShot()
    {
        PreventPushing(false);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, shotDirection, config.laserRange, config.PlayerLayerMask);
        if (!hit) return;
        Player player = hit.transform.GetComponentInParent<Player>();
        player.health.TakeDamage(stats.totalAttack);
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
            DelayNextAttack(config.attackTimeDelayByHurt);
        }
        base.Hurt();
    }

    private void DelayNextAttack(float timedelay)
    {
        lastAttackTime = Time.time - config.attackCooldownTime + timedelay;
    }

    public override void Death()
    {
        base.Death();

    }
    public override void Destroy(float time = 0)
    {
        elapsedAimingTime = 0;
        elapsedDelayAttackTime = 0;
        base.Destroy(time);
    }

    protected override void Flip()
    {
        if (CurrentState == EnemyState.Move && playerDistance < config.fleeDistance)
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
