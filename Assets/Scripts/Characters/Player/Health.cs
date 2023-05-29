using System;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "Health", menuName = "Scriptable Object/Health")]
public class Health : ScriptableObject
{
    [SerializeField]
    private float currentHealth;

    [SerializeField]
    private float maxHealth;
    private float deffense;

    public static event Action OnGameOver;
    public static event Action<float, float, float> OnHealthChange; //(Change Value, Remain Health, Max Health)

    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        private set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
        }
    }

    public void Init(float maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        CharacterStats.OnStatsChange += OnStatsChange;
    }

    private void OnStatsChange(CharacterStats stats)
    {
        maxHealth = stats.MaxHealthPoint.Total;
        deffense = stats.Defense.Total;
        OnHealthChange?.Invoke(0, currentHealth, maxHealth);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= FinalDamage(amount, deffense);
        OnHealthChange?.Invoke(-amount, currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            OnGameOver?.Invoke();
            GameManager.Instance.GameOver();
        }
    }

    public void TakeHeal(float amount)
    {
        currentHealth += amount;
        OnHealthChange?.Invoke(amount, currentHealth, maxHealth);
    }

    private float FinalDamage(float damageReceive, float deffense)
    {
        float finaldamage = damageReceive * (1 - deffense / (10 + deffense));
        return finaldamage;
    }
}
