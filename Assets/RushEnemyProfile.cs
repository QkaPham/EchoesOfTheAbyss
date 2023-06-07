using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RushEnemyProfile", menuName = "Scriptable Object/Enemy/Rush Enemy Profile")]
public class RushEnemyProfile : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Aiming")]
    public float aimingRange = 5f;
    public float aimingTime = 2;
    public float delayAttackTime = 2f;

    [Header("Attack")]
    public float rushSpeed = 10;
    public float rushRange = 3;
    public float attackCooldownTime = 0.5f;
    public float damageDistance = .8f;
    public float timeBetweenEachDamage = 1f;
}
