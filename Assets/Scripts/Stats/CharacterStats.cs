using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum StatID
{
    Attack,
    Defense,
    MaxHealthPoint,
    CriticalHitChance,
    CriticalHitDamage,
    Haste,
    Dodge,
    Luck,
    Regen,
    Speed
}

[CreateAssetMenu(fileName = "Stats", menuName = "Scriptable Object/Stats")]
public class CharacterStats : ScriptableObject
{
    [Header("Level")]
    [SerializeField] private int level = 1;
    [SerializeField] private int maxLevel = 10;
    public bool isMaxLevel => level >= maxLevel;
    public int Level
    {
        get
        {
            return level;
        }
        private set
        {
            level = Mathf.Clamp(value, 1, maxLevel);
            EventManager.Instance.Raise(EventID.LevelChange, new LevelChangeNotify(level, LevelUpCost));
        }
    }
    public int LevelUpCost => Level * 10 + 40;

    [Header("Starting Stats")]
    public int BaseAttack = 12;
    public int BaseDefense = 5;
    public int BaseMaxHealth = 100;
    public float BaseCrit = 0.1f;
    public float BaseCritDamage = 0.5f;
    public float BaseHaste = 0.5f;
    public float BaseDodge = 0.0f;
    public float BaseLuck = 0.0f;
    public float BaseRegen = 0.0f;


    [Header("Stats Growth")]
    public int AttackGrowth = 2;
    public int DefenseGrowth = 1;
    public int MaxHealthGrowth = 10;
    public float CritGrowth = 0.01f;
    public float CritDamageGrowth = 0.05f;
    public float HasteGrowth = 0.05f;

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

    [Header("Stamina")]
    public float MaxStamina = 100f;
    public float StaminaRecovery = 10f;
    public float DelayStaminaRecoverTime = 1f;

    [Header("Mana")]
    public float MaxMana = 100f;
    public float ManaRecovery = 10f;
    public float DelayManaRecoverTime = 1f;

    [Header("HP")]
    public float HPRecovery = 5f;

    private List<Stat> stats = new List<Stat>();
    public Stat Attack;
    public Stat Defense;
    public Stat MaxHealthPoint;
    public Stat CriticalHitChance;
    public Stat CriticalHitDamage;
    public Stat Haste;
    public List<Item> ModifierSources = new List<Item>();

    private Action<Notify> OnEquipmentChange;
    private void OnEnable()
    {
        OnEquipmentChange = thisNotify =>
        {
            if (thisNotify is EquipmentChangeNotify notify)
            {
                ClearModifier();
                foreach (var item in notify.items)
                {
                    AddModifiers(item);
                }
            }
        };

        EventManager.Instance.AddListener(EventID.EquipmentChange, OnEquipmentChange);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(EventID.EquipmentChange, OnEquipmentChange);
    }

    public void Init()
    {
        Level = 1;
        Attack = new Stat(StatID.Attack, BaseAttack);
        Defense = new Stat(StatID.Defense, BaseDefense);
        MaxHealthPoint = new Stat(StatID.MaxHealthPoint, BaseMaxHealth);
        CriticalHitChance = new Stat(StatID.CriticalHitChance, BaseCrit);
        CriticalHitDamage = new Stat(StatID.CriticalHitDamage, BaseCritDamage);
        Haste = new Stat(StatID.Haste, BaseHaste);

        stats.AddRange(new Stat[] { Attack, Defense, MaxHealthPoint, CriticalHitChance, CriticalHitDamage, Haste });
        ClearModifier();
    }

    public void AddModifiers(Item source)
    {
        ModifierSources.Add(source);
        UpdateBonusStats();
        EventManager.Instance.Raise(EventID.StatsChange, new StatsChangeNotify(this));
    }

    public void RemoveModifiers(Item source)
    {
        ModifierSources.Remove(source);
        UpdateBonusStats();
        EventManager.Instance.Raise(EventID.StatsChange, new StatsChangeNotify(this));
    }

    private void ClearModifier()
    {
        ModifierSources.Clear();
        UpdateBonusStats();
        EventManager.Instance.Raise(EventID.StatsChange, new StatsChangeNotify(this));
    }

    private void UpdateBonusStats()
    {
        foreach (Stat stat in stats)
        {
            float flatModifiers = ModifierSources.Sum(s => s.modifiers
                .Where(m => m.statType == stat.StatType && m.modifierType == ModifierType.Flat)
                .Sum(m => m.amount));

            float percentModifiers = ModifierSources.Sum(s => s.modifiers
                .Where(m => m.statType == stat.StatType && m.modifierType == ModifierType.PercentMultiply)
                .Sum(m => m.amount));

            float percentaddModifiers = ModifierSources.Sum(s => s.modifiers
                .Where(m => m.statType == stat.StatType && m.modifierType == ModifierType.PercentAdd)
                .Sum(m => m.amount));

            stat.Modifier = stat.Base * percentModifiers + flatModifiers + percentaddModifiers;
        }
    }

    public bool LevelUp()
    {
        if (level >= maxLevel) return false;

        Level++;
        Attack.Base += AttackGrowth;
        Defense.Base += DefenseGrowth;
        MaxHealthPoint.Base += MaxHealthGrowth;
        CriticalHitChance.Base += CritGrowth;
        CriticalHitDamage.Base += CritDamageGrowth;
        Haste.Base += HasteGrowth;
        EventManager.Instance.Raise(EventID.StatsChange, new StatsChangeNotify(this));
        return true;
    }
}

[Serializable]
public class Stat
{
    [HideInInspector]
    public StatID StatType;
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
    public Stat(StatID statType, float baseValue)
    {
        StatType = statType;
        Base = baseValue;
    }
}