using System;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private Currency currency;

    [SerializeField]
    private CircleCollider2D magnetCollider;
    private float magnetRadius;

    private Action<Notify> OnRoundEnd, OnStartNextRound;

    private void Awake()
    {
        magnetRadius = magnetCollider.radius;

        OnRoundEnd = thisNotify => ExpandMagnetRadius();
        OnStartNextRound = thisNotify => ResetMagnetRadius();
    }

    private void Start()
    {
        EventManager.AddListiener(EventID.RoundEnd, OnRoundEnd);
        EventManager.AddListiener(EventID.StartNextRound, OnStartNextRound);
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
        TakeItem(collision);
        TakeFragment(collision);
    }

    private void TakeItem(Collider2D collision)
    {
        CollectibleItem collectibleItem = collision.gameObject.GetComponent<CollectibleItem>();
        if (collectibleItem != null)
        {
            inventory.MergeAdd(collectibleItem.item);
            AudioManager.Instance.PlaySE("Collect");
            Destroy(collectibleItem.gameObject);
        }
    }

    private void TakeFragment(Collider2D collision)
    {
        Fragment fragment = collision.gameObject.GetComponent<Fragment>();
        if (fragment != null)
        {
            AudioManager.Instance.PlaySE("Collect");
            currency.Gain(fragment.Amount);
            fragment.Release();
        }
    }
}
