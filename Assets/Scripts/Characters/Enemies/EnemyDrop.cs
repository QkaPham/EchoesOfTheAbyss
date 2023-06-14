using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyDrop : MonoBehaviour
{
    [SerializeField] protected int fragmentDrop = 5000;
    [SerializeField] protected int fragmentGrowth = 5000;

    [SerializeField] protected List<ItemProfile> itemProfiles;
    [SerializeField] protected List<RarityChance> rarityChances;

    [SerializeField] protected float dropChanceGrowth = 0.05f;
    [SerializeField] protected int maxRarity;

    [SerializeField] protected GameObject CollectibleItemPrefab;

    protected ObjectPool<Fragment> fragmentPool;
    protected ObjectPool<CollectibleItem> collectibleItemPool;

    public void Drop()
    {
        var item = collectibleItemPool.Get();
        item.transform.position = transform.position;
        item.item = new Item(RandomItemProfile(), RandomRarity());


        var fragment = fragmentPool.Get();
        fragment.transform.position = this.transform.position;
        fragment.Amount = fragmentDrop;
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

    protected ItemProfile RandomItemProfile()
    {
        return itemProfiles[UnityEngine.Random.Range(0, itemProfiles.Count)];
    }


    [Serializable]
    public class RarityChance
    {
        public int rarity;
        public float chance;
        public float growth;
    }
}
