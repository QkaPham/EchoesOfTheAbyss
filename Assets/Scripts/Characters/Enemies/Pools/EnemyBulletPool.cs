using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPool : BasePool<EnemyBullet>
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override EnemyBullet Create()
    {
        return base.Create();
    }
    protected override void OnGet(EnemyBullet poolableobject)
    {
        base.OnGet(poolableobject);
    }
    protected override void OnRelease(EnemyBullet poolableobject)
    {
        base.OnRelease(poolableobject);
    }
}
