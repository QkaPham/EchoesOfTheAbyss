using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Item
{
    public ItemProfile profile;
    public int rarity;
    public List<Modifier> modifiers;
    public int recyclePrice => profile.recyclePrice[rarity];
    public int price => profile.price[rarity];
    public bool isMaxUpgrade => rarity >= profile.maxRarity;


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
                modifier.amount += config.growth * rarity;
                modifiers.Add(modifier);
            }
        }
    }

    public bool Upgrade()
    {
        if (isMaxUpgrade) return false;

        rarity++;
        for (int i = 0; i < modifiers.Count; i++)
        {
            modifiers[i].amount += profile.modifierConfig[i].growth;
        }
        return true;
    }
    public override bool Equals(object obj)
    {
        if (obj is Item item)
        {
            return item.profile == profile && item.rarity == rarity;
        }
        else
        {
            return false;
        }
    }
}


