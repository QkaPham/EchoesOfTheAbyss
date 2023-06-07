using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Object/Item")]
public class Item : ScriptableObject, IModifierSource
{
    public ItemID ID;
    public string Name;
    public string Description;
    public Sprite Icon;
    public int MaxStackSize;
    public int RecyclePrice;
    [SerializeField] 
    private List<Modifier> modifiers;
    public List<Modifier> Modifiers { get => modifiers; set => modifiers = value; }
    public Item(Item other)
    {
        ID = other.ID;
        Name = other.Name;
        Description = other.Description;
        Icon = other.Icon;
        MaxStackSize = other.MaxStackSize;
        modifiers = new List<Modifier>(other.modifiers);
    }
}
public enum ItemID
{
    NoneItem = -1,
    Feather,
    Chalice,
    Necklace
}

