using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemKeeper : ScriptableObject
{
    [SerializeField] protected int size = 10;
    [field: SerializeField] public List<Item> Items { get; private set; }
    public bool isFull => Items.Count >= size;
    public virtual void Init()
    {
        if (Items == null) Items = new List<Item>();
        else Items.Clear();
        UpdateUI();
    }

    public virtual bool Add(Item item)
    {
        if (isFull) return false;

        Items.Add(item);
        UpdateUI();
        return true;
    }

    public virtual void Remove(int index)
    {
        if (index < Items.Count && index >= 0)
        {
            Items.RemoveAt(index);
            UpdateUI();
        }
    }

    public virtual bool Remove(Item item)
    {
        bool result = Items.Remove(item);
        UpdateUI();
        return result;
    }

    public virtual int Recycle(int index)
    {
        var item = Items[index];
        Remove(index);
        return item.recyclePrice;
    }

    public virtual int Recycle(Item item)
    {
        Remove(item);
        return item.recyclePrice;
    }

    public virtual void UpdateUI()
    {
        Sort();
        // Raise event
    }

    protected virtual void Sort()
    {
        Items = Items.OrderByDescending(item => item.Rarity).ThenBy(item => item.profile.id).ToList();
    }

    public virtual Item FindSimilarItem(Item other)
    {
        return Items.FirstOrDefault(item => item.Compare(other));
    }
}
