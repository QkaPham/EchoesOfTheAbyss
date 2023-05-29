using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "Equipment", menuName = "Scriptable Object/Equipment")]
public class Equipment : ScriptableObject
{
    public int size;

    [field: SerializeField]
    public List<Item> Items { get; private set; }

    [SerializeField]
    private Currency currency;

    public static event Action<bool, Item, int> OnEquipmentChange;

    public void Init()
    {
        Items = new List<Item>();
        for (int index = 0; index < size; index++)
        {
            Items.Add(null);
            OnEquipmentChange?.Invoke(false, null, index);
        }
    }

    public bool Add(Item item)
    {
        int index = Items.FindIndex(item => item == null);
        if (index >= 0)
        {
            Items[index] = item;
            OnEquipmentChange?.Invoke(true, item, index);
            
            return true;
        }
        return false;
    }

    public void Remove(int index)
    {
        Item item = Items[index];
        OnEquipmentChange?.Invoke(false, item, index);
        Items[index] = null;
    }

    public void Recycle(int index)
    {
        Item item = Items[index];
        OnEquipmentChange?.Invoke(false, item, index);
        currency.Gain(item.RecyclePrice);
        Items[index] = null;
    }
}
