using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TestButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public Image image;

    public void OnPointerClick(PointerEventData eventData)
    {
        image.color = Color.white;
        Debug.Log("OnPointerClick");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        image.color = Color.cyan;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.white;
        Debug.Log("OnPointerExit");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        image.color = Color.white;
        Debug.Log("OnPointerUp");
    }

    public void Click()
    {
        image.DOColor(Color.cyan, 0.1f).OnComplete(() => image.DOColor(Color.white, 0.1f));
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Click();
        }
    }
}
