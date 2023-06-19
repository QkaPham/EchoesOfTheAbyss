using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyDrop : MonoBehaviour
{
    [SerializeField] protected int fragmentDrop = 5000;
    [SerializeField] protected int fragmentGrowth = 5000;
    protected ObjectPool<Fragment> fragmentPool;

    public void Drop()
    {
        var fragment = fragmentPool.Get();
        fragment.transform.position = this.transform.position;
        fragment.Amount = fragmentDrop;
    }
}
