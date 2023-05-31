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

    private void Awake()
    {
        magnetRadius = magnetCollider.radius;
    }

    private void OnEnable()
    {
        GameManager.OnRoundEnd += ExpandMagnetRadius;
        GameManager.OnStartNextRound += ResetMagnetRadius;
    }

    private void OnDisable()
    {
        GameManager.OnRoundEnd -= ExpandMagnetRadius;
        GameManager.OnStartNextRound -= ResetMagnetRadius;
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
            if (inventory.Add(collectibleItem.item))
            {
                AudioManager.Instance.PlaySE("Collect");
                Destroy(collectibleItem.gameObject);
            }
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
