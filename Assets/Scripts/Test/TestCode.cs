using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    public RangeEnemy enemy;
    public Player player;
    public EnemyBulletPool pool;

    private void Awake()
    {
        enemy.Init(player, null, enemy.transform.position, null);
        enemy.bulletPool = pool.pool;
    }
}
