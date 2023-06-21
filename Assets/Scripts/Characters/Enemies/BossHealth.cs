using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : EnemyHealth
{
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    public override void Init(BaseEnemy enemy)
    {
        base.Init(enemy);
        EventManager.Instance.Raise(EventID.BossHealthChange, new HealthChangeNotify(CurrentHealth, maxHealth));
    }
    public override void TakeDamage(float damageAmount, bool isCritHit)
    {
        base.TakeDamage(damageAmount, isCritHit);
        EventManager.Instance.Raise(EventID.BossHealthChange, new HealthChangeNotify(CurrentHealth, maxHealth));
    }
}
