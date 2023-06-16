using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField]
    private Button rollButton;

    //[SerializeField]
    // private Inventory inventory;
    // [SerializeField]
    // private Equipment equipment;
    [SerializeField]
    private Currency currency;

    [SerializeField]
    private int maxSlot;
    [SerializeField]
    private List<ShopSlot> slots;
    [SerializeField]
    private List<ItemProfile> itemProfiles;
    [SerializeField]
    private List<RarityChance> rarityChances;

    [SerializeField]
    private List<Item> rollingPool;
    [SerializeField]
    private List<Item> sellItems;
    [SerializeField]
    private List<Item> junkItems;// contain item used to upgrade and waiting for return to rolling pool;
    [SerializeField]
    private OwnItemManager ownItem;

    private void Awake()
    {
        rollingPool = new List<Item>();
        sellItems = new List<Item>();
        junkItems = new List<Item>();

        rollingPool.Clear();
        foreach (var profile in itemProfiles)
        {
            // Each profile add 20 Item copies to rolling pool
            for (int i = 0; i < 20; i++)
            {
                rollingPool.Add(new Item(profile, 1));
            }
        }
        slots = GetComponentsInChildren<ShopSlot>().ToList();
        maxSlot = slots.Count();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            currency.Gain(20);
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
        if (Input.GetKeyDown(KeyCode.P))
        {
            Sell(ownItem.inventory.Items.FirstOrDefault());
        }
    }

    public void Buy(int index)
    {
        Item newItem = slots[index].item;
        if (newItem == null) return;

        Item similarItem = ownItem.FindSimilarItem(newItem);

        if (!ownItem.isFull && (similarItem == null || similarItem.isMaxUpgrade))
        {
            if (currency.Use(newItem.price))
            {
                slots[index].UpdateShopSlot(null);
                BuyNewItem(newItem);
                return;
            }
        }

        // buy upgrade
        if (similarItem != null && !newItem.isMaxUpgrade)
        {
            if (currency.Use(newItem.price))
            {
                slots[index].UpdateShopSlot(null);
                Upgrade(similarItem);

                sellItems.Remove(newItem);
                junkItems.Add(newItem);
                return;
            }
        }

        // try buy higher upgrade 
        if (ownItem.isFull && similarItem == null && !newItem.isMaxUpgrade)
        {
            for (int i = 0; i < 4; i++) // 4 is (MaxRarity - 1)
            {
                List<Item> upgradeRequires = HigherUpgradeRequire(newItem, i + 1);

                similarItem = ownItem.FindSimilarItem(new Item(newItem.profile, i + 1));
                if (upgradeRequires != null && similarItem != null)
                {
                    if (currency.Use(upgradeRequires.Sum(i => i.price)))
                    {
                        foreach (var item in upgradeRequires)
                        {
                            var slot = slots.FirstOrDefault(slot => slot.item == item);

                            junkItems.Add(item);
                            sellItems.Remove(item);
                            slot.UpdateShopSlot(null);
                        }
                        Upgrade(similarItem);
                        return;
                    }
                    break;
                }
            }
            return;
        }

    }

    private List<Item> HigherUpgradeRequire(Item item, int targetRarity)
    {
        int targetValue = (int)Mathf.Pow(2, targetRarity - 1);
        int remainValue = targetValue - item.quantityValue;

        List<Item> candidates = sellItems.Where(i => i.profile == item.profile).ToList();
        candidates.Remove(item);
        candidates.OrderBy(i => i.quantityValue);

        List<Item> current = new List<Item>();
        List<List<Item>> answer = new List<List<Item>>();
        BackTracking(answer, current, 0, 0, remainValue, candidates);

        List<Item> result = answer.FirstOrDefault();
        if (result != null)
        {
            result.Add(item);
            return result;
        }
        else return null;
    }

    public void BackTracking(List<List<Item>> answer, List<Item> current, int i, int total, int target, List<Item> candidates)
    {
        if (total == target)
        {
            answer.Add(new List<Item>(current));
            return;
        }
        if (total > target)
        {
            return;
        }
        if (i == candidates.Count)
        {
            return;
        }
        current.Add(candidates[i]);
        total = total + candidates[i].quantityValue;
        BackTracking(answer, current, i + 1, total, target, candidates);
        current.RemoveAt(current.Count - 1);
        total = total - candidates[i].quantityValue;
        while ((i + 1) < candidates.Count && candidates[i] == candidates[i + 1])
        {
            i = i + 1;
        }
        BackTracking(answer, current, i + 1, total, target, candidates);
    }


    private void BuyNewItem(Item item)
    {
        ownItem.Add(item);
        sellItems.Remove(item);
    }

    private void Upgrade(Item item)
    {
        item.Upgrade();
        ownItem.UpdateUI();
        if (item.isMaxUpgrade) return;

        ownItem.Remove(item);
        Item similarItem = ownItem.FindSimilarItem(item);
        if (similarItem != null)
        {
            Upgrade(similarItem);
        }
        else
        {
            ownItem.Add(item);
        }
    }

    public void Roll()
    {
        rollingPool.AddRange(sellItems);
        sellItems.Clear();

        for (int i = 0; i < maxSlot; i++)
        {
            Item item = RandomItem();
            item.Rarity = RandomRarity();

            sellItems.Add(item);
            slots[i].UpdateShopSlot(item);
        }
    }

    public void Sell(Item item)
    {
        if (item == null) return;
        currency.Gain(ownItem.inventory.Recycle(item));

        rollingPool.Add(item);
        ReturnRollingPool(item.profile, item.quantityValue);
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

    private void ReturnRollingPool(ItemProfile profile, int quantity)
    {
        List<Item> items = junkItems.Where(item => item.profile == profile).Take(quantity).ToList();
        foreach (var item in ownItem.inventory.Items)
        {
            items.Remove(item);
        }
        items.ForEach(item => junkItems.Remove(item));
        rollingPool.AddRange(items);
    }

    protected Item RandomItem()
    {
        Item item = rollingPool[UnityEngine.Random.Range(0, rollingPool.Count)];
        rollingPool.Remove(item);
        return item;
    }

    [Serializable]
    public class RarityChance
    {
        public int rarity;
        public float chance;
        public float growth;
    }
}
