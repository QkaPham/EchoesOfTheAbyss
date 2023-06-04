using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField]
    private List<EnemyHealth> DamageableObjects = new List<EnemyHealth>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth DamageableObject = collision.gameObject.GetComponent<EnemyHealth>();
        if (DamageableObject != null)
        {
            DamageableObjects.Add(DamageableObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyHealth DamageableObject = collision.gameObject.GetComponent<EnemyHealth>();
        if (DamageableObject != null)
        {
            DamageableObjects.Remove(DamageableObject);
        }
    }

    public void DealDamage(CharacterStats stats, float multifier = 1f)
    {
        int count = DamageableObjects.Count;
        if (count == 0) return;
        AudioManager.Instance.PlaySE("Hit");
        for (int i = count - 1; i >= 0; i--)
        {
            float damage = DamageCalculate(out bool isCrit, stats.Attack.Total, stats.CriticalHitChance.Total, stats.CriticalHitDamage.Total, multifier);
            DamageableObjects[i].TakeDamage(damage, isCrit);
        }
    }

    private float DamageCalculate(out bool isCrit, float attack, float critRate, float critDamage, float damageMultiplier)
    {
        float rand = UnityEngine.Random.value;
        if (rand < critRate)
        {
            isCrit = true;
            return attack * (1 + critDamage) * damageMultiplier;
        }
        else
        {
            isCrit = false;
            return attack * damageMultiplier;
        }
    }

    private void Update()
    {
        Flip();
        RotateToMouse();
    }

    protected void RotateToMouse()
    {
        Vector2 direction = InputManager.Instance.MouseOnWorld - (Vector2)transform.parent.position;
        float angle = Vector2.SignedAngle(Vector2.up, direction);
        angle += (InputManager.Instance.MouseOnScreen.x < 0.5f) ? -90f : 90f;
        gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }
    protected void Flip()
    {
        if (InputManager.Instance.MouseOnScreen.x < 0.5f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        if (InputManager.Instance.MouseOnScreen.x > 0.5f)
        {
            transform.localScale = Vector3.one;
        }
    }
}
