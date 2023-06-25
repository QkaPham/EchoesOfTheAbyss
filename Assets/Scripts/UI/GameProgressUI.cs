using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameProgressUI : MonoBehaviour
{
    [SerializeField] protected Sprite pin;
    [SerializeField] protected Sprite normal;
    [SerializeField] protected Sprite defeat;
    [SerializeField] protected Sprite boss;

    [SerializeField] protected Color bossRoundColor;
    [SerializeField] protected Color normalColor;
    [SerializeField] protected Color defeatColor;
    private void Awake()
    {
        Reset();
    }

    public void Reset()
    {
        for (int i = 0; i < 5; i++)
        {
            Image image = transform.GetChild(i).GetChild(0).GetComponent<Image>();
            if (i == 4)
            {
                image.color = bossRoundColor;
                image.sprite = boss;
            }
            else
            {
                image.sprite = normal;
                image.color = normalColor;
            }
        }
    }

    public void UpdateProgress(int round)
    {
        if (round >= 6) return;
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
