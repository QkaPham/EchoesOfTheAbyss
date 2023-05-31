using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayer : Player
{
    [SerializeField]
    protected Rigidbody2D rb;

    [SerializeField]
    public Animator animator;

    [SerializeField]
    public Transform rootTransform;

    [SerializeField]
    public GameObject playerBulletPrefabs;

    [SerializeField]
    protected Transform firePoint;

    [SerializeField]
    public Transform TargetPoint;

    [SerializeField]
    protected Currency currency;

    [SerializeField]
    public CharacterStats stats;

    [SerializeField]
    public Health health;

    [SerializeField]
    public Mana mana;

    [SerializeField]
    public Stamina stamina;

    [SerializeField]
    protected Inventory inventory;

    [SerializeField]
    protected Equipment equipment;

    [SerializeField]
    protected Weapon weapon;

    [SerializeField]
    protected PlayerState currentPlayerState;
    
    [SerializeField]
    protected PlayerState nextPlayerState;
    public PlayerState NextPlayerState { get => nextPlayerState; set => nextPlayerState = value; }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        weapon = GetComponentInChildren<Weapon>();
    }

    private void OnEnable()
    {
        GameManager.OnStartGame += ResetPlayer;
    }

    private void OnDisable()
    {
        GameManager.OnStartGame -= ResetPlayer;
    }

    private void ResetPlayer()
    {
        gameObject.SetActive(true);
        currency.Init();
        stats.Init();
        health.Init(stats.MaxHealthPoint.Total, this);
        mana.Init(stats.MaxStamina);
        stamina.Init(stats.MaxMana);
        inventory.Init();
        equipment.Init();
        weapon.gameObject.SetActive(true);
    }

    private void Update()
    {
        Flip();
        stamina.Regenerate();
        mana.Regenerate();
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }
    public void SetVelocity(Vector2 velocity)
    {
        rb.velocity = velocity;
    }

    public void AddForce(Vector2 force, ForceMode2D forceMode2D = ForceMode2D.Impulse)
    {
        rb.AddForce(force, forceMode2D);
    }

    private void Flip()
    {
        if (Time.timeScale != 0.0f)
        {
            rootTransform.localScale = InputManager.Instance.MouseOnWorld.x < transform.position.x ? new Vector3(-1, 1, 1) : Vector3.one;
        }
    }

    public void FireBullet()
    {
        PlayerBullet playerBullet = Instantiate(playerBulletPrefabs, this.transform).GetComponent<PlayerBullet>();
        playerBullet.Init(firePoint.position, (InputManager.Instance.MouseOnWorld - (Vector2)firePoint.position).normalized, stats.Attack.Total, 10, 4);
    }

    public void Hurt()
    {

    }

    public void Death()
    {
        animator.SetTrigger("Death");
        weapon.gameObject.SetActive(false);
    }
}
