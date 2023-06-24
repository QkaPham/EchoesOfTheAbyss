using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetLoader : Singleton<AssetLoader>
{
    //private Dictionary<ItemID,ItemProfile> Items;
    //protected override void Awake()
    //{
    //    ItemProfile[] items = Resources.LoadAll<ItemProfile>("Items");

    //    Items = new Dictionary<ItemID, ItemProfile>();

    //    foreach (ItemProfile item in items)
    //    {
    //        Items.Add(item.id, item);
    //    }
    //}
    //public ItemProfile GetItem(ItemID id)
    //{
    //    ItemProfile item;
    //    if (Items.TryGetValue(id, out item))
    //    {
    //        return item;
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Item with ID " + id + " not found in GameResources.");
    //        return null;
    //    }
    //}
}
