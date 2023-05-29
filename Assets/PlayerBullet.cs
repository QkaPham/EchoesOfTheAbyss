using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    protected float damage = 5;
    protected float despawnTime = 4;
    protected float despawnTimeElapsed = 0;

    [SerializeField]
    protected Rigidbody2D rb;

    [SerializeField]
    protected Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Despawn();
    }

    public void Init(Vector3 position, Vector3 direction, float damage, float speed = 10f, float despawnTime = 4f)
    {
        transform.position = position;
        this.damage = damage;
        this.despawnTime = despawnTime;
        rb.velocity = direction * speed;
    }

    public void Despawn()
    {
        despawnTimeElapsed += Time.deltaTime;
        if (despawnTimeElapsed >= despawnTime)
        {
            despawnTimeElapsed = 0;
            Destroy(gameObject);
            //Release();
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage, false);
            Destroy(gameObject);
            //Release();
        }
    }
}
