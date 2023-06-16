using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RarityColor", menuName = "Scriptable Object/Rarity Color")]
public class RarityColor : ScriptableObject
{
    public List<Color> darkColors;
    public List<Color> lightColors;
    public Color DarkColor(int rarity) => darkColors[rarity - 1];
    public Color LightColor(int rarity) => lightColors[rarity - 1];
}
