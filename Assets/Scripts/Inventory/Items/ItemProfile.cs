using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/Item Profile")]
public class ItemProfile : ScriptableObject
{
    public ItemID id;
    public string itemName;
    public string description;
    public Sprite icon;
    public int maxRarity;
    public int[] recyclePrice;
    public int[] price;
    public Color[] backGroundColor;
    public List<ModiferConfig> modifierConfig;

    [Serializable]
    public class ModiferConfig
    {
        public float TotalValue(int rarity) => modifier.amount + (rarity - 1) * growth;
        public float growth;
        public Modifier modifier;
    }
}

public enum ItemID
{
    NoneItem = -1,
    Feather,
    Chalice,
    Necklace
}