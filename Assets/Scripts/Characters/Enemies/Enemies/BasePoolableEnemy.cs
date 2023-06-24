using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BasePoolableEnemy : BaseEnemy, PoolableObject<BasePoolableEnemy>
{
    Action<Notify> OnRoundEnd, OnVictory;
    protected override void Awake()
    {
        base.Awake();
        OnRoundEnd = thisNotify => Death();
        OnVictory = thisNotify => Death();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        EventManager.Instance.AddListener(EventID.RoundEnd, OnRoundEnd);
        EventManager.Instance.AddListener(EventID.Victory, OnVictory);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        animator.SetTrigger("Reset");
        EventManager.Instance.RemoveListener(EventID.RoundEnd, OnRoundEnd);
        EventManager.Instance.RemoveListener(EventID.Victory, OnVictory);
    }

    protected ObjectPool<BasePoolableEnemy> pool;

    public void SetPool(ObjectPool<BasePoolableEnemy> pool)
    {
        this.pool = pool;
    }

    public override void Destroy(float time = 0f)
    {
        if (health.isDeath)
        {
            Drop();
        }
        Realease(time);
    }

    public virtual void Realease(float delay = 0f)
    {
        StartCoroutine(DelayRealease(delay));
    }

    protected IEnumerator DelayRealease(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        if (gameObject.activeSelf)
        {
            pool.Release(this);
        }
    }
}
