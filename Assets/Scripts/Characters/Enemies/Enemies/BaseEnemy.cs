using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum EnemyState
{
    //Empty = -1,
    Idle = 0,
    Move,
    Attack,
    Hurt,
    Death,
    Aiming = 5,

    //Boss state
    FireBullet = 6,
    RushAim,
    Rush,
    LaserAim,
    ShotLaser,
    Intro
}

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D), typeof(Animator))]
public abstract class BaseEnemy : MonoBehaviour
{
    public string enemyName;
    [TextArea]
    public string enemyDescription;
    protected Player player;
    protected Vector3 playerPositon => player.transform.position;
    protected float playerDistance => Vector3.Distance(playerPositon, transform.position);
    protected Vector3 playerDirection => (player.transform.position - transform.position).normalized;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected EnemyHealth health;
    [SerializeField]
    protected EnemyState currentState;
    public EnemyState CurrentState { get => currentState; set => currentState = value; }
    [SerializeField]
    protected EnemyState nextState;
    public EnemyState NextState { get => nextState; set => nextState = value; }

    [SerializeField]
    protected Transform rootTransform;
    protected ObjectPool<Fragment> fragmentPool;
    protected float lastAttackTime = float.MinValue;
    public EnemyStats stats;
    private bool stopAttack;

    protected Action<Notify> OnPlayerDeath;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = GetComponentInChildren<EnemyHealth>();

        OnPlayerDeath = thisNotify => StopAttack();
    }

    protected virtual void Start()
    {
        stats.Init();
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventID.PlayerDeath, OnPlayerDeath);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventID.PlayerDeath, OnPlayerDeath);
    }

    protected virtual void Update()
    {
        Flip();
        StateUpdate();
    }

    public virtual void Init(Player player = null, ObjectPool<Fragment> fragmentPool = null, Vector3 position = default)
    {
        this.player = player;
        transform.position = position;
        this.fragmentPool = fragmentPool;
        health.Init(this);
        NextState = EnemyState.Idle;
        stopAttack = false;
    }

    protected virtual void StopAttack()
    {
        stopAttack = true;
    }

    protected virtual void Flip()
    {
        if (playerDirection.x > 0)
        {
            rootTransform.localScale = Vector3.one;
        }
        if (playerDirection.x < 0)
        {
            rootTransform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
    protected virtual void ReverseFlip()
    {
        if (playerDirection.x > 0)
        {
            rootTransform.localScale = new Vector3(-1f, 1f, 1f);
        }
        if (playerDirection.x < 0)
        {
            rootTransform.localScale = Vector3.one;
        }
    }

    protected virtual void StateUpdate()
    {
        if (stopAttack)
        {
            NextState = EnemyState.Idle;
            CurrentState = EnemyState.Idle;
        }
        if (NextState == CurrentState) return;
        else
        {
            CurrentState = NextState;
            animator.SetInteger("State", (int)CurrentState);
        }
    }

    /// <summary>
    /// Function will be call when enemy health become 0.
    /// </summary>
    public virtual void Death()
    {
        NextState = EnemyState.Death;
    }

    /// <summary>
    /// Function will be call when death animation done.
    /// </summary>
    public virtual void Destroy()
    {
        Drop();
        Destroy(gameObject);
    }

    public virtual void Hurt()
    {
        NextState = EnemyState.Hurt;
    }

    public virtual void EndHurt()
    {
        if (!health.isDeath)
        {
            nextState = EnemyState.Idle;
        }
    }

    protected virtual void PreventPushing(bool active)
    {
        if (active)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    protected void Drop()
    {
        var fragment = fragmentPool.Get();
        fragment.transform.position = transform.position;
        fragment.Amount = stats.totalfragment;
    }
}
