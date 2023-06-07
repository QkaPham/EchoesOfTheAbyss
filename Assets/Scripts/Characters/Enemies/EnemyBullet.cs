using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyBullet : MonoBehaviour, PoolableObject<EnemyBullet>
{
    protected Rigidbody2D rb;
    protected float damage = 5;

    protected ObjectPool<EnemyBullet> pool { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector3 position, Vector3 direction, float damage, float speed = 10f, float range = 4f)
    {
        transform.position = position;
        this.damage = damage;
        rb.velocity = direction * speed;
        Realease(range / speed);
    }

    public void SetPool(ObjectPool<EnemyBullet> pool)
    {
        this.pool = pool;
    }

    public void Realease(float delay = 0f)
    {
        StartCoroutine(DelayDestroy(delay));
    }
    private IEnumerator DelayDestroy(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        if (gameObject.activeSelf)
        {
            pool.Release(this);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.health.TakeDamage(damage);
            Realease();
        }
    }
}
