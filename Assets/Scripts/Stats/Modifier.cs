using JetBrains.Annotations;
using System;
using UnityEngine;
public enum ModifierType
{
    Flat,
    PercentMultiply,
    PercentAdd
}
[Serializable]
public class Modifier
{
    public StatType statType;
    public ModifierType modifierType;
    public float amount;
    public Modifier(StatType statType, ModifierType modifierType, float amount)
    {
        this.statType = statType;
        this.modifierType = modifierType;
        this.amount = amount;
    }

    public Modifier(Modifier modifier)
    {
        statType = modifier.statType;
        modifierType = modifier.modifierType;
        amount = modifier.amount;
    }

    public override string ToString()
    {
        string type = "";
        switch (statType)
        {
            case StatType.Attack:
                type = "Atk";
                break;
            case StatType.Defense:
                type = "Def";
                break;
            case StatType.MaxHealthPoint:
                type = "HP";
                break;
            case StatType.CriticalHitChance:
                type = "Crit Rate";
                break;
            case StatType.CriticalHitDamage:
                type = "Crit Dmg";
                break;
            case StatType.Haste:
                type = "Haste";
                break;
            default:
                break;
        }
        if (modifierType == ModifierType.Flat)
        {
            return $"{type} {amount}";
        }
        else if (modifierType == ModifierType.PercentMultiply || modifierType == ModifierType.PercentAdd)
        {
            return $"{type} {amount * 100}%";
        }
        return base.ToString();
    }
}