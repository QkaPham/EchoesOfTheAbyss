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
        if (ModifierType == ModifierType.Flat)
        {
            return $"{StatType} {Amount}";
        }
        else if (ModifierType == ModifierType.PercentMultiply || ModifierType == ModifierType.PercentAdd)
        {
            return $"{StatType} {Amount}%";
        }
        return base.ToString();
    }
}