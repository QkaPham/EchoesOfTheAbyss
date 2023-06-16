using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class NewInventory : MonoBehaviour
{
    public class InventoryItem
    {
        public Item item;
        public bool isEquip;
    }
    [SerializeField]
    protected int inventorySize = 10;
    protected int equipmentSize = 1;
    protected int size => inventorySize + equipmentSize;


    [SerializeField]
    private List<InventoryItem> items;

    public bool isFull => items.Count >= size;
    public bool isInventoryFull => items.Where(item => !item.isEquip).Count() >= inventorySize;
    public bool isEquipmentFull => items.Where(item => item.isEquip).Count() >= equipmentSize;
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (items == null) items = new List<InventoryItem>();
        else items.Clear();
    }

    public bool Add(Item item)
    {
        if(!isInventoryFull)
        {
            
        }
        return false;
    }

    public bool Remove(Item item)
    {

        return false;
    }

    public bool Equip(Item item)
    {
        return false;
    }

    public bool UnEquip(Item item)
    {
        return false;
    }

    public void UpdateUI()
    {

    }
}
