using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : EnemyHealth
{
    public static event Action<BossHealth> OnHealthChange;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    public override void Init(BaseEnemy enemy)
    {
        base.Init(enemy);
        OnHealthChange?.Invoke(this);
    }
    public override void TakeDamage(float damageAmount, bool isCritHit)
    {
        base.TakeDamage(damageAmount, isCritHit);
        OnHealthChange?.Invoke(this);
    }
}
