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
    [Header("Level")]
    public int Level = 1;
    public int LevelUpCost => Level * 1000 + 4000;

    [Header("Starting Stats")]
    public int BaseAttack = 12;
    public int BaseDefense = 5;
    public int BaseMaxHealth = 100;
    public float BaseCrit = 0.1f;
    public float BaseCritDamage = 0.5f;
    public float BaseHaste = 0.5f;

    [Header("Stats Growth")]
    public int AttackGrowth = 2;
    public int DefenseGrowth = 1;
    public int MaxHealthGrowth = 10;
    public float CritGrowth = 0.01f;
    public float CritDamageGrowth = 0.5f;
    public float HasteGrowth = 0.5f;

    [Header("Abilities")]
    [Header("Move")]
    public float BaseMoveSpeed = 4f;

    [Header("Run")]
    public float RunSpeed = 7f;
    public float RunStaminaConsume = 15f;

    [Header("Dash")]
    public float DashSpeed = 20f;
    public float DashTime = 0.2f;
    public float DashCooldownTime = 2f;
    public float DashStaminaConsume = 20f;

    [Header("Melee Attack")]
    public float AttackDamageMultifier = 1f;
    public float AttackTime = 0.2f;
    public float AttackMoveSpeed = 2f;
    public float BaseAttackCooldownTime = 1f;
    public float AttackCooldownTime => BaseAttackCooldownTime / (1 + Haste.Total / 100);

    [Header("Range Attack")]
    public float RangeAttackDamageMultifier = 0.5f;
    public float BaseRangeAttackCooldownTime = 1f;
    public float RangeAttackManaConsume = 20f;

    [Header("Stamina")]
    public float MaxStamina = 100f;
    public float StaminaRecovery = 15;
    public float DelayStaminaRecoverTime = 1f;

    [Header("Mana")]
    public float MaxMana = 100f;
    public float ManaRecovery = 15;
    public float DelayManaRecoverTime = 1f;

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

    private void Reset()
    {
        Init();
    }

    public void Init()
    {
        Level = 1;
        Attack = new Stat(StatType.Attack, BaseAttack);
        Defense = new Stat(StatType.Defense, BaseDefense);
        MaxHealthPoint = new Stat(StatType.MaxHealthPoint, BaseMaxHealth);
        CriticalHitChance = new Stat(StatType.CriticalHitChance, BaseCrit);
        CriticalHitDamage = new Stat(StatType.CriticalHitDamage, BaseCritDamage);
        Haste = new Stat(StatType.Haste, BaseHaste);

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
        Attack.Base += AttackGrowth;
        Defense.Base += DefenseGrowth;
        MaxHealthPoint.Base += MaxHealthGrowth;
        CriticalHitChance.Base += CritGrowth;
        CriticalHitDamage.Base += CritDamageGrowth;
        Haste.Base += HasteGrowth;
        OnStatsChange?.Invoke(this);
        OnLevelChange?.Invoke(Level, LevelUpCost);
    }
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