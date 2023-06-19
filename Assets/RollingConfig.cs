using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RollingConfig", menuName = "Scriptable Object/Rolling Config")]
public class RollingConfig : SerializedScriptableObject
{
    [SerializeField]
    private int baseRollingCost = 20;
    public int costGrowth;

    private int rollingTimes = 0;
    public int RollingCost => baseRollingCost + rollingTimes * costGrowth;

    public Dictionary<int, List<RarityChance>> RollingChanceMap = new Dictionary<int, List<RarityChance>>
    {
        {1, new List<RarityChance>(){
            new RarityChance(1, 1f) } },

        {2, new List<RarityChance>(){
            new RarityChance(1, 0.9f),
            new RarityChance(2, 0.1f) } },

        {3, new List<RarityChance>(){
            new RarityChance(1, 0.85f),
            new RarityChance(2, 0.15f) } },

        {4, new List<RarityChance>(){
            new RarityChance(1, 0.75f),
            new RarityChance(2, 0.2f),
            new RarityChance(3, 0.05f) } },

        {5, new List<RarityChance>(){
            new RarityChance(1, 0.625f),
            new RarityChance(2, 0.3f),
            new RarityChance(3, 0.075f) } },

        {6, new List<RarityChance>(){
            new RarityChance(1, 0.43f),
            new RarityChance(2, 0.45f),
            new RarityChance(3, 0.1f),
            new RarityChance(4, 0.02f) } },

        {7, new List<RarityChance>(){
            new RarityChance(1, 0.37f),
            new RarityChance(2, 0.45f),
            new RarityChance(3, 0.15f),
            new RarityChance(4, 0.03f) } },

        {8, new List<RarityChance>(){
            new RarityChance(1, 0.32f),
            new RarityChance(2, 0.4f),
            new RarityChance(3, 0.23f),
            new RarityChance(4, 0.04f),
            new RarityChance(5, 0.01f) } },

        {9, new List<RarityChance>(){
            new RarityChance(1, 0.225f),
            new RarityChance(2, 0.35f),
            new RarityChance(3, 0.35f),
            new RarityChance(4, 0.06f),
            new RarityChance(5, 0.015f) } },

        {10, new List<RarityChance>(){
             new RarityChance(1, 0.14f),
             new RarityChance(2, 0.25f),
             new RarityChance(3, 0.5f),
             new RarityChance(4, 0.09f),
             new RarityChance(5, 0.02f) } },
    };

    public void ResetRollingCost()
    {
        rollingTimes = 0;
    }

    public void IncreaseRollCost()
    {
        rollingTimes++;
    }
}

[Serializable]
public class RarityChance
{
    public int rarity;
    public float chance;

    public RarityChance(int rarity, float chance)
    {
        this.rarity = rarity;
        this.chance = chance;
    }
}
