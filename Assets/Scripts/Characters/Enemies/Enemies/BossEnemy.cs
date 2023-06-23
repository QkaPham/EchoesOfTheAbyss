using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public enum AttackType
{
    Rush,
    Bullet,
    Laser
}

public class BossEnemy : BaseEnemy
{
    [Header("Addional References")]
    [SerializeField]
    protected LineRenderer laserLineRenderer;

    [SerializeField]
    protected Transform ShotPoint;

    [SerializeField]
    protected LineRenderer rushLineRenderer;

    [Header("Movement")]
    [SerializeField]
    protected float moveSpeed = 2f;

    public List<AttackType> attackCycle;
    private int currentAttackPointer;

    [SerializeField]
    public AttackType currentAttackType => attackCycle[currentAttackPointer];

    [Header("Rush")]
    [SerializeField]
    protected float RushAimingRange = 5f;

    [SerializeField]
    protected float rushAimingTime = 2;
    protected float elapsedRushAimingTime = 0;

    [SerializeField]
    protected float rushDelayAttackTime = 2f;
    protected float elapsedRushDelayAttackTime = 0f;

    [SerializeField]
    protected float rushSpeed = 10;

    [SerializeField]
    protected float rushRange = 3;

    protected Vector3 rushDirection;
    protected Vector3 rushPositon;

    [SerializeField]
    protected float rushAttackDamage = 1;

    [SerializeField]
    protected float attackCooldownTime = 0.5f;
    protected bool canAttack => Time.time >= lastAttackTime + attackCooldownTime;

    [SerializeField]
    protected float rushDamageDistance = .8f;
    protected float rushTime;

    [SerializeField]
    protected float timeBetweenEachRushDamage = 1f;
    protected float elapsedTimeBetweenEachRushDamage;

    [Header("Laser")]
    [SerializeField]
    protected float laserAimingRange = 5f;

    [SerializeField]
    protected float laserAimingTime = 2;
    protected float elapsedLaserAimingTime = 0;
    protected Vector3 laserShotDirection;

    [SerializeField]
    protected float laserDelayAttackTime = 2f;
    protected float elapsedLaserDelayAttackTime = 0f;

    [SerializeField]
    protected float laserAttackDamage = 20;

    [SerializeField]
    protected float laserRange = 15f;

    [SerializeField]
    protected LayerMask PlayerLayerMask;

    [Header("Bullet")]
    [SerializeField]
    protected Transform bulletFirePoint;

    protected EnemyBulletPool bulletPool;

    [SerializeField]
    protected float bulletRange = 12f;

    [SerializeField]
    protected float bulletAttackDamage = 10f;
    protected Vector3 targetDirection => (playerPositon + new Vector3(0f, 0.5f, 0f) - transform.position).normalized;

    [SerializeField]
    protected int waveNumber = 4;
    protected int counter = 0;

    [SerializeField]
    protected int bulletNumber = 4;

    [SerializeField]
    protected float bulletAngle = 60f;

    [SerializeField]
    protected float bulletSpeed = 10f;

    protected float spawnTime;
    protected float introDuration = 2;
    protected override void Awake()
    {
        base.Awake();
        spawnTime = Time.time;
        PreventPushing(true);
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
            case EnemyState.FireBullet:
                HandleFireBulletState();
                break;
            case EnemyState.Death:
                HandleDeathState();
                break;
            case EnemyState.LaserAim:
                HandleLaserAimState();
                break;
            case EnemyState.ShotLaser:
                HandleShotLaserState();
                break;
            case EnemyState.RushAim:
                HandleRushAimState();
                break;
            case EnemyState.Rush:
                HandleRushState();
                break;
            default:
                break;
        }
    }

    public void Init(Player player, Vector3 position, EnemyBulletPool bulletPool)
    {
        this.player = player;
        transform.position = position;
        this.bulletPool = bulletPool;
        stats.Init();
        health.Init(this);
    }

    public void ChangeAttackType()
    {
        currentAttackPointer = (currentAttackPointer + 1) % attackCycle.Count;
    }

    protected virtual void HandleIdleState()
    {
        rb.velocity = Vector3.zero;
        if (Time.time > spawnTime + introDuration)
        {
            if (currentAttackType == AttackType.Rush)
            {
                if (playerDistance > RushAimingRange)
                {
                    NextState = EnemyState.Move;
                    return;
                }
                if (playerDistance <= RushAimingRange && canAttack)
                {
                    NextState = EnemyState.RushAim;
                }
            }

            if (currentAttackType == AttackType.Laser)
            {
                if (playerDistance > laserAimingRange)
                {
                    NextState = EnemyState.Move;
                    return;
                }
                if (playerDistance <= laserAimingRange && canAttack)
                {
                    NextState = EnemyState.LaserAim;
                }
            }

            if (currentAttackType == AttackType.Bullet)
            {
                if (playerDistance > bulletRange)
                {
                    NextState = EnemyState.Move;
                    return;
                }
                if (playerDistance <= bulletRange && canAttack)
                {
                    NextState = EnemyState.FireBullet;
                }
            }
        }
    }

    protected virtual void HandleMoveState()
    {
        rb.velocity = playerDirection * moveSpeed;

        if (currentAttackType == AttackType.Rush)
        {
            if (playerDistance <= RushAimingRange && !canAttack)
            {
                NextState = EnemyState.Idle;
                return;
            }
            if (playerDistance <= RushAimingRange && canAttack)
            {
                NextState = EnemyState.RushAim;
            }
        }

        if (currentAttackType == AttackType.Laser)
        {
            if (playerDistance <= laserAimingRange && !canAttack)
            {
                NextState = EnemyState.Idle;
                return;
            }
            if (playerDistance <= laserAimingRange && canAttack)
            {
                NextState = EnemyState.LaserAim;
            }
        }

        if (currentAttackType == AttackType.Bullet)
        {
            if (playerDistance <= bulletRange && !canAttack)
            {
                NextState = EnemyState.Idle;
                return;
            }
            if (playerDistance <= bulletRange && canAttack)
            {
                NextState = EnemyState.FireBullet;
            }
        }
    }

    protected virtual void HandleFireBulletState()
    {
        rb.velocity = Vector3.zero;
    }

    protected virtual void HandleRushAimState()
    {
        if (elapsedRushAimingTime < rushAimingTime)
        {
            rb.velocity = Vector3.zero;
            elapsedRushAimingTime += Time.deltaTime;
            rushLineRenderer.SetPosition(0, transform.position);
            rushLineRenderer.SetPosition(1, transform.position + playerDirection * rushRange);
        }
        else
        {
            elapsedRushDelayAttackTime += Time.deltaTime;
        }

        if (elapsedRushAimingTime >= rushAimingTime && elapsedRushDelayAttackTime == 0)
        {
            rushPositon = transform.position + playerDirection * rushRange;
            rushDirection = playerDirection;
        }

        if (elapsedRushDelayAttackTime >= rushDelayAttackTime)
        {
            elapsedTimeBetweenEachRushDamage = timeBetweenEachRushDamage;
            elapsedRushDelayAttackTime = 0;
            elapsedRushAimingTime = 0;
            NextState = EnemyState.Rush;
        }
    }

    protected virtual void HandleRushState()
    {
        rb.velocity = rushDirection * rushSpeed;
        rushTime += Time.deltaTime;

        if (playerDistance < rushDamageDistance && elapsedTimeBetweenEachRushDamage >= timeBetweenEachRushDamage)
        {
            player.health.TakeDamage(rushAttackDamage);
            elapsedTimeBetweenEachRushDamage = 0;
        }

        if (elapsedTimeBetweenEachRushDamage < timeBetweenEachRushDamage)
        {
            elapsedTimeBetweenEachRushDamage += Time.deltaTime;
        }

        if (rushTime >= rushRange / rushSpeed)
        {
            rushTime = 0;
            lastAttackTime = Time.time;
            ChangeAttackType();
            NextState = EnemyState.Idle;
        }
    }

    protected virtual void HandleLaserAimState()
    {
        if (elapsedLaserAimingTime < laserAimingTime)
        {
            rb.velocity = Vector3.zero;
            laserLineRenderer.SetPosition(0, ShotPoint.position);
            laserLineRenderer.SetPosition(1, (player.TargetPoint.position - ShotPoint.position).normalized * laserRange + ShotPoint.position);
            elapsedLaserAimingTime += Time.deltaTime;
        }
        else
        {
            elapsedLaserDelayAttackTime += Time.deltaTime;
        }

        if (elapsedLaserAimingTime >= laserAimingTime && elapsedLaserDelayAttackTime == 0)
        {
            laserShotDirection = playerDirection;
        }

        if (elapsedLaserDelayAttackTime >= laserDelayAttackTime)
        {
            elapsedLaserDelayAttackTime = 0;
            elapsedLaserAimingTime = 0;
            NextState = EnemyState.ShotLaser;
        }
    }

    protected virtual void HandleShotLaserState()
    {
        rb.velocity = Vector3.zero;
        lastAttackTime = Time.time;
    }

    protected virtual void HandleDeathState()
    {
        rb.velocity = Vector3.zero;
    }

    protected virtual void HandleAimingState()
    {
        if (elapsedRushAimingTime < rushAimingTime)
        {
            rb.velocity = Vector3.zero;
            elapsedRushAimingTime += Time.deltaTime;
            rushLineRenderer.SetPosition(0, transform.position);
            rushLineRenderer.SetPosition(1, transform.position + playerDirection * rushRange);
        }
        else
        {
            elapsedRushDelayAttackTime += Time.deltaTime;
        }

        if (elapsedRushAimingTime >= rushAimingTime && elapsedRushDelayAttackTime == 0)
        {
            rushPositon = transform.position + playerDirection * rushRange;
            rushDirection = playerDirection;
        }

        if (elapsedRushDelayAttackTime >= rushDelayAttackTime)
        {
            elapsedTimeBetweenEachRushDamage = timeBetweenEachRushDamage;
            elapsedRushDelayAttackTime = 0;
            elapsedRushAimingTime = 0;
            NextState = EnemyState.Attack;
        }
    }

    public void FireBulletCounter()
    {
        counter++;
        if (counter >= waveNumber)
        {
            counter = 0;
            ChangeAttackType();
            nextState = EnemyState.Idle;
        }
    }

    public void FireBullets()
    {
        float deltaAngel = bulletAngle / (bulletNumber - 1);
        float minAngel = -bulletAngle / 2;
        for (int i = 0; i < bulletNumber; i++)
        {
            FireBullet(bulletFirePoint.position, targetDirection, minAngel + (i * deltaAngel), bulletAttackDamage);
        }
        lastAttackTime = Time.time;
    }


    protected void FireBullet(Vector2 startPoint, Vector2 originalDirection, float angle, float damage)
    {
        EnemyBullet bullet = bulletPool.Get();
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
        Vector2 rotatedDirection = rotation * originalDirection;
        bullet.Init(bulletFirePoint.position, rotatedDirection, damage, bulletSpeed, bulletRange);
    }

    public void ShotLaser()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, laserShotDirection, laserRange, PlayerLayerMask);
        if (!hit)
        {
            return;
        }

        if (hit.transform.TryGetComponent<Player>(out Player player))
        {
            player.health.TakeDamage(laserAttackDamage);
        }
    }

    public override void Hurt()
    {
        animator.SetTrigger("Hurt");
    }

    public override void Death()
    {
        base.Death();
        GameManager.Instance.Victory();
    }

    public override void Destroy(float time = 0f)
    {
        Destroy(gameObject, time);
    }

    protected override void Flip()
    {
        if (elapsedRushDelayAttackTime != 0 || CurrentState == EnemyState.Attack || NextState == EnemyState.Attack)
        {
            return;
        }
        base.Flip();
    }
}
