using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mana", menuName = "Scriptable Object/Mana")]
public class Mana : ScriptableObject
{
    public float currentMana;
    public float CurrentMana
    {
        get
        {
            return currentMana;
        }
        private set
        {
            currentMana = Mathf.Clamp(value, 0, maxMana);
        }
    }

    [SerializeField]
    private float maxMana;
    public float MaxMana { get => maxMana; }

    [SerializeField]
    private float delayManaRecoverTime;
    public float LastTimeConsumeMana { get; set; }
    private bool recoverMana => Time.time >= delayManaRecoverTime + LastTimeConsumeMana;
    private bool fullyRecovered => currentMana == maxMana;

    public float manaRecoverPerSecond;
    public void Init(float maxMana)
    {
        this.maxMana = maxMana;
        currentMana = maxMana;
    }

    public void Regenerate()
    {
        if (!fullyRecovered)
        {
            if (recoverMana)
            {
                CurrentMana += manaRecoverPerSecond * Time.deltaTime;
            }
            if (UIManager.Instance.GamePanel != null)
            {
                UIManager.Instance.GamePanel.UpdateManaBar(this);
            }
        }
    }

    public bool Consume(float mana)
    {
        if (CurrentMana < mana)
        {
            return false;
        }
        CurrentMana -= mana;
        LastTimeConsumeMana = Time.time;
        return true;
    }

    public bool ConsumePerSecond(float mana)
    {
        return Consume(mana * Time.deltaTime);
    }
}
