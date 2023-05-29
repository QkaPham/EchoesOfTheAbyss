using UnityEngine;
using UnityEngine.Pool;

public class EnemyBullet : MonoBehaviour, PoolableObject<EnemyBullet>
{
    protected Vector3 direction;
    protected float speed = 10;
    protected float damage = 5;
    protected float despawnTime = 4;
    protected float despawnTimeElapsed = 0;

    protected ObjectPool<EnemyBullet> pool { get; set; }

    void Update()
    {
        Fly();
        Despawn();
    }

    public void Init(Vector3 position, Vector3 direction, float damage, float speed = 10f, float despawnTime = 4f)
    {
        transform.position = position;
        this.direction = direction;
        this.damage = damage;
        this.speed = speed;
        this.despawnTime = despawnTime;
    }

    public void SetPool(ObjectPool<EnemyBullet> pool)
    {
        this.pool = pool;
    }

    public void Release()
    {
        pool.Release(this);
    }

    public void Fly()
    {
        transform.position += direction * Time.deltaTime * speed;
    }

    public void Despawn()
    {
        despawnTimeElapsed += Time.deltaTime;
        if (despawnTimeElapsed >= despawnTime)
        {
            despawnTimeElapsed = 0;
            Release();
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.health.TakeDamage(damage);
            Release();
        }
    }
}
