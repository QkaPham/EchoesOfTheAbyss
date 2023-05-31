using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected Player player;
    protected Vector3 playerPositon => player.transform.position;

    [SerializeField]
    protected Transform rootTransform;
    protected Animator animator;
    protected bool isAttacking;
    protected void Awake()
    {
        player = GetComponentInParent<Player>();
        animator = GetComponent<Animator>();
        if (rootTransform == null)
        {
            foreach (Transform child in transform)
            {
                if (child.name == "Flip")
                {
                    rootTransform = child;
                    continue;
                }
            }
        }
    }

    protected void OnEnable()
    {
        CharacterStats.OnStatsChange += SpeedUpAttackAnimation;
        Attack.OnStartAttack += OnStartAttack;
        //Health.OnGameOver += () => gameObject.SetActive(false);
    }

    protected void OnDisable()
    {
        CharacterStats.OnStatsChange -= SpeedUpAttackAnimation;
        Attack.OnStartAttack -= OnStartAttack;
        //Health.OnGameOver -= () => gameObject.SetActive(false);
    }

    protected void OnStartAttack(CharacterStats stats)
    {
        isAttacking = true;
        RotateToMouse();
        animator.SetTrigger("Swing");
    }
    protected void OnEndAttack()
    {
        isAttacking = false;
    }

    protected void Update()
    {
        if (!isAttacking)
        {
            Flip();
            ResetRotation();
        }
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
}
