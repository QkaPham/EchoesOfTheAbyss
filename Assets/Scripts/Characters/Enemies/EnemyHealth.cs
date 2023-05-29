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
    public void Init(BaseEnemy enemy)
    {
        this.enemy = enemy;
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float damageAmount, bool isCritHit)
    {
        currentHealth -= damageAmount;
        //DamagePopup.Show(damagePopupPrefab, damageAmount, enemy.transform.position + Vector3.up);
        DamagePopup damagePopup = Instantiate(damagePopupPrefab, transform).GetComponent<DamagePopup>();
        damagePopup.SetUp(damageAmount, enemy.transform.position + Vector3.up, isCritHit);

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
