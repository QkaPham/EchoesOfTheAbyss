using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UIElements;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    protected float maxHealth = 20;

    [SerializeField]
    protected float currentHealth;

    protected BaseEnemy enemy;
    [SerializeField]
    protected GameObject damagePopupPrefab;

    [SerializeField]
    protected Transform damagePopupPoint;

    public bool isDeath => currentHealth <= 0;
    public void Init(BaseEnemy enemy)
    {
        this.enemy = enemy;
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float damageAmount, bool isCritHit)
    {
        currentHealth -= damageAmount;
        DamagePopup damagePopup = Instantiate(damagePopupPrefab, transform).GetComponent<DamagePopup>();
        damagePopup.SetUp(damageAmount, damagePopupPoint.position - transform.position, isCritHit);

        if (currentHealth <= 0)
        {
            enemy.Death();

        }
        else
        {
            enemy.Hurt();
        }
    }
}
