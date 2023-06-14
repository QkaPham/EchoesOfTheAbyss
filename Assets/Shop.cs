using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class Shop : MonoBehaviour
{
    public Inventory inventory;
    public Equipment equipment;
    public Currency currency;

    public int slot;
    public List<Item> sellItems = new List<Item>();
    public List<ItemProfile> itemProfiles;
    public List<RarityChance> rarityChances;

    public List<Item> rollingPool;
    public List<Item> PulledPool;

    private void Awake()
    {
        rollingPool.Clear();
        foreach (var profile in itemProfiles)
        {
            for (int i = 0; i < 20; i++)
            {
                rollingPool.Add(new Item(profile, 1));
            }
        }
        Roll();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            currency.Gain(111111);
            Roll();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Buy(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Buy(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Buy(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Buy(3);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Sell(inventory.Items[0]);
        }
    }

    public void Buy(int index)
    {
        Item newItem = sellItems[index];
        if (newItem == null) return;
        if (currency.Use(newItem.price))
        {
            sellItems[index] = null;

            RollingPoolRemove(newItem);
            Item similarItem;
            similarItem = inventory.FindSimilarItem(newItem);
            if (similarItem != null && !newItem.isMaxUpgrade)
            {
                Upgrade(similarItem);
                return;
            }
            BuyNewItem(newItem);
        }
    }

    private void BuyNewItem(Item item)
    {
        inventory.Items.Add(item);
    }

    private void Upgrade(Item item)
    {
        item.Upgrade();
        inventory.Remove(item);

        Item similarItem = inventory.FindSimilarItem(item);
        if (similarItem != null && !item.isMaxUpgrade)
        {
            Upgrade(similarItem);
        }
        else
        {
            inventory.Items.Add(item);
        }
    }

    private void Roll()
    {
        sellItems.Clear();
        for (int i = 0; i < slot; i++)
        {
            Item item = RandomItem();
            item.rarity = RandomRarity();

            sellItems.Add(item);
        }
    }

    public void Sell(Item item)
    {
        currency.Gain(inventory.Recycle(item));

        RollingPoolAdd(item.profile, (int)Mathf.Pow(2, item.rarity - 1));
    }

    protected int RandomRarity()
    {
        float totalChance = rarityChances.Sum(item => item.chance);
        var random = UnityEngine.Random.Range(0f, totalChance);
        float cumulativeChance = 0f;

        foreach (var raritychange in rarityChances)
        {
            cumulativeChance += raritychange.chance;
            if (random <= cumulativeChance)
            {
                return raritychange.rarity;
            }
        }
        return 0;
    }

    private void RollingPoolRemove(Item item)
    {
        rollingPool.Remove(item);
        PulledPool.Add(item);
    }

    private void RollingPoolAdd(ItemProfile profile, int quantity)
    {
        List<Item> items = PulledPool.Where(item => item.profile == profile).Take(quantity).ToList();
        items.ForEach(item => PulledPool.Remove(item));
        rollingPool.AddRange(items);
    }

    protected Item RandomItem()
    {
        return rollingPool[UnityEngine.Random.Range(0, rollingPool.Count)];
    }

    [Serializable]
    public class RarityChance
    {
        public int rarity;
        public float chance;
        public float growth;
    }
}
