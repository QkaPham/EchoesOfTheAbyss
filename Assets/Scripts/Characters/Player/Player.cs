using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
public enum PlayerState
{
    Idle = 0,
    Walk,
    Run,
    Dash,
    Attack,
    Death,
    Hurt
}

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] public Animator animator;
    [SerializeField] public Transform rootTransform;
    [SerializeField] private Transform firePoint;
    [SerializeField] public Transform TargetPoint;
    [SerializeField] private ParticleSystem dashParticle;
    [SerializeField] protected ParticleSystem hurtParticle;
    [SerializeField] protected ParticleSystem DeathParticle;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Currency currency;
    [SerializeField] public CharacterStats stats;
    [SerializeField] public Health health;
    [SerializeField] public Mana mana;
    [SerializeField] public Stamina stamina;
    [SerializeField] private Weapon weapon;
    [SerializeField] private RangeWeapon rangeWeapon;
    private StateMachine stateMachine;
    protected string dashSFX = "Dash";
    [SerializeField] protected string hurtSFX = "Hurt";

    private Action<Notify> OnStartGame;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        weapon = GetComponentInChildren<Weapon>();

        stateMachine = new StateMachine();
        var idle = new Idle(stateMachine, animator, this);
        var walk = new Walk(stateMachine, animator, this);
        var run = new Run(stateMachine, animator, this);
        var dash = new Dash(stateMachine, animator, this);

        stateMachine.Initialize(idle);
        stateMachine.AddAnyTransition(dash, () => InputManager.Instance.Dash && dash.CanDash && stamina.CurrentStamina >= stats.DashStaminaConsume);
        stateMachine.AddTransition(dash, idle, () => dash.DashEnd);
        stateMachine.AddTransition(idle, walk, () => InputManager.Instance.Move);
        stateMachine.AddTransition(walk, run, () => InputManager.Instance.Run && stamina.CurrentStamina >= 1f);
        stateMachine.AddTransition(walk, idle, () => !InputManager.Instance.Move);
        stateMachine.AddTransition(run, idle, () => !InputManager.Instance.Run || !InputManager.Instance.Move);
        stateMachine.AddTransition(run, walk, () => stamina.CurrentStamina == 0f);

        OnStartGame = thisNotify => Init();
    }

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventID.StartGame, OnStartGame);
        EventManager.Instance.AddListener(EventID.LevelChange, inventory.OnLevelChange);
        EventManager.Instance.AddListener(EventID.EquipmentChange, stats.OnEquipmentChange);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventID.StartGame, OnStartGame);
        EventManager.Instance.RemoveListener(EventID.LevelChange, inventory.OnLevelChange);
        EventManager.Instance.RemoveListener(EventID.EquipmentChange, stats.OnEquipmentChange);
    }

    private void Init()
    {
        animator.SetTrigger("Reset");
        currency.Init();
        stats.Init();
        health.Init(Death, Hurt);
        mana.Init(stats);
        stamina.Init(stats);
        inventory.Init();
        weapon.Init();
        rangeWeapon.firePoint = firePoint;
        transform.position = Vector3.zero;
    }

    private void Update()
    {
        stateMachine.OnFSMUpdate();
        Flip();
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
        if (Time.timeScale == 0.0f || stateMachine.currentState.GetType() == typeof(Dash))
            return;

        rootTransform.localScale = InputManager.Instance.MouseOnWorld.x < transform.position.x ? new Vector3(-1, 1, 1) : Vector3.one;
    }

    public void Hurt()
    {
        animator.SetTrigger("Hurt");
        AudioManager.Instance.PlaySFX(hurtSFX);
    }

    private void PlayHurtParticle()
    {
        hurtParticle.Play();
    }

    public void Death()
    {
        GameManager.Instance.GameOver();
        EventManager.Instance.Raise(EventID.PlayerDeath, null);
        animator.SetTrigger("Death");
        DeathParticle.Play();
        weapon.Destroy();
    }

    private void Destroy()
    {

    }

    public void PlayDashEffect(Vector2 dashDirection)
    {
        float angle = Vector2.SignedAngle(Vector2.right, dashDirection);
        dashParticle.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        dashParticle.Play();
        AudioManager.Instance.PlaySFX(dashSFX);
    }
}
