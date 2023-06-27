using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
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

    private Action<Notify> OnStartGame;
    private void Awake()
    {
        OnStartGame = thisNotify => Init();
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventID.StartGame, OnStartGame);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventID.StartGame, OnStartGame);
    }

    public void Init()
    {
        for (int i = 0; i < 5; i++)
        {
            Image image = transform.GetChild(i).GetChild(0).GetComponent<Image>();
            if (i == 0)
            {
                image.sprite = pin;
                image.color = defeatColor;
            }
            else if (i == 4)
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
