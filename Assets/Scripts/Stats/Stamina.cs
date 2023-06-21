using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Stamina", menuName = "Scriptable Object/Stamina")]
public class Stamina : ScriptableObject
{
    [SerializeField] private float currentStamina;
    [SerializeField] private float maxStamina;
    private float delayStaminaRecoverTime;
    private float lastTimeConsumeStamina;
    private float staminaRecovery;

    public float CurrentStamina
    {
        get
        {
            return currentStamina;
        }
        private set
        {
            currentStamina = Mathf.Clamp(value, 0, maxStamina);
            EventManager.Instance.Raise(EventID.StaminaChange, new StaminaChangeNotify(currentStamina, maxStamina));
        }
    }
    public float MaxStanima { get => maxStamina; private set => maxStamina = value; }
    private bool recoverStamina => Time.time >= delayStaminaRecoverTime + lastTimeConsumeStamina;
    private bool fullyRecovered => CurrentStamina == maxStamina;

    public void Init(CharacterStats stats)
    {
        maxStamina = stats.MaxStamina;
        CurrentStamina = maxStamina;
        delayStaminaRecoverTime = stats.DelayManaRecoverTime;
        staminaRecovery = stats.StaminaRecovery;
    }

    public void Regenerate()
    {
        if (!fullyRecovered)
        {
            if (recoverStamina)
            {
                CurrentStamina += staminaRecovery * Time.deltaTime;
            }
        }
    }

    public bool Consume(float stamina)
    {
        if (CurrentStamina < stamina)
        {
            return false;
        }
        CurrentStamina -= stamina;
        lastTimeConsumeStamina = Time.time;
        return true;
    }

    public bool ConsumePerSecond(float stamina)
    {
        return Consume(stamina * Time.deltaTime);
    }
}
