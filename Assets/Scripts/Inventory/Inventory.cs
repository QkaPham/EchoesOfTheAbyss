using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using System;
using UnityEditor;

[CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable Object/New Inventory")]
public class Inventory : ScriptableObject
{
    protected int totalSize => inventorySize + equipmentSize;
    [SerializeField]
    protected int inventorySize = 10;
    [SerializeField]
    protected int equipmentSize = 1;

    [SerializeField]
    private List<Item> ownedItems;
    private List<Item> inventoryItems => ownedItems
                                        .Where(item => !item.isEquip)
                                        .OrderByDescending(item => item.Rarity)
                                        .ThenBy(item => item.profile.itemName).ToList();
    private List<Item> equimentItems => ownedItems
                                        .Where(i => i.isEquip)
                                        .OrderByDescending(item => item.Rarity)
                                        .ThenBy(item => item.profile.itemName).ToList();
    public bool isFull => totalSize <= ownedItems.Count();
    public bool isInventoryFull => inventorySize <= ownedItems.Where(item => !item.isEquip).Count();
    public bool isEquipmentFull => equipmentSize <= ownedItems.Where(item => item.isEquip).Count();

    private Action<Notify> OnLevelChange;

    private void OnEnable()
    {
        OnLevelChange = thisNotify =>
        {
            if (thisNotify is LevelChangeNotify notify)
            {
                equipmentSize = notify.level;
            }
        };

        EventManager.Instance.AddListener(EventID.LevelChange, OnLevelChange);
    }
    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventID.LevelChange, OnLevelChange);
    }

    public void Init()
    {
        if (ownedItems == null) ownedItems = new List<Item>();
        else ownedItems.Clear();
        OnChange();
    }


    public bool Add(Item item)
    {
        if (isFull) return false;

        item.isEquip = isInventoryFull;
        ownedItems.Add(item);
        OnChange();
        return true;
    }

    public bool Remove(Item item)
    {
        var result = ownedItems.Remove(item);
        OnChange();
        return result;
    }

    public bool Equip(Item item)
    {
        if (item == null) return false;
        if (item.isEquip || isEquipmentFull) return false;

        item.isEquip = true;
        OnChange();
        return true;
    }

    public bool UnEquip(Item item)
    {
        if (item == null) return false;
        if (!item.isEquip || isInventoryFull) return false;

        item.isEquip = false;
        OnChange();
        return true;
    }


    public int Recycle(Item item)
    {
        if (item == null) return 0;
        Remove(item);
        return item.price;
    }

    public void Upgrade(Item item)
    {
        item.Upgrade();
        OnChange();
    }

    private void OnChange()
    {
        Sort();
        EventManager.Instance.Raise(EventID.InventoryChange, new InventoryChangeNotify(inventoryItems));
        EventManager.Instance.Raise(EventID.EquipmentChange, new EquipmentChangeNotify(equimentItems));
    }

    public Item FindSimilarItem(Item item)
    {
        var similarItem = ownedItems.FirstOrDefault(i => i.Compare(item));
        return similarItem;
    }
    private void Sort()
    {
        ownedItems.OrderByDescending(item => item.Rarity);//.ThenBy(item => item.profile.itemName);
    }
}