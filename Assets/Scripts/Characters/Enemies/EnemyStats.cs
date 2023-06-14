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
    private float attackGrowth = 1;
    [SerializeField]
    private float maxHealthGrowth = 2f;
    [SerializeField]
    public float fragmentGrowth = 1.5f;

    [Header("Stats")]
    public float totalAttack;
    public float totalMaxHealth;
    public int totalfragment;


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
        totalAttack *= attackGrowth;
        totalMaxHealth *= maxHealthGrowth;
        totalfragment = (int)(totalfragment * fragmentGrowth);
    }
}
