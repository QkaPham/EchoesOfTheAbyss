using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Health", menuName = "Scriptable Object/Health")]
public class Health : ScriptableObject
{
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    private float Recovery;
    private float deffense;

    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        private set
        {
            currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            EventManager.Raise(EventID.HealthChange, new HealthChangeNotify(currentHealth, maxHealth));
        }
    }
    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }
        private set
        {
            if (value > CurrentHealth)
            {
                CurrentHealth = value;
            }
            maxHealth = value;
            EventManager.Raise(EventID.HealthChange, new HealthChangeNotify(currentHealth, maxHealth));
        }
    }

    private event Action Death, Hurt;
    private Action<Notify> OnStatsChange;

    public void Init(CharacterStats stats, Action OnDeath, Action OnHurt)
    {
        MaxHealth = stats.MaxHealthPoint.Total;
        CurrentHealth = MaxHealth;
        Recovery = stats.HPRecovery;
        Death = OnDeath;
        Hurt = OnHurt;
        OnStatsChange = thisNotify => { if (thisNotify is StatsChangeNotify notify) UpdateStats(notify.stats); };
    }


    private void OnEnable()
    {
        EventManager.AddListener(EventID.StatsChange, OnStatsChange);
    }

    private void UpdateStats(CharacterStats stats)
    {
        MaxHealth = stats.MaxHealthPoint.Total;
        deffense = stats.Defense.Total;
    }

    private bool fullyRecovered => currentHealth == maxHealth;

    public void Regenerate()
    {
        if (!fullyRecovered)
        {
            CurrentHealth += Recovery * Time.deltaTime;
        }
    }

    public void TakeDamage(float amount)
    {
        if (CurrentHealth <= 0) return;
        CurrentHealth -= FinalDamage(amount, deffense);

        if (CurrentHealth <= 0)
        {
            Death?.Invoke();
        }
        else
        {
            Hurt?.Invoke();
        }
    }

    public void TakeHeal(float amount)
    {
        CurrentHealth += amount;
    }

    private float FinalDamage(float damageReceive, float deffense)
    {
        float finaldamage = damageReceive * (1 - deffense / (10 + deffense));
        return finaldamage;
    }
}
