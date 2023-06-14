using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentChangeNotify : Notify
{
    public bool isEquip;
    public Item item;
    public int slot;

    public EquipmentChangeNotify(bool isEquip, Item item, int slot)
    {
        this.isEquip = isEquip;
        this.item = item;
        this.slot = slot;
    }
}
