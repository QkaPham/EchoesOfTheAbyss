using System;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] private Currency currency;
    [SerializeField] private CircleCollider2D magnetCollider;

    private Action<Notify> OnRoundEnd, OnStartNextRound;

    private void Awake()
    {
        OnRoundEnd = thisNotify => ExpandMagnetRadius();
        OnStartNextRound = thisNotify => ResetMagnetRadius();
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventID.RoundEnd, OnRoundEnd);
        EventManager.Instance.AddListener(EventID.StartNextRound, OnStartNextRound);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventID.RoundEnd, OnRoundEnd);
        EventManager.Instance.RemoveListener(EventID.StartNextRound, OnStartNextRound);
    }

    private void ExpandMagnetRadius()
    {
        magnetCollider.radius += 1000;
    }

    private void ResetMagnetRadius()
    {
        magnetCollider.radius -= 1000;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TakeFragment(collision);
    }

    private void TakeFragment(Collider2D collision)
    {
        Fragment fragment = collision.gameObject.GetComponent<Fragment>();
        if (fragment != null)
        {
            AudioManager.Instance.PlaySFX("Collect");
            currency.Gain(fragment.Amount);
            fragment.Release();
        }
    }
}
