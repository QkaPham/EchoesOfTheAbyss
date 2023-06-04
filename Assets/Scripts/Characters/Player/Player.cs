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

    private StateMachine playerStateMachine;
    private StateMachine weaponStateMachine;

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

    public static event Action PlayerDeath;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        weapon = GetComponentInChildren<Weapon>();

        playerStateMachine = new StateMachine();
        var idle = new Idle(playerStateMachine, animator, this);
        var walk = new Walk(playerStateMachine, animator, this);
        var run = new Run(playerStateMachine, animator, this);
        var dash = new Dash(playerStateMachine, animator, this);

        playerStateMachine.Initialize(idle);
        playerStateMachine.AddAnyTransition(dash, () => InputManager.Instance.Dash && dash.CanDash && stamina.CurrentStamina >= stats.DashStaminaConsume);
        playerStateMachine.AddTransition(dash, idle, () => dash.DashEnd);
        playerStateMachine.AddTransition(idle, walk, () => InputManager.Instance.Move);
        playerStateMachine.AddTransition(walk, run, () => InputManager.Instance.Run && stamina.CurrentStamina >= 1f);
        playerStateMachine.AddTransition(walk, idle, () => !InputManager.Instance.Move);
        playerStateMachine.AddTransition(run, idle, () => !InputManager.Instance.Run || !InputManager.Instance.Move);
        playerStateMachine.AddTransition(run, walk, () => stamina.CurrentStamina == 0f);
    }

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        GameManager.OnStartGame += Init;
    }

    private void OnDisable()
    {
        GameManager.OnStartGame -= Init;
    }

    private void Init()
    {
        //gameObject.SetActive(true);
        animator.SetTrigger("Reset");
        currency.Init();
        stats.Init();
        health.Init(stats, this);
        mana.Init(stats);
        stamina.Init(stats);
        inventory.Init();
        equipment.Init();
        weapon.Init();
        rangeWeapon.firePoint = firePoint;
    }

    private void Update()
    {
        playerStateMachine.OnFSMUpdate();
        Flip();
        stamina.Regenerate();
        mana.Regenerate();

        TestCode();
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
        if (Time.timeScale == 0.0f || playerStateMachine.currentState.GetType() == typeof(Dash))
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
        PlayerDeath?.Invoke();
        animator.SetTrigger("Death");
        DeathParticle.Play();
        weapon.Destroy();
    }

    private void Destroy()
    {
        GameManager.Instance.GameOver();
        //weapon.gameObject.SetActive(false);
    }

    public void PlayDashEffect(Vector2 dashDirection)
    {
        float angle = Vector2.SignedAngle(Vector2.right, dashDirection);
        dashParticle.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        dashParticle.Play();
    }


    private void TestCode()
    {
        //if(Input.GetKeyDown(KeyCode.P)) {
        //    health.TakeDamage(1);
        //}
        //if(Input.GetKeyDown(KeyCode.O))
        //{
        //    health.TakeDamage(1000);
        //}
        //if(Input.GetKeyDown(KeyCode.I))
        //{
        //    Init();
        //}
    }
}
