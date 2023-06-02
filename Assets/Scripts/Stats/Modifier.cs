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
    public StatType StatType;
    public ModifierType ModifierType;
    public float Amount;
    public IModifierSource Source;
    public Modifier(IModifierSource source, StatType statType, ModifierType modifierType, float amount)
    {
        Source = source;
        StatType = statType;
        ModifierType = modifierType;
        Amount = amount;
    }
    public override string ToString()
    {
        string type = "";
        switch (StatType)
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
        if (ModifierType == ModifierType.Flat)
        {
            return $"{type} {Amount}";
        }
        else if (ModifierType == ModifierType.PercentMultiply || ModifierType == ModifierType.PercentAdd)
        {
            return $"{type} {Amount}%";
        }
        return base.ToString();
    }
}