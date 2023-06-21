using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mana", menuName = "Scriptable Object/Mana")]
public class Mana : ScriptableObject
{
    [SerializeField]
    private float currentMana;
    public float CurrentMana
    {
        get
        {
            return currentMana;
        }
        private set
        {
            currentMana = Mathf.Clamp(value, 0, maxMana);
            EventManager.Instance.Raise(EventID.ManaChange, new ManaChangeNotify(currentMana, maxMana));
        }
    }

    [SerializeField]
    private float maxMana;
    public float MaxMana { get => maxMana; }
    private float delayManaRecoverTime;
    private float LastTimeConsumeMana { get; set; }
    private bool recoverMana => Time.time >= delayManaRecoverTime + LastTimeConsumeMana;
    private bool fullyRecovered => currentMana == maxMana;
    private float manaRecovery;
    public void Init(CharacterStats stats)
    {
        maxMana = stats.MaxMana;
        currentMana = maxMana;
        delayManaRecoverTime = stats.DelayManaRecoverTime;
        manaRecovery = stats.ManaRecovery;
    }

    public void Regenerate()
    {
        if (!fullyRecovered)
        {
            if (recoverMana)
            {
                CurrentMana += manaRecovery * Time.deltaTime;
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
