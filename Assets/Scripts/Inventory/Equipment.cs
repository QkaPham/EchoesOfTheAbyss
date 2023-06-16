using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "Equipment", menuName = "Scriptable Object/Equipment")]
public class Equipment : ItemKeeper
{
    public override void UpdateUI()
    {
        base.UpdateUI();
        EventManager.Raise(EventID.EquipmentChange, new EquipmentChangeNotify(Items));
    }
    //public int size;

    //[field: SerializeField]
    //public List<Item> Items { get; private set; }

    //[SerializeField]
    //private Currency currency;
    //public bool isFull => Items.Count >= size;

    //public void Init()
    //{
    //    if (Items == null) Items = new List<Item>();
    //    else Items.Clear();
    //    UpdateUI();
    //}

    ////public bool Add(Item item)
    ////{
    ////    int index = Items.FindIndex(item => item == null);
    ////    if (index >= 0)
    ////    {
    ////        Items[index] = item;
    ////        EventManager.Raise(EventID.EquipmentChange, new EquipmentChangeNotify(true, item, index));
    ////        return true;
    ////    }
    ////    return false;
    ////}
    //public bool Add(Item item)
    //{
    //    if (isFull) return false;

    //    Items.Add(item);
    //    UpdateUI();
    //    return true;
    //}

    //public bool Remove(Item item)
    //{
    //    bool result = Items.Remove(item);
    //    UpdateUI();
    //    return result;
    //}

    //public void Remove(int index)
    //{
    //    Item item = Items[index];
    //    EventManager.Raise(EventID.EquipmentChange, new EquipmentChangeNotify(false, item, index));
    //    Items[index] = null;
    //}

    //public void Recycle(int index)
    //{
    //    Item item = Items[index];
    //    EventManager.Raise(EventID.EquipmentChange, new EquipmentChangeNotify(false, item, index));
    //    currency.Gain(item.recyclePrice);
    //    Items[index] = null;
    //}

    //public Item FindSimilarItem(Item item)
    //{
    //    return Items.FirstOrDefault(i => i.Compare(item));
    //}

    //private void Sort()
    //{
    //    Items = Items.OrderByDescending(s => s.Rarity).ThenBy(s => s.profile.id).ToList();
    //}
}
