using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResources : Singleton<GameResources>
{
    private Dictionary<ItemID,Item> Items;
    protected override void Awake()
    {
        Item[] items = Resources.LoadAll<Item>("Items");

        Items = new Dictionary<ItemID, Item>();

        foreach (Item item in items)
        {
            Items.Add(item.ID, item);
        }
    }
    public Item GetItem(ItemID id)
    {
        Item item;
        if (Items.TryGetValue(id, out item))
        {
            return item;
        }
        else
        {
            Debug.LogWarning("Item with ID " + id + " not found in GameResources.");
            return null;
        }
    }
}
