using System;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class Item
{
    public ItemProfile profile;
    public bool isEquip;
    [SerializeField] private int rarity;
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
    public List<Modifier> modifiers;
    public int quantityValue => (int)Mathf.Pow(2, Rarity - 1);
    public int RecyclePrice => ItemConfig.GetRecyclePrice(Rarity);
    public int Price => ItemConfig.GetPrice(Rarity);
    public bool IsMaxUpgrade => Rarity >= ItemConfig.maxRarity;
    public Color DarkColor => ItemConfig.GetDarkColor(Rarity);
    public Color LightColor => ItemConfig.GetLightColor(Rarity);
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
        if (IsMaxUpgrade) return false;
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

    public string ModifierStat()
    {
        StringBuilder builder = new StringBuilder();
        foreach (var modifier in modifiers)
        {
            switch (modifier.statType)
            {
                case StatID.Attack:
                    builder.Append($"Atk\n");
                    break;
                case StatID.Defense:
                    builder.Append($"Def\n");
                    break;
                case StatID.MaxHealthPoint:
                    builder.Append($"Hp\n");
                    break;
                case StatID.CriticalHitChance:
                    builder.Append($"Crit\n");
                    break;
                case StatID.CriticalHitDamage:
                    builder.Append($"Crit Dmg\n");
                    break;
                case StatID.Haste:
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


