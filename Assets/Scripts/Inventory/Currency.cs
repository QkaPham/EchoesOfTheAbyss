using System;
using UnityEngine;

[CreateAssetMenu(fileName ="Currency", menuName ="Scriptable Object/Currency")]
public class Currency : ScriptableObject
{
    [SerializeField]
    private int currencyAmount;
    private bool EnoughCurrency(int amount) => currencyAmount >= amount;
    public static event Action<int> OnCurrencyChange;

    public void Init()
    {
        currencyAmount= 0;
        OnCurrencyChange?.Invoke(currencyAmount);
    }

    public void Gain(int amount)
    {
        currencyAmount += amount;
        OnCurrencyChange?.Invoke(currencyAmount);
    }

    public bool Use(int amount)
    {
        if (!EnoughCurrency(amount)) return false;
        currencyAmount -= amount;
        OnCurrencyChange?.Invoke(currencyAmount);

        return true;
    }
}
