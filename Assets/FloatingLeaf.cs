using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingLeaf : MonoBehaviour
{
    [SerializeField]
    private float amplitude = 0.1f;
    [SerializeField]
    private float duration = 1f;

    void Start()
    {
        amplitude = Random.Range(amplitude * 0.9f, amplitude * 1.1f) * (Random.Range(0, 2) * 2 - 1);
        duration = Random.Range(duration * 0.95f, duration * 1.05f);
        Vector3 moveDir = new Vector3(0f, amplitude, 0f);
        transform.DOMove(transform.position + moveDir, duration / 2).OnComplete(() =>
            transform.DOMove(transform.position - 2 * moveDir, duration).SetEase(Ease.InOutFlash).SetLoops(-1, LoopType.Yoyo)
        );
    }
}
