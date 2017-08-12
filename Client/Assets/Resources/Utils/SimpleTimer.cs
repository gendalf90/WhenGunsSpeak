using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTimer
{
    private float period;
    private float startTime;

    private SimpleTimer(float periodInSeconds)
    {
        period = periodInSeconds;
        startTime = Time.realtimeSinceStartup;
    }

    public bool ItIsTime
    {
        get
        {
            return Time.realtimeSinceStartup - startTime > period;
        }
    }

    public void Restart()
    {
        startTime = Time.realtimeSinceStartup;
    }

    public bool GetItIsTimeAndRestartIfTrue()
    {
        if(ItIsTime)
        {
            Restart();
            return true;
        }

        return false;
    }

    public static SimpleTimer StartNew(float periodInSeconds)
    {
        return new SimpleTimer(periodInSeconds);
    }
}
