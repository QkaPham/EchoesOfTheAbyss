using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private Currency currency;
    [SerializeField] private List<ItemProfile> itemProfiles;
    [SerializeField] private RollingConfig rollingConfig;
    [SerializeField] private Inventory inventory;
    [SerializeField] private RollButton rollButton;

    private List<ShopSlot> slots;
    private List<RarityChance> rollingChance;

    private List<Item> rollingPool;
    private List<Item> sellItems;
    private List<Item> junkItems;// contain item used to upgrade and waiting for return to rolling pool;

    private Action<Notify> OnRoundEnd, OnLevelChange;

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

        OnRoundEnd = thisNotify =>
        {
            Roll();
            rollingConfig.ResetRollingCost();
            rollButton.SetPriceText(rollingConfig.RollingCost);
        };
        OnLevelChange = thisNotify => { if (thisNotify is LevelChangeNotify notify) rollingChance = rollingConfig.RollingChanceMap[notify.level]; };
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventID.RoundEnd, OnRoundEnd);
        EventManager.Instance.AddListener(EventID.LevelChange, OnLevelChange);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventID.RoundEnd, OnRoundEnd);
        EventManager.Instance.RemoveListener(EventID.LevelChange, OnLevelChange);
    }

    public void Buy(int index)
    {
        Item newItem = slots[index].item;
        if (newItem == null) return;

        Item similarItem = inventory.FindSimilarItem(newItem);

        if (!inventory.isFull && (similarItem == null || similarItem.isMaxUpgrade))
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
        if (inventory.isFull && similarItem == null && !newItem.isMaxUpgrade)
        {
            for (int i = 0; i < 4; i++) // 4 is (MaxRarity - 1)
            {
                List<Item> upgradeRequires = HigherUpgradeRequire(newItem, i + 1);

                similarItem = inventory.FindSimilarItem(new Item(newItem.profile, i + 1));
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
        result?.Add(item);
        return result;
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
        inventory.Add(item);
        sellItems.Remove(item);
    }

    private void Upgrade(Item item)
    {
        inventory.Upgrade(item);
        if (item.isMaxUpgrade) return;

        inventory.Remove(item);
        Item similarItem = inventory.FindSimilarItem(item);
        if (similarItem != null)
        {
            Upgrade(similarItem);
        }
        else
        {
            inventory.Add(item);
        }
    }

    public void OnRollButtonClick()
    {
        if (currency.Use(rollingConfig.RollingCost))
        {
            rollingConfig.IncreaseRollCost();
            rollButton.SetPriceText(rollingConfig.RollingCost);

            Roll();

            //if (rollingConfig.RollingCost > currency.Balance)
            //{
            //    rollButton.Insufficient();
            //}
            //else
            //{
            //    rollButton.Sufficient();
            //}
        }
    }

    public void Roll()
    {
        rollingPool.AddRange(sellItems);
        sellItems.Clear();

        for (int i = 0; i < slots.Count(); i++)
        {
            Item item = RandomItem();
            item.Rarity = RandomRarity();

            sellItems.Add(item);
            slots[i].UpdateShopSlot(item);
        }
    }

    public void Recycle(Item item)
    {
        if (item == null) return;
        rollingPool.Add(item);
        ReturnRollingPool(item.profile, item.quantityValue - 1);

        //if (rollingConfig.RollingCost > currency.Balance)
        //{
        //    rollButton.Insufficient();
        //}
        //else
        //{
        //    rollButton.Sufficient();
        //}
    }

    protected int RandomRarity()
    {
        float totalChance = rollingChance.Sum(item => item.chance);
        var random = UnityEngine.Random.Range(0f, totalChance);
        float cumulativeChance = 0f;

        foreach (var raritychange in rollingChance)
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
        items.ForEach(item => junkItems.Remove(item));
        rollingPool.AddRange(items);
    }

    protected Item RandomItem()
    {
        Item item = rollingPool[UnityEngine.Random.Range(0, rollingPool.Count)];
        rollingPool.Remove(item);
        return item;
    }
}
