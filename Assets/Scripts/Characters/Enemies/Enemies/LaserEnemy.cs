using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemy : BasePoolableEnemy
{
    [SerializeField]
    protected LaserEnemyProfile profile;

    [Header("Additonal References")]
    [SerializeField]
    protected LineRenderer lineRenderer;

    [SerializeField]
    protected Transform ShotPoint;

    protected float elapsedAimingTime = 0;
    protected float elapsedDelayAttackTime = 0;
    protected Vector3 shotDirection;
    protected bool canAttack => Time.time >= lastAttackTime + profile.attackCooldownTime;

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

        if (playerDistance < profile.fleeDistance || playerDistance > profile.aimingRange)
        {
            NextState = EnemyState.Move;
            return;
        }
        if (playerDistance <= profile.aimingRange && canAttack)
        {
            NextState = EnemyState.Aiming;
            PreventPushing(true);
        }
    }

    protected virtual void HandleMoveState()
    {
        if (playerDistance > profile.aimingRange)
        {
            rb.velocity = playerDirection * profile.moveSpeed;
        }
        else if (playerDistance < profile.fleeDistance)
        {
            rb.velocity = -playerDirection * profile.moveSpeed;
        }


        if (playerDistance >= profile.fleeDistance && playerDistance <= profile.aimingRange)
        {
            NextState = EnemyState.Idle;
            return;
        }
        if (playerDistance <= profile.aimingRange && canAttack)
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
        if (elapsedAimingTime < profile.aimingTime)
        {
            rb.velocity = Vector3.zero;
            lineRenderer.SetPosition(0, ShotPoint.position);
            lineRenderer.SetPosition(1, (player.TargetPoint.position - ShotPoint.position).normalized * profile.laserRange + ShotPoint.position);
            elapsedAimingTime += Time.deltaTime;
        }
        else
        {
            elapsedDelayAttackTime += Time.deltaTime;
        }

        if (elapsedAimingTime >= profile.aimingTime && elapsedDelayAttackTime == 0)
        {
            shotDirection = playerDirection;
        }

        if (elapsedDelayAttackTime >= profile.delayAttackTime)
        {
            elapsedDelayAttackTime = 0;
            elapsedAimingTime = 0;
            NextState = EnemyState.Attack;
        }
    }

    protected virtual void LaserShot()
    {
        PreventPushing(false);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, shotDirection, profile.laserRange, profile.PlayerLayerMask);
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
            DelayNextAttack(profile.attackTimeDelayByHurt);
        }
        base.Hurt();
    }

    private void DelayNextAttack(float timedelay)
    {
        lastAttackTime = Time.time - profile.attackCooldownTime + timedelay;
    }

    public override void Death()
    {
        base.Death();
    }

    protected override void Flip()
    {
        if (CurrentState == EnemyState.Move && playerDistance < profile.fleeDistance)
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
