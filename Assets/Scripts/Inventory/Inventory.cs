using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable Object/InventorySystem")]
public class Inventory : ScriptableObject
{
    public int size;

    [field: SerializeField]
    public List<Item> Items { get; private set; }

    [SerializeField]
    private Currency currency;

    public static event Action<Item, int> OnInventoryChange;


    private void OnEnable()
    {
        //UpgradePanel.OnNextRound += SortItems;
    }

    public void Init()
    {
        Items = new List<Item>();
        for (int index = 0; index < size; index++)
        {
            Items.Add(null);
            OnInventoryChange?.Invoke(null, index);
        }
    }

    public bool Add(Item item)
    {
        int index = Items.FindIndex(item => item == null);

        if (index >= 0)
        {
            Items[index] = item;
            OnInventoryChange?.Invoke(item, index);
            return true;
        }
        else
        {
            Items.Add(item);
            Debug.Log(Items.Count - 1);
            OnInventoryChange?.Invoke(item, Items.Count - 1);
            return true;
        }
        // return false;
    }

    public void Remove(int index)
    {
        var item = Items[index];
        OnInventoryChange?.Invoke(null, index);
        Items[index] = null;
    }

    public void Recycle(int index)
    {
        var item = Items[index];
        currency.Gain(item.RecyclePrice);
        OnInventoryChange?.Invoke(null, index);
        Items[index] = null;
    }

    private void SortItems()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items.Count == size) break;
            if (Items[i] == null)
            {
                Items.RemoveAt(i);
                i--;
            }
        }
    }
}