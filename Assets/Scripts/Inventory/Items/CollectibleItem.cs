using UnityEngine;
using UnityEngine.Pool;

public class CollectibleItem : MonoBehaviour, PoolableObject<CollectibleItem>
{
    public Item item;
    public ObjectPool<CollectibleItem> pool { get; set; }

    public void SetPool(ObjectPool<CollectibleItem> pool)
    {
        this.pool= pool;
    }
    public void Realease(float delay = 0f)
    {
        pool.Release(this);
    }
}
