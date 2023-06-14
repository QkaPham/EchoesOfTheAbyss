using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaChangeNotify : Notify
{
    public float currentMana;
    public float maxMana;
    public ManaChangeNotify(float currentMana, float maxMana)
    {
        this.currentMana = currentMana;
        this.maxMana = maxMana;
    }
}
