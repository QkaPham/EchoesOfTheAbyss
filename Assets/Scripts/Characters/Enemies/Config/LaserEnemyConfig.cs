using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "LaserEnemyProfile", menuName = "Scriptable Object/Enemy/Laser Enemy Profile")]
public class LaserEnemyConfig : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float fleeDistance = 2f;

    [Header("Aiming")]
    public float aimingRange = 5f;
    public float aimingTime = 2;
    public float delayAttackTime = 2;

    [Header("Attack")]
    public LayerMask PlayerLayerMask;
    public float laserRange = 15f;
    public float attackCooldownTime = 0.5f;
    public float attackTimeDelayByHurt = 2f;
}
