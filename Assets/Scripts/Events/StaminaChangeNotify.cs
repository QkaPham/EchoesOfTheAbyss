using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaChangeNotify : Notify
{
    public float currentStanima;
    public float maxStamina;
    public StaminaChangeNotify(float currentStanima, float maxStamina)
    {
        this.currentStanima = currentStanima;
        this.maxStamina = maxStamina;
    }
}
