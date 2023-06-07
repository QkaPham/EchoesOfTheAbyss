using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private Collider2D col;

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

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        col.enabled = true;
    }

    private void OnDisable()
    {
        col.enabled = false;
    }

    public virtual void Init(BaseEnemy enemy)
    {
        this.enemy = enemy;
        maxHealth = enemy.stats.totalMaxHealth;
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
            col.enabled = false;
        }
        else
        {
            enemy.Hurt();
        }
    }
}
