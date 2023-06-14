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
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    public Animator animator;

    [SerializeField]
    public Transform rootTransform;

    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    public Transform TargetPoint;

    [SerializeField]
    private ParticleSystem dashParticle;

    [SerializeField]
    protected ParticleSystem hurtParticle;

    [SerializeField]
    protected ParticleSystem DeathParticle;

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

    [SerializeField]
    private RangeWeapon rangeWeapon;

    private Action<Notify> OnRetry;

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

        OnRetry = thisNotify => Init();
    }

    private void Start()
    {
        Init();
        EventManager.AddListiener(EventID.StartGame, OnRetry);
    }

    private void Init()
    {
        animator.SetTrigger("Reset");
        currency.Init();
        stats.Init();
        health.Init(stats, Death, Hurt);
        mana.Init(stats);
        stamina.Init(stats);
        inventory.Init();
        equipment.Init();
        weapon.Init();
        rangeWeapon.firePoint = firePoint;
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
        if (Time.timeScale == 0.0f || stateMachine.currentState.GetType() == typeof(Dash))
            return;

        rootTransform.localScale = InputManager.Instance.MouseOnWorld.x < transform.position.x ? new Vector3(-1, 1, 1) : Vector3.one;
    }

    public void Hurt()
    {
        animator.SetTrigger("Hurt");
    }

    private void PlayHurtParticle()
    {
        hurtParticle.Play();
    }

    public void Death()
    {
        GameManager.Instance.GameOver();
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
    }
}
