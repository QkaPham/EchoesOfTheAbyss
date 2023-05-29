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
    public void Release()
    {
        pool.Release(this);
    }
}
