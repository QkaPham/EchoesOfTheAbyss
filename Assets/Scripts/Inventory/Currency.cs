using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Currency", menuName = "Scriptable Object/Currency")]
public class Currency : ScriptableObject
{
    [SerializeField]
    private int balance;
    public int Balance
    {
        get
        {
            return balance;
        }
        private set
        {
            balance = value;
            EventManager.Instance.Raise(EventID.CurrencyChange, new CurrencyChangeNotify(balance));
        }
    }

    private bool EnoughCurrency(int amount) => Balance >= amount;

    public void Init()
    {
        Balance = 0;
    }

    public void Gain(int amount)
    {
        Balance += amount;
    }

    public bool Use(int amount)
    {
        if (!EnoughCurrency(amount)) return false;
        Balance -= amount;
        return true;
    }
}
