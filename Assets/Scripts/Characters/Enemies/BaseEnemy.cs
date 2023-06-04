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

    FireBullet,
    RushAim,
    Rush,
    LaserAim,
    ShotLaser
}

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D), typeof(Animator))]
public abstract class BaseEnemy : MonoBehaviour
{
    public string enemyName;
    [TextArea]
    public string enemyDescription;
    [SerializeField]
    protected Player player;
    protected Vector3 playerPositon => player.transform.position;
    protected float playerDistance => Vector3.Distance(playerPositon, transform.position);
    protected Vector3 playerDirection => (player.transform.position - transform.position).normalized;

    protected Rigidbody2D rb;
    protected Animator animator;
    [SerializeField]
    protected EnemyHealth health;

    [SerializeField]
    protected EnemyState currentState;
    public EnemyState CurrentState { get => currentState; set => currentState = value; }

    [SerializeField]
    protected EnemyState nextState;
    public EnemyState NextState { get => nextState; set => nextState = value; }

    [SerializeField]
    protected Transform FlipTransform;

    [Header("Enemy Drops")]
    [SerializeField]
    protected int fragment = 5000;

    [SerializeField]
    protected List<Item> items;

    [SerializeField]
    protected float dropChance = 0.5f;

    protected ObjectPool<Fragment> fragmentPool;
    protected ObjectPool<CollectibleItem> collectibleItemPool;

    protected float lastAttackTime = float.MinValue;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = GetComponentInChildren<EnemyHealth>();
        NextState = EnemyState.Idle;
    }

    protected virtual void OnEnable()
    {
        Player.PlayerDeath += StopAttack;
    }

    protected virtual void OnDisable()
    {
        Player.PlayerDeath -= StopAttack;
    }
    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        Flip();
        StateUpdate();
    }

    public virtual void Init(Player player = null, ObjectPool<Fragment> fragmentPool = null, Vector3 position = default, ObjectPool<CollectibleItem> collectibleItemPool = null)
    {
        this.player = player;
        health.Init(this);
        transform.position = position;
        this.fragmentPool = fragmentPool;
        this.collectibleItemPool = collectibleItemPool;
        NextState = EnemyState.Idle;
    }

    protected virtual void StopAttack()
    {
        lastAttackTime = float.MaxValue;
    }

    protected virtual void Flip()
    {
        if (playerDirection.x > 0)
        {
            FlipTransform.localScale = Vector3.one;
        }
        if (playerDirection.x < 0)
        {
            FlipTransform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
    protected virtual void ReverseFlip()
    {
        if (playerDirection.x > 0)
        {
            FlipTransform.localScale = new Vector3(-1f, 1f, 1f);
        }
        if (playerDirection.x < 0)
        {
            FlipTransform.localScale = Vector3.one;
        }
    }

    protected virtual void StateUpdate()
    {
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

    public void Drop()
    {
        var random = Random.value;
        if (random < dropChance && items.Count >= 1)
        {
            var item = collectibleItemPool.Get();
            item.transform.position = this.transform.position;
            item.item = items[Random.Range(0, items.Count - 1)];
        }

        var fragment = fragmentPool.Get();
        fragment.transform.position = this.transform.position;
        fragment.Amount = this.fragment;
    }
}
