using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Props : EnemyHealth
{
    public GameObject CollectibleItemPrefab;

    public Vector3 Position => transform.position;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public override void TakeDamage(float damageAmount, bool isCrit)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            DropItem();
        }
    }

    void DropItem()
    {
        if (CollectibleItemPrefab != null)
        {
            Instantiate(CollectibleItemPrefab, transform.position, Quaternion.identity);
        }
    }
}
