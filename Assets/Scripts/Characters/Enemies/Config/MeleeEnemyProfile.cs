using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeeleeEnemyProfile", menuName = "Scriptable Object/Enemy/Melee Enemy Profile")]
public class MeleeEnemyProfile : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Attack")]
    public float attackRange = 1f;
    public float damageRange = 1.5f;
    public float attackCooldownTime = 1f;
}
