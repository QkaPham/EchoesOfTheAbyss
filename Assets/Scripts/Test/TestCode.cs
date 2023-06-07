using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestCode : MonoBehaviour
{
    public RangeEnemy enemy;
    public Player player;
    public EnemyBulletPool pool;
    public InputActionReference inputaction;

    public PlayerInput input;
    public InputActionRebindingExtensions.RebindingOperation rebinding;

    private void Awake()
    {
        enemy.Init(player, null, enemy.transform.position, null);
        enemy.bulletPool = pool.pool;
    }
}
