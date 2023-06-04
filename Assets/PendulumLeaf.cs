using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumLeaf : MonoBehaviour
{
    [SerializeField]
    private float amplitude = 0.1f;
    [SerializeField]
    private float duration = 1f;

    void Start()
    {
        amplitude = Random.Range(amplitude * 0.9f, amplitude * 1.1f) * (Random.Range(0, 2) * 2 - 1);
        duration = Random.Range(duration * 0.95f, duration * 1.05f);
        Vector3 rotation = new Vector3(0f, 0f, amplitude);
        transform.DORotate(transform.rotation.eulerAngles + rotation, duration / 2).OnComplete(() =>
         transform.DORotate(transform.rotation.eulerAngles - 2 * rotation, duration).SetEase(Ease.InOutFlash).SetLoops(-1, LoopType.Yoyo)
        );

    }
}
