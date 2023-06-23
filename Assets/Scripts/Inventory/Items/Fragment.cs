using System;
using UnityEngine;
using UnityEngine.Pool;

public class Fragment : MonoBehaviour
{
    private ObjectPool<Fragment> pool;
    public int Amount;
    public void SetPool(ObjectPool<Fragment> pool) => this.pool = pool;

    private Action<Notify> OnRetry;
    private void Awake()
    {
        OnRetry = thisNotify => Release();
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventID.Retry, OnRetry);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventID.Retry, OnRetry);
    }

    public void Release()
    {
        pool.Release(this);
    }
}
