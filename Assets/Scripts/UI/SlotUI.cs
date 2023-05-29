using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    public bool HasItem { get; protected set; }
    public int Index { get; protected set; }
    public virtual SlotType SlotType { get; protected set; }
    protected virtual void Awake()
    {
        Index = transform.GetSiblingIndex();
    }
    public virtual void UpdateUISlot(Item item)
    {
        
    }
}
