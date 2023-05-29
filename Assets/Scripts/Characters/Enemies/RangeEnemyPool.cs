using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyPool : BasePool<BasePoolableEnemy>
{
    [SerializeField]
    protected EnemyBulletPool enemyBulletPool;

    protected override BasePoolableEnemy Create()
    {
        var rangeEnemy = Instantiate(Prefabs, this.transform).GetComponent<RangeEnemy>();
        rangeEnemy.SetPool(pool);
        rangeEnemy.bulletPool = enemyBulletPool.pool;
        return rangeEnemy;
    }
}
