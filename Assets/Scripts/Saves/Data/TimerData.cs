using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimerData
{
    public List<Timer> timers;
    public List<Chronometre> chronometres;

    public TimerData(TimeManager timeManager)
    {
        timers = timeManager.timers;
        chronometres = timeManager.chronometres;

        foreach (Timer item in timers)
        {
            item.methode = null;
        }
    }
}
