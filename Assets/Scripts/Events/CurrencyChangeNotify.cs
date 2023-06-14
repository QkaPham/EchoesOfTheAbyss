using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyChangeNotify : Notify
{
    public int balance;
    public CurrencyChangeNotify(int balance)
    {
        this.balance = balance;
    }
}
