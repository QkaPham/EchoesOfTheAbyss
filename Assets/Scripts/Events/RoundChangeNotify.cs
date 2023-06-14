using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundChangeNotify : Notify
{
    public int round;
    public bool isBossRound;
    public RoundChangeNotify(int round, bool isBossRound)
    {
        this.round = round;
        this.isBossRound = isBossRound;
    }
}
