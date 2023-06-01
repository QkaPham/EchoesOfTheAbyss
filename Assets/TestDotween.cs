using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TestDotween : MonoBehaviour
{
    //  RectTransform rectTransform;
    //public CanvasGroup canvasGroup;

    public Vector3 moveDirection;
    public float duration;
    public float delay;
    public bool snapping;
    //public Vector3 currentPosition;

    public Transform backgroundTrans;
    public Image bgImage;

    public Transform buttonsTrans;
    public CanvasGroup buttonsCanvasGroup;

    private void Awake()
    {
        ResetUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            buttonsTrans.DOMove(moveDirection + buttonsTrans.position, duration, snapping);
            //MoveUp();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            buttonsTrans.DOPlayBackwards();
           // ResetUI();
        }
    }

    private void MoveUp()
    {
        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(backgroundTrans.DOMove(moveDirection + backgroundTrans.position, duration, snapping));
        mySequence.Join(bgImage.DOFade(1f, duration));
        mySequence.Join(buttonsTrans.DOMove(moveDirection + buttonsTrans.position, duration, snapping).SetDelay(delay));
        mySequence.Join(buttonsCanvasGroup.DOFade(1f, duration).SetDelay(delay));
    }

    public void ResetUI()
    {
        backgroundTrans.DOMove(-moveDirection + backgroundTrans.position, 0, false);
        buttonsTrans.DOMove(-moveDirection + buttonsTrans.position, 0, false);

        Color c = bgImage.color;
        c.a = 0;
        bgImage.color = c;

        buttonsCanvasGroup.alpha = 0;
    }
}
