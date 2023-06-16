using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnItemManager : MonoBehaviour
{
    public Inventory inventory;
    public Equipment equipment;

    private void Awake()
    {
        inventory.Init();
        equipment.Init();
    }

    public bool isFull => inventory.isFull && equipment.isFull;

    public void Add(Item item)
    {
        if (item != null)
        {
            if (!inventory.isFull)
            {
                inventory.Add(item);
            }
            else
            {
                equipment.Add(item);
            }
        }
    }

    public bool Remove(Item item)
    {
        if (inventory.Remove(item))
        {
            Debug.Log("Remove inventory item");
            return true;
        }
        if (equipment.Remove(item))
        {
            Debug.Log("Remove equipment item");
            return true;
        }
        return false;
    }

    public void UpdateUI()
    {
        inventory.UpdateUI();
        equipment.UpdateUI();
    }

    public Item FindSimilarItem(Item item)
    {
        var similarItem = inventory.FindSimilarItem(item);
        if (similarItem != null)
        {
            Debug.Log("Found similar item");
            return similarItem;
        }

        similarItem = equipment.FindSimilarItem(item);
        if (similarItem != null)
        {
            Debug.Log("Found similar item");
            return similarItem;
        }

        Debug.Log("Not Found similar item");
        return null;
    }
}
