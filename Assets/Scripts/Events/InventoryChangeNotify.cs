using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryChangeNotify : Notify
{
    public List<Item> items;
    public InventoryChangeNotify(List<Item> items = null)
    {
        this.items = items;
    }
}
