using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "RarityColor", menuName = "Scriptable Object/Rarity Color")]
public class ItemConfig : SerializedScriptableObject
{
    [ShowInInspector] public const int maxRarity = 5;
    [ShowInInspector] public static int[] itemPrices = { 52, 89, 162, 292, 525 };
    [ShowInInspector] public static int[] itemRecyclePrices = { 41, 72, 130, 234, 421 };
    [ShowInInspector]
    public static Color[] darkColors = { new Color(128 / 255f, 128 / 255f, 128 / 255f, 1),
                                         new Color(0 / 255f, 76 / 255f, 54 / 255f, 1),
                                         new Color(0 / 255f, 45 / 255f, 89 / 255f, 1),
                                         new Color(71 / 255f, 23 / 255f, 102 / 255f, 1),
                                         new Color(115 / 255f, 45 / 255f, 31 / 255f, 1)};
    [ShowInInspector]
    public static Color[] lightColors = { new Color(255 / 255f, 255 / 255f, 255 / 255f, 1),
                                          new Color(0 / 255f, 178 / 255f, 125 / 255f, 1),
                                          new Color(0 / 255f, 110 / 255f, 218 / 255f, 1),
                                          new Color(157 / 255f, 51 / 255f, 230 / 255f, 1),
                                          new Color(242 / 255f, 94 / 255f, 64 / 255f, 1)};

    private void OnEnable()
    {
        if (darkColors.Count() < maxRarity) { Debug.LogError("ItemConfig not enough preset value"); };
        if (lightColors.Count() < maxRarity) { Debug.LogError("ItemConfig not enough preset value"); };
        if (itemPrices.Count() < maxRarity) { Debug.LogError("ItemConfig not enough preset value"); };
        if (itemRecyclePrices.Count() < maxRarity) { Debug.LogError("ItemConfig not enough preset value"); };
    }

    public static int GetPrice(int rarity) => itemPrices[rarity - 1];
    public static int GetRecyclePrice(int rarity) => itemRecyclePrices[rarity - 1];
    public static Color GetDarkColor(int rarity) => darkColors[rarity - 1];
    public static Color GetLightColor(int rarity) => lightColors[rarity - 1];
}
