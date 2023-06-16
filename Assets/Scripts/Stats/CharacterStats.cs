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
    public int level = 1;
    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
            EventManager.Raise(EventID.LevelChange, new LevelChangeNotify(level, LevelUpCost));
        }
    }
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

    public static event Action<int, int> OnLevelChange;

    private Action<Notify> OnEquipmentChange;
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
        RemoveAllModifier();

        OnEquipmentChange = thisNotify =>
        {
            if (thisNotify is EquipmentChangeNotify notify)
            {
                //if (notify.isEquip)
                //{
                //    AddModifiers(notify.item);
                //}
                //else
                //{
                //    RemoveModifiers(notify.item);
                //}
                ModifierSources.Clear();
                foreach (var item in notify.items)
                {
                    AddModifiers(item);
                }
            }
        };
    }

    private void OnEnable()
    {
        EventManager.AddListiener(EventID.EquipmentChange, OnEquipmentChange);
    }

    public void AddModifiers(Item source)
    {
        ModifierSources.Add(source);
        UpdateBonusStats();
        EventManager.Raise(EventID.StatsChange, new StatsChangeNotify(this));
    }

    public void RemoveModifiers(Item source)
    {
        ModifierSources.Remove(source);
        UpdateBonusStats();
        EventManager.Raise(EventID.StatsChange, new StatsChangeNotify(this));
    }

    private void RemoveAllModifier()
    {
        ModifierSources.Clear();
        EventManager.Raise(EventID.StatsChange, new StatsChangeNotify(this));
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

    public void LevelUp()
    {
        Level++;
        Attack.Base += AttackGrowth;
        Defense.Base += DefenseGrowth;
        MaxHealthPoint.Base += MaxHealthGrowth;
        CriticalHitChance.Base += CritGrowth;
        CriticalHitDamage.Base += CritDamageGrowth;
        Haste.Base += HasteGrowth;
        EventManager.Raise(EventID.StatsChange, new StatsChangeNotify(this));
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