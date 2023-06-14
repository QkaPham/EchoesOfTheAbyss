using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsChangeNotify : Notify
{
    public CharacterStats stats;
    public StatsChangeNotify(CharacterStats stats)
    {
        this.stats = stats;
    }
}
