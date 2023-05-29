using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class BasePool<T> : MonoBehaviour where T : MonoBehaviour, PoolableObject<T>
{
    [SerializeField]
    protected GameObject Prefabs;
    public ObjectPool<T> pool;
    protected virtual void Awake()
    {
        pool = new ObjectPool<T>(Create, OnGet, OnRelease);
    }

    protected virtual T Create()
    {
        var poolableobject = Instantiate(Prefabs, this.transform).GetComponent<T>();
        poolableobject.SetPool(pool);
        return poolableobject;
    }

    protected virtual void OnGet(T poolableobject)
    {
        poolableobject.gameObject.SetActive(true);
    }

    protected virtual void OnRelease(T poolableobject)
    {
        poolableobject.gameObject.SetActive(false);
    }

    public virtual T Get()
    {
        return pool.Get();
    }
}
