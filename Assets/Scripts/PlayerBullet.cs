using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField]
    protected Rigidbody2D rb;

    public CharacterStats stats;
    public float damageMultifier;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector3 position, Vector3 direction, float range, float speed)
    {
        transform.position = position;
        rb.velocity = direction * speed;
        Destroy(gameObject, range / speed);
        float angle = Vector2.SignedAngle(Vector2.up, direction);
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        if (angle < 0) Flip();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            float damage = DamageCalculate(out bool isCrit, stats, damageMultifier);
            enemyHealth.TakeDamage(damage, isCrit);
            Destroy(gameObject);
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector3(-1f, 1, 1);
    }

    private float DamageCalculate(out bool isCrit, CharacterStats stats, float damageMultiplier)
    {
        float rand = UnityEngine.Random.value;
        if (rand < stats.CriticalHitChance.Total)
        {
            isCrit = true;
            return stats.Attack.Total * (1 + stats.CriticalHitDamage.Total) * damageMultiplier;
        }
        else
        {
            isCrit = false;
            return stats.Attack.Total * damageMultiplier;
        }
    }
}
