using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ItemPool : MonoBehaviour
{
    [SerializeField]
    private GameObject collectibleItemPrefab;
    public ObjectPool<CollectibleItem> pool;
    void Awake()
    {
        pool = new ObjectPool<CollectibleItem>(CreateCollectible, OnGetCollectible, OnReleaseCollectible);
    }

    private CollectibleItem CreateCollectible()
    {
        var collectible = Instantiate(collectibleItemPrefab,transform).GetComponent<CollectibleItem>();
        collectible.SetPool(pool);
        return collectible;
    }

    private void OnGetCollectible(CollectibleItem collectible)
    {
        collectible.gameObject.SetActive(true);
    }

    private void OnReleaseCollectible(CollectibleItem collectible)
    {
        collectible.gameObject.SetActive(false);
    }
}
