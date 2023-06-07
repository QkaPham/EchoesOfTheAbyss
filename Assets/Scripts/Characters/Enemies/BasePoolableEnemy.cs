using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BasePoolableEnemy : BaseEnemy, PoolableObject<BasePoolableEnemy>
{
    protected override void OnEnable()
    {
        base.OnEnable();
        GameManager.OnStartGame += Destroy;
        GameManager.OnRoundEnd += Death;
        GameManager.OnVictory += Death;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        animator.SetTrigger("Reset");
        GameManager.OnStartGame -= Destroy;
        GameManager.OnRoundEnd -= Death;
        GameManager.OnVictory -= Death;
    }

    protected ObjectPool<BasePoolableEnemy> pool;

    public void SetPool(ObjectPool<BasePoolableEnemy> pool)
    {
        this.pool = pool;
    }

    public override void Destroy()
    {
        if (health.isDeath)
        {
            Drop();
        }
        Realease();
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
