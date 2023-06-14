using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Scriptable Object/Equipment")]
public class Equipment : ScriptableObject
{
    public int size;

    [field: SerializeField]
    public List<Item> Items { get; private set; }

    [SerializeField]
    private Currency currency;

    public void Init()
    {
        Items = new List<Item>();
        for (int index = 0; index < size; index++)
        {
            Items.Add(null);
            EventManager.Raise(EventID.EquipmentChange, new EquipmentChangeNotify(false, null, index));
        }
    }

    public bool Add(Item item)
    {
        int index = Items.FindIndex(item => item == null);
        if (index >= 0)
        {
            Items[index] = item;
            EventManager.Raise(EventID.EquipmentChange, new EquipmentChangeNotify(true, item, index));
            return true;
        }
        return false;
    }

    public void Remove(int index)
    {
        Item item = Items[index];
        EventManager.Raise(EventID.EquipmentChange, new EquipmentChangeNotify(false, item, index));
        Items[index] = null;
    }

    public void Recycle(int index)
    {
        Item item = Items[index];
        EventManager.Raise(EventID.EquipmentChange, new EquipmentChangeNotify(false, item, index));
        currency.Gain(item.recyclePrice);
        Items[index] = null;
    }
}
