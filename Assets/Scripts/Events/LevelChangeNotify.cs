using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChangeNotify : Notify
{
    public int level;
    public int levelUpCost;
    public LevelChangeNotify(int level, int levelUpCost)
    {
        this.level = level;
        this.levelUpCost = levelUpCost;
    }
}
