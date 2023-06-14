using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class RangeWeapon : MonoBehaviour
{
    [SerializeField]
    protected GameObject rangeWeaponPrefab;
    [SerializeField]
    public Transform firePoint;

    protected Player player;
    protected CharacterStats stats => player.stats;
    protected Mana mana => player.mana;

    protected Vector3 fireDirection => (InputManager.Instance.MouseOnWorld - (Vector2)firePoint.position).normalized;

    [SerializeField]
    protected float baseAttackCooldown;
    [SerializeField]
    protected float manaConsume;
    [SerializeField]
    protected float range;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float damageMultifier;
    [SerializeField]
    protected float angle;
    [SerializeField]
    protected int quantity;
    [SerializeField]
    protected float delayPerFire;

    protected bool canAttack => Time.time >= lastAttackTime + cooldownTime;
    protected float cooldownTime => baseAttackCooldown / stats.Haste.Total;
    protected float lastAttackTime;

    protected void Awake()
    {
        player = GetComponentInParent<Player>();

        PlayerBullet bullet = rangeWeaponPrefab.GetComponent<PlayerBullet>();
        bullet.stats = stats;
        bullet.damageMultifier = damageMultifier;

        //Cooldown 10% when start game
        lastAttackTime = -cooldownTime * 1.1f;
    }

    protected void Update()
    {
        if (InputManager.Instance.RangeAttack && canAttack && mana.CurrentMana >= manaConsume)
        {
            mana.Consume(manaConsume);
            StartCoroutine(Attack());
        }
    }

    public IEnumerator Attack()
    {
        lastAttackTime = Time.time;
        for (int i = 0; i < quantity; i++)
        {
            PlayerBullet playerBullet = Instantiate(rangeWeaponPrefab, this.transform).GetComponent<PlayerBullet>();
            float randAngle = Random.Range(-angle/2,angle/2);
            Vector2 direction = Quaternion.Euler(0f, 0f, randAngle) * fireDirection;
            playerBullet.Init(firePoint.position, direction, range, speed);
            yield return new WaitForSeconds(delayPerFire);
        }
    }
}
