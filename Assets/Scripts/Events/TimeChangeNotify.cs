using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeChangeNotify : Notify
{
    public float time;
    public TimeChangeNotify(float time)
    {
        this.time = time;
    }
}
