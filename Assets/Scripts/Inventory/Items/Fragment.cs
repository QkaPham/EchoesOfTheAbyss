using UnityEngine;
using UnityEngine.Pool;

public class Fragment : MonoBehaviour
{
    private ObjectPool<Fragment> pool;
    public int Amount;
    public void SetPool(ObjectPool<Fragment> pool) => this.pool = pool;
    public void Release()
    {
        pool.Release(this);
    }
}
