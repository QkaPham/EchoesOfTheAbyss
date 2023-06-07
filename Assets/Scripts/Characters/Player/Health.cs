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
            OnHealthChange?.Invoke(this);
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
            OnHealthChange?.Invoke(this);
        }
    }

    public static event Action<Health> OnHealthChange;
    private event Action Death, Hurt;

    public void Init(CharacterStats stats, Action OnDeath, Action OnHurt)
    {
        MaxHealth = stats.MaxHealthPoint.Total;
        CurrentHealth = MaxHealth;
        Recovery = stats.HPRecovery;
        Death = OnDeath;
        Hurt = OnHurt;

    }

    private void OnEnable()
    {
        CharacterStats.OnStatsChange += OnStatsChange;
    }

    private void OnDisable()
    {
        CharacterStats.OnStatsChange -= OnStatsChange;
    }

    private void OnStatsChange(CharacterStats stats)
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
