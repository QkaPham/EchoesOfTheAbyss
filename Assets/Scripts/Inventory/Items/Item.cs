using System;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.Progress;

[Serializable]
public class Item
{
    public ItemProfile profile;
    [SerializeField]
    private int rarity;
    public int Rarity
    {
        get
        {
            return rarity;
        }
        set
        {
            rarity = value;
            UpDateModifiers(rarity);
        }
    }

    public int quantityValue => (int)Mathf.Pow(2, Rarity - 1);

    public List<Modifier> modifiers;
    public int recyclePrice => profile.recyclePrice[Rarity - 1];
    public int price => profile.price[Rarity - 1];
    public bool isMaxUpgrade => Rarity >= profile.maxRarity;
    public Color backGroundColor => profile.backGroundColor[Rarity - 1];

    public Item(ItemProfile profile, int rarity)
    {
        this.profile = profile;
        this.rarity = rarity;

        modifiers = new List<Modifier>();
        if (profile != null)
        {
            foreach (var config in profile.modifierConfig)
            {
                Modifier modifier = new Modifier(config.modifier);
                modifier.amount += config.TotalValue(rarity);
                modifiers.Add(modifier);
            }
        }
    }

    public bool Upgrade()
    {
        if (isMaxUpgrade) return false;
        Rarity++;
        return true;
    }

    private void UpDateModifiers(int rarity)
    {
        for (int i = 0; i < modifiers.Count; i++)
        {
            modifiers[i].amount = profile.modifierConfig[i].TotalValue(rarity);
        }
    }

    public bool Compare(Item other)
    {
        return other.profile == profile && other.Rarity == Rarity;
    }

    public string ModifierType()
    {
        StringBuilder builder = new StringBuilder();
        foreach (var modifier in modifiers)
        {
            switch (modifier.statType)
            {
                case StatType.Attack:
                    builder.Append($"Atk\n");
                    break;
                case StatType.Defense:
                    builder.Append($"Def\n");
                    break;
                case StatType.MaxHealthPoint:
                    builder.Append($"Hp\n");
                    break;
                case StatType.CriticalHitChance:
                    builder.Append($"Crit\n");
                    break;
                case StatType.CriticalHitDamage:
                    builder.Append($"Crit Dmg\n");
                    break;
                case StatType.Haste:
                    builder.Append($"Haste\n");
                    break;
                default:
                    break;
            }
        }
        return builder.ToString();
    }

    public string ModifierValue()
    {
        StringBuilder builder = new StringBuilder();
        foreach (var modifier in modifiers)
        {
            switch (modifier.modifierType)
            {
                case global::ModifierType.Flat:
                    builder.Append($"{modifier.amount}\n");
                    break;
                case global::ModifierType.PercentMultiply:
                    builder.Append($"{modifier.amount * 100}%\n");
                    break;
                case global::ModifierType.PercentAdd:
                    builder.Append($"{modifier.amount * 100}%\n");
                    break;
                default:
                    break;
            }


        }
        return builder.ToString();
    }

}


