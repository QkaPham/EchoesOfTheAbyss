using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
public enum PlayerAnimatorParameters
{
    Idle = 0,
    Walk,
    Run,
    Dash,
    Attack,
    Death
}

public class Player : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    public Animator animator;

    [SerializeField]
    public Transform rootTransform;

    [SerializeField]
    public GameObject playerBulletPrefabs;

    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    public Transform TargetPoint;

    private StateMachine stateMachine;

    [SerializeField]
    private Currency currency;

    [SerializeField]
    public CharacterStats stats;

    [SerializeField]
    public Health health;

    [SerializeField]
    public Mana mana;

    [SerializeField]
    public Stamina stamina;

    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private Equipment equipment;

    [SerializeField]
    private Weapon weapon;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        weapon = GetComponentInChildren<Weapon>();

        stateMachine = new StateMachine();
        var idle = new Idle(stateMachine, animator, this);
        var walk = new Walk(stateMachine, animator, this);
        var run = new Run(stateMachine, animator, this);
        var dash = new Dash(stateMachine, animator, this);
        var attack = new Attack(stateMachine, animator, this);
        var rangeAttack = new RangeAttack(stateMachine, animator, this);

        stateMachine.Initialize(idle);
        stateMachine.AddAnyTransition(dash, () => InputManager.Instance.Dash && dash.CanDash && stamina.CurrentStamina >= stats.DashStaminaConsume);
        stateMachine.AddTransition(dash, idle, () => dash.DashEnd);
        stateMachine.AddAnyTransition(attack, () => InputManager.Instance.Attack && attack.CanAttack && stateMachine.currentState != dash);
        stateMachine.AddTransition(attack, idle, () => attack.AttackEnd);
        stateMachine.AddTransition(idle, walk, () => InputManager.Instance.Move);
        stateMachine.AddTransition(walk, run, () => InputManager.Instance.Run && stamina.CurrentStamina >= 1f);
        stateMachine.AddTransition(walk, idle, () => !InputManager.Instance.Move);
        stateMachine.AddTransition(run, idle, () => !InputManager.Instance.Run || !InputManager.Instance.Move);
        stateMachine.AddTransition(run, walk, () => stamina.CurrentStamina == 0f);
        stateMachine.AddAnyTransition(rangeAttack, () => InputManager.Instance.RangeAttack && rangeAttack.CanAttack && mana.CurrentMana >= 20);
        stateMachine.AddTransition(rangeAttack, idle, () => rangeAttack.AttackEnd || mana.currentMana < 20);
    }

    private void OnEnable()
    {
        GameManager.OnStartGame += ResetPlayer;
        //Health.OnGameOver += () => animator.SetTrigger("Death");
    }

    private void OnDisable()
    {
        GameManager.OnStartGame -= ResetPlayer;
        //Health.OnGameOver -= () => animator.SetTrigger("Death");
    }

    private void ResetPlayer()
    {
        gameObject.SetActive(true);
        currency.Init();
        stats.Init();
        health.Init(stats.MaxHealthPoint.Total,this);
        mana.Init(stats.MaxStamina);
        stamina.Init(stats.MaxMana);
        inventory.Init();
        equipment.Init();
        weapon.gameObject.SetActive(true);
    }

    private void Update()
    {
        stateMachine.OnFSMUpdate();
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

    public void Death()
    {
        animator.SetTrigger("Death");
        weapon.gameObject.SetActive(false);
    }
}
