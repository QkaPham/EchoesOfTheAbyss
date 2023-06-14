using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthChangeNotify : Notify
{
    public float currentHealth;
    public float maxHealth;
    public HealthChangeNotify(float currentHealth, float maxHealth)
    {
        this.currentHealth = currentHealth;
        this.maxHealth = maxHealth;
    }
}
