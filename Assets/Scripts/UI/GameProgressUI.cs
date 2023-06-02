using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameProgressUI : MonoBehaviour
{
    [SerializeField]
    protected Sprite pin;

    [SerializeField]
    protected Sprite defeat;

    [SerializeField]
    private Color defeatColor;
    public void UpdateProgress(int round)
    {
        Image image = transform.GetChild(round - 1).GetChild(0).GetComponent<Image>();
        image.sprite = pin;
        image.color = defeatColor;
        if (round >= 2)
        {
            Image image1 = transform.GetChild(round - 2).GetChild(0).GetComponent<Image>();
            image1.sprite = defeat;
        }
    }
}
