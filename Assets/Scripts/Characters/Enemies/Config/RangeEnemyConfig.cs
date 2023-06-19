using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangeEnemyProfile", menuName = "Scriptable Object/Enemy/Range Enemy Profile")]
public class RangeEnemyConfig : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float fleeRange = 2f;
    public float desireRange => (attackRange + fleeRange) / 2;

    [Header("Attack")]
    public float attackRange = 4f;
    public float bulletRange = 20f;
    public float attackCooldownTime = 1f;
    public float bulletSpeed = 3f;
    public float attackTimeDelayByHurt = 2f;
}
