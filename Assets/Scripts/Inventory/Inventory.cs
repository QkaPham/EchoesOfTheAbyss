using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable Object/InventorySystem")]
public class Inventory : ScriptableObject
{
    public int size;

    [field: SerializeField]
    public List<Item> Items { get; private set; }

    public void Init()
    {
        if (Items == null) Items = new List<Item>();
        else Items.Clear();
        Update();
    }

    private void Add(Item item)
    {
        Items.Add(item);
        Update();
    }

    public void Remove(int index)
    {
        Items.RemoveAt(index);
        Update();
    }

    public void Remove(Item item)
    {
        Items.Remove(item);
        Update();
    }

    public int Recycle(int index)
    {
        var item = Items[index];
        Remove(index);
        return item.recyclePrice;
    }

    public int Recycle(Item item)
    {
        Remove(item);
        return item.recyclePrice;
    }

    public void MergeAdd(Item newItem)
    {
        var similarItem = Items.FirstOrDefault(item => item.Equals(newItem));

        if (similarItem != null)
        {
            Remove(similarItem);
            similarItem.Upgrade();
            MergeAdd(similarItem);
        }
        else
        {
            Add(newItem);
        }
    }
    private void Update()
    {
        Sort();
        EventManager.Raise(EventID.InventoryChange, new InventoryChangeNotify(Items));
    }

    private void Sort()
    {
        Items.OrderBy(s => s.rarity).ThenBy(s => s.profile.id);
    }

    public Item FindSimilarItem(Item item)
    {
        return Items.FirstOrDefault(i => i.Equals(item));
    }
}