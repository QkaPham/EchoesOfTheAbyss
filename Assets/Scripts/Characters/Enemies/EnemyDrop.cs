using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;
using System.Collections;

public class EnemyDrop : MonoBehaviour
{
    [SerializeField]
    protected int fragmentDrop = 5000;

    protected ObjectPool<Fragment> fragmentPool;

    [SerializeField]
    protected GameObject CollectibleItemPrefab;

    [SerializeField]
    protected List<Item> itemsDrop;

    [SerializeField]
    protected float dropChance = 0.5f;

    public void Drop()
    {
        var random = Random.value;
        if (random < dropChance)
        {
            if (CollectibleItemPrefab != null)
            {
                var item = Instantiate(CollectibleItemPrefab, transform.position, Quaternion.identity).GetComponent<CollectibleItem>();
            }
        }

        var fragment = fragmentPool.Get();
        fragment.transform.position = this.transform.position;
        fragment.Amount = fragmentDrop;
    }
}
