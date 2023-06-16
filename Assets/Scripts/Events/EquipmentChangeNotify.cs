using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentChangeNotify : Notify
{
    public List<Item> items;

    public EquipmentChangeNotify(List<Item> items)
    {
        this.items = items;
    }
}
