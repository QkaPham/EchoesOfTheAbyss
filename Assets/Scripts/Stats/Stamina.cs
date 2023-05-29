using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Stamina", menuName = "Scriptable Object/Stamina")]
public class Stamina : ScriptableObject
{
    [SerializeField]
    private float currentStamina;
    public float CurrentStamina
    {
        get
        {
            return currentStamina;
        }
        private set
        {
            currentStamina = Mathf.Clamp(value, 0, maxStamina);
        }
    }

    [SerializeField]
    private float maxStamina;
    public float MaxStanima { get => maxStamina; }

    [SerializeField]
    private float delayStaminaRecoverTime;
    public float LastTimeConsumeStamina { get; set; }
    private bool recoverStamina => Time.time >= delayStaminaRecoverTime + LastTimeConsumeStamina;
    private bool fullyRecovered => CurrentStamina == maxStamina;

    public float staminaRecoverPerSecond;
    public void Init(float maxStamina)
    {
        this.maxStamina = maxStamina;
        CurrentStamina = maxStamina;
    }

    public void Regenerate()
    {
        if (!fullyRecovered)
        {
            if (recoverStamina)
            {
                CurrentStamina += staminaRecoverPerSecond * Time.deltaTime;
            }
            if (UIManager.Instance.GamePanel != null)
            {
                UIManager.Instance.GamePanel.UpdateStaminaBar(this);
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
        LastTimeConsumeStamina = Time.time;
        return true;
    }

    public bool ConsumePerSecond(float stamina)
    {
        return Consume(stamina * Time.deltaTime);
    }
}
