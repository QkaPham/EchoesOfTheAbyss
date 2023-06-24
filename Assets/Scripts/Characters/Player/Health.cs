using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    private float recovery;
    private float deffense;
    private bool stopRegenerate;

    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        private set
        {
            currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            EventManager.Instance.Raise(EventID.HealthChange, new HealthChangeNotify(currentHealth, maxHealth));
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
            EventManager.Instance.Raise(EventID.HealthChange, new HealthChangeNotify(currentHealth, maxHealth));
        }
    }

    private event Action Death, Hurt;
    private Action<Notify> OnStatsChange, OnRoundEnd, OnStartNextRound;

    private void Awake()
    {
        OnStatsChange = thisNotify =>
        {
            if (thisNotify is StatsChangeNotify notify) UpdateStats(notify.stats);
        };
        OnRoundEnd = thisNotify => stopRegenerate = true;
        OnStartNextRound = thisNotify => stopRegenerate = false;
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventID.StatsChange, OnStatsChange);
        EventManager.Instance.AddListener(EventID.RoundEnd, OnRoundEnd);
        EventManager.Instance.AddListener(EventID.StartNextRound, OnStartNextRound);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventID.StatsChange, OnStatsChange);
        EventManager.Instance.RemoveListener(EventID.RoundEnd, OnRoundEnd);
        EventManager.Instance.RemoveListener(EventID.StartNextRound, OnStartNextRound);
    }

    private void Update()
    {
        Regenerate();
    }

    public void Init(Action OnDeath, Action OnHurt)
    {
        CurrentHealth = MaxHealth;
        Death = OnDeath;
        Hurt = OnHurt;
    }

    private void UpdateStats(CharacterStats stats)
    {
        MaxHealth = stats.MaxHealthPoint.Total;
        deffense = stats.Defense.Total;
        recovery = stats.HPRecovery;
    }

    private bool fullyRecovered => currentHealth == maxHealth;

    public void Regenerate()
    {
        if (!fullyRecovered && !stopRegenerate)
        {
            CurrentHealth += recovery * Time.deltaTime;
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
