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
    public StatID statType;
    public ModifierType modifierType;
    public float amount;


    public Modifier(StatID statType, ModifierType modifierType, float amount)
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
            case StatID.Attack:
                type = "Atk";
                break;
            case StatID.Defense:
                type = "Def";
                break;
            case StatID.MaxHealthPoint:
                type = "HP";
                break;
            case StatID.CriticalHitChance:
                type = "Crit Rate";
                break;
            case StatID.CriticalHitDamage:
                type = "Crit Dmg";
                break;
            case StatID.Haste:
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