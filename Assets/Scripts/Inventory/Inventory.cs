using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable Object/InventorySystem")]
public class Inventory : ItemKeeper
{
    public override void UpdateUI()
    {
        base.UpdateUI();
        EventManager.Raise(EventID.InventoryChange, new InventoryChangeNotify(Items));
    }
    //[SerializeField] private int size = 10;
    //[field: SerializeField] public List<Item> Items { get; private set; }
    //public bool isFull => Items.Count >= size;

    //public void Init()
    //{
    //    if (Items == null) Items = new List<Item>();
    //    else Items.Clear();
    //    UpdateUI();
    //}

    //public bool Add(Item item)
    //{
    //    if (isFull) return false;

    //    Items.Add(item);
    //    UpdateUI();
    //    return true;
    //}

    //public void Remove(int index)
    //{
    //    if (index < Items.Count && index >= 0)
    //    {
    //        Items.RemoveAt(index);
    //        UpdateUI();
    //    }
    //}

    //public bool Remove(Item item)
    //{
    //    bool result = Items.Remove(item);
    //    UpdateUI();
    //    return result;
    //}

    //public int Recycle(int index)
    //{
    //    var item = Items[index];
    //    Remove(index);
    //    return item.recyclePrice;
    //}

    //public int Recycle(Item item)
    //{
    //    Remove(item);
    //    return item.recyclePrice;
    //}

    ////public void Upgrade(Item item)
    ////{
    ////    item.Upgrade();
    ////    UpdateUI();
    ////}

    //public void UpdateUI()
    //{
    //    Sort();
    //    EventManager.Raise(EventID.InventoryChange, new InventoryChangeNotify(Items));
    //}

    //private void Sort()
    //{
    //    Items = Items.OrderByDescending(s => s.Rarity).ThenBy(s => s.profile.id).ToList();
    //}

    //public Item FindSimilarItem(Item item)
    //{
    //    return Items.FirstOrDefault(i => i.Compare(item));
    //}
}