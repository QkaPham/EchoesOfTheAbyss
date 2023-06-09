using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    protected Transform rootTransform;
    protected Animator animator;
    protected Player player;
    protected CharacterStats stats => player.stats;
    protected Vector3 playerPositon => player.transform.position;

    [SerializeField]
    protected float BaseAttackCooldown = 1f;
    [SerializeField]
    protected float damageMultifier = 1f;

    [SerializeField]
    protected HitBox hitbox;

    protected bool CanAttack => Time.time >= lastAttackTime + CooldownTime;
    protected bool EndAttack = true;
    protected float CooldownTime => BaseAttackCooldown / stats.Haste.Total;
    protected float lastAttackTime = 0f;

    protected void Awake()
    {
        player = GetComponentInParent<Player>();
        animator = GetComponent<Animator>();
        hitbox = GetComponentInChildren<HitBox>();
        if (rootTransform == null)
        {
            foreach (Transform child in transform)
            {
                if (child.name == "Root")
                {
                    rootTransform = child;
                    continue;
                }
            }
        }

        //Cooldown 10% when start game
        lastAttackTime = -CooldownTime * 1.1f;
    }

    protected void OnEnable()
    {
        CharacterStats.OnStatsChange += SpeedUpAttackAnimation;
    }

    protected void OnDisable()
    {
        CharacterStats.OnStatsChange -= SpeedUpAttackAnimation;
    }


    protected void Update()
    {
        if (InputManager.Instance.Attack && CanAttack)
        {
            Attack();
        }
        if (EndAttack)
        {
            ResetRotation();
            Flip();
        }
    }

    private void Attack()
    {
        EndAttack = false;
        lastAttackTime = Time.time;
        RotateToMouse();
        animator.SetTrigger("Swing");
        AudioManager.Instance.PlaySE("Slash");
        hitbox.DealDamage(stats, damageMultifier);
    }

    protected void OnEndAttack()
    {
        EndAttack = true;
    }

    protected void RotateToMouse()
    {
        Vector2 direction = InputManager.Instance.MouseOnWorld - (Vector2)transform.position;
        float angle = Vector2.SignedAngle(Vector2.up, direction);
        angle += (InputManager.Instance.MouseOnWorld.x < playerPositon.x) ? -90f : 90f;
        rootTransform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    protected void Flip()
    {
        if (InputManager.Instance.MouseOnWorld.x < playerPositon.x)
        {
            rootTransform.localScale = new Vector3(-1f, 1f, 1f);
        }
        if (InputManager.Instance.MouseOnWorld.x > playerPositon.x)
        {
            rootTransform.localScale = Vector3.one;
        }
    }

    protected void ResetRotation()
    {
        if (rootTransform.rotation.z != 0f)
        {
            rootTransform.rotation = Quaternion.Lerp(rootTransform.rotation, Quaternion.identity, Time.deltaTime / 0.2f);
        }
    }

    protected void SpeedUpAttackAnimation(CharacterStats stats)
    {
        animator.SetFloat("AttackSpeedMultiplier", (1 + stats.Haste.Total));
    }

    public void Destroy()
    {
        animator.SetTrigger("Destroy");
    }

    public void Init()
    {
        animator.SetTrigger("Reset");
    }
}
