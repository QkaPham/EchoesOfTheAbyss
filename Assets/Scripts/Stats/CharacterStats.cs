using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum StatType
{
    Attack,
    Defense,
    MaxHealthPoint,
    CriticalHitChance,
    CriticalHitDamage,
    Haste
}

[CreateAssetMenu(fileName = "Stats", menuName = "Scriptable Object/Stats")]
public class CharacterStats : ScriptableObject
{
    [Header("Walk Stats")]
    public float WalkSpeed;

    [Header("Run Stats")]
    public float RunSpeed;
    public float RunStaminaConsume = 8f;

    [Header("Dash Stats")]
    public float DashSpeed;
    public float DashTime;
    public float DashCooldownTime;
    public float DashStaminaConsume = 20f;

    [Header("Attack Stats")]
    public float AttackTime;
    public float AttackMoveSpeed;
    public float BaseAttackCooldownTime;
    public float AttackCooldownTime => BaseAttackCooldownTime / (1 + Haste.Total / 100);

    [Header("Level Info")]
    public int Level = 1;

    [Header("Stamina")]
    public float MaxStamina = 100f;
    public float StaminaRecovery = 15;

    [Header("Mana")]
    public float MaxMana = 100f;
    public float ManaRecovery = 15;

    private List<Stat> stats = new List<Stat>();
    public Stat Attack;
    public Stat Defense;
    public Stat MaxHealthPoint;
    public Stat CriticalHitChance;
    public Stat CriticalHitDamage;
    public Stat Haste;
    public List<IModifierSource> ModifierSources = new List<IModifierSource>();

    public static event Action<CharacterStats> OnStatsChange;
    public static event Action<int, int> OnLevelChange;

    public void Init()
    {
        Level = 1;
        Attack = new Stat(StatType.Attack, 12);
        Defense = new Stat(StatType.Defense, 5);
        MaxHealthPoint = new Stat(StatType.MaxHealthPoint, 100);
        CriticalHitChance = new Stat(StatType.CriticalHitChance, 0.1f);
        CriticalHitDamage = new Stat(StatType.CriticalHitDamage, 0.5f);
        Haste = new Stat(StatType.Haste, 0.5f);

        stats.AddRange(new Stat[] { Attack, Defense, MaxHealthPoint, CriticalHitChance, CriticalHitDamage, Haste });
        OnLevelChange?.Invoke(Level, LevelUpCost);
        OnStatsChange?.Invoke(this);
    }

    private void OnEnable()
    {
        Equipment.OnEquipmentChange += (isAddItem, item, index) => OnEquipmentChange(isAddItem, item);
    }

    private void OnDisable()
    {
        Equipment.OnEquipmentChange -= (isAddItem, item, index) => OnEquipmentChange(isAddItem, item);
    }

    private void OnEquipmentChange(bool isAddItem, Item item)
    {
        if (isAddItem)
        {
            AddModifiers(item);
        }
        else
        {
            RemoveModifiers(item);
        }
    }
    public void AddModifiers(IModifierSource source)
    {
        ModifierSources.Add(source);
        UpdateBonusStats();
        OnStatsChange?.Invoke(this);
    }

    public void RemoveModifiers(IModifierSource source)
    {
        ModifierSources.Remove(source);
        UpdateBonusStats();
        OnStatsChange?.Invoke(this);
    }

    private void UpdateBonusStats()
    {
        foreach (Stat stat in stats)
        {
            float flatModifiers = ModifierSources.Sum(s => s.Modifiers
                .Where(m => m.StatType == stat.StatType && m.ModifierType == ModifierType.Flat)
                .Sum(m => m.Amount));

            float percentModifiers = ModifierSources.Sum(s => s.Modifiers
                .Where(m => m.StatType == stat.StatType && m.ModifierType == ModifierType.PercentMultiply)
                .Sum(m => m.Amount));

            stat.Modifier = stat.Base * percentModifiers / 100 + flatModifiers;
        }
    }

    public void LevelUp()
    {
        Level++;
        Attack.Base += 2;
        Defense.Base += 1;
        MaxHealthPoint.Base += 10;
        CriticalHitChance.Base += 0.01f;
        CriticalHitDamage.Base += 0.05f;
        OnStatsChange?.Invoke(this);
        OnLevelChange?.Invoke(Level, LevelUpCost);
    }

    public int LevelUpCost => (Level + 1) * 1000 + 3000;
}

[Serializable]
public class Stat
{
    [HideInInspector]
    public StatType StatType;
    public float Base;
    public float Modifier;
    [SerializeField]
    private float total;
    public float Total
    {
        get
        {
            total = Base + Modifier;
            return total;
        }
        set
        {
            total = value;
        }
    }
    public Stat(StatType statType, float baseValue)
    {
        StatType = statType;
        Base = baseValue;
    }
}