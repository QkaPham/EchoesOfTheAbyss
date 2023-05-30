using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollViewItem : MonoBehaviour, ISelectHandler
{
    public static event Action<ScrollViewItem> ScrollViewItemSelect;
    public void OnSelect(BaseEventData eventData)
    {
        ScrollViewItemSelect?.Invoke(this);
    }
}
