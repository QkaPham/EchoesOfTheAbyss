using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollViewAutoScroll : MonoBehaviour
{
    [Header("View Port")]
    public RectTransform viewPort;
    public string VPselectName;
    public Vector2 VPrectSize;
    public Vector3 VPpos;


    [Header("Button")]
    public string selectName;
    public Vector2 rectSize;
    public Vector3 pos;
    public Vector2 anchor;
    public Vector2 pivot;

    private void Update()
    {
        GameObject select = EventSystem.current.currentSelectedGameObject;
        if (select != null)
        {
            selectName = (select.name);
            rectSize = select.GetComponent<RectTransform>().rect.size;
            pos = select.transform.localPosition;
            anchor = select.GetComponent<RectTransform>().anchoredPosition;
            pivot = select.GetComponent<RectTransform>().pivot;
        }

        VPrectSize = viewPort.rect.size;
        VPpos = viewPort.transform.localPosition;
    }
}
