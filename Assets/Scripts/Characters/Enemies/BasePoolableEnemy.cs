using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BasePoolableEnemy : BaseEnemy, PoolableObject<BasePoolableEnemy>
{
    protected override void OnEnable()
    {
        base.OnEnable();
        GameManager.OnStartGame += Death;
        GameManager.OnRoundEnd += Death;
        GameManager.OnVictory += Death;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        animator.SetTrigger("Reset");
        GameManager.OnStartGame -= Death;
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
        Release();
    }

    public virtual void Release()
    {
        if (gameObject.activeSelf)
        {
            if (pool != null)
            {
                pool.Release(this);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
