using System;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] private Currency currency;
    [SerializeField] private CircleCollider2D magnetCollider;
    private float magnetRadius;

    private Action<Notify> OnRoundEnd, OnStartNextRound;

    private void Awake()
    {
        magnetRadius = magnetCollider.radius;

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
        magnetCollider.radius = 100;
    }

    private void ResetMagnetRadius()
    {
        magnetCollider.radius = magnetRadius;
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
