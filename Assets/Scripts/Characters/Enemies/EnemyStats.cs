using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Object/Enemy/Stats")]
public class EnemyStats : ScriptableObject
{
    [Header("Base Stats")]
    public float baseAttack = 1;
    public float baseMaxHealth = 15f;
    public int baseFragment = 5000;

    [Header("Stats Growth")]
    [SerializeField]
    private float attackGrowth = 5;
    [SerializeField]
    private float maxHealthGrowth = 10;
    [SerializeField]
    public int fragmentGrowth = 54;

    [Header("Stats")]
    public float totalAttack;
    public float totalMaxHealth;
    public int totalfragment;

    private Action<Notify> OnRetry, OnRoundChange;

    private void Reset()
    {
        Init();
    }

    public void Init()
    {
        totalAttack = baseAttack;
        totalMaxHealth = baseMaxHealth;
        totalfragment = baseFragment;
    }

    public void StatsGrowth()
    {
        totalAttack += attackGrowth;
        totalMaxHealth += maxHealthGrowth;
        totalfragment += fragmentGrowth;
    }
}
