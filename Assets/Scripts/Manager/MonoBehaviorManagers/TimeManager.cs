using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

[System.Serializable]
public class Timer
{
    public string name;
    public float duration;
    public float currentTime;

    public Action methode;

    public Timer(string nameNew, float durationNew, Action methodeNew)
    {
        name = nameNew;
        duration = durationNew;
        methode = methodeNew;
        currentTime = 0;

    }
}
[System.Serializable]
public class Chronometre
{
    public string name;
    public float currentTime;

    public Chronometre(string nameNew)
    {
        name = nameNew;
        currentTime = 0;
    }
}

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    public List<Timer> timers;
    public List<Chronometre> chronometres;

    private const string CUSTOM_EVENT_NAME = "TimeFinal_Miniming";

    //MonoBehaviour methode:
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < timers.Count; i++)
        {
            Timer currents = timers[i];
            if (currents.currentTime > currents.duration)
            {
                timers.Remove(currents);
                currents.methode?.Invoke();
            }
            currents.currentTime += Time.fixedDeltaTime;
        }

        for (int i = 0; i < chronometres.Count; i++)
        {
            Chronometre currents = chronometres[i];
            currents.currentTime += Time.fixedDeltaTime;
        }
    }

    //Public timer Methode:
    public void AddTime(string nameNew, float durationNew, Action methodeNew)
    {
        bool isThere = false;
        foreach (Timer item in timers)
        {
            if (item.name == nameNew)
                isThere = true;
        }

        if (!isThere)
            timers.Add(new Timer(nameNew, durationNew, methodeNew));

    }
    public void AddTimeRemoveIfThere(string nameNew, float durationNew, Action methodeNew)
    {
        for (int i = 0; i < timers.Count; i++)
        {
            Timer currents = timers[i];

            if (currents.name == nameNew)
            {
                timers.Remove(currents);
            }
        }

        timers.Add(new Timer(nameNew, durationNew, methodeNew));
    }
    public float getTimeRemaningByName(string name)
    {
        float timeReturn = 0.0f;

        foreach (Timer item in timers)
        {
            if (item.name == name)
                timeReturn = item.duration - item.currentTime;
        }

        return timeReturn;
    }
    public bool commanderMethode(string nameNew, Action methodeNew)
    {
        bool isCommander = false;
        foreach (Timer item in timers)
        {
            if (item.name == nameNew)
            {
                item.methode = methodeNew;
                isCommander = true;
            }
        }

        return isCommander;
    }
    public void stopTime(string nameNew)
    {
        for (int i = 0; i < timers.Count; i++)
        {
            Timer currents = timers[i];
            if (currents.name == nameNew)
            {
                timers.Remove(currents);
            }
        }
    }

    //Public chronometre Methode:
    public void AddChronometre(string nameNew)
    {
        bool isThere = false;
        foreach (Chronometre item in chronometres)
        {
            if (item.name == nameNew)
                isThere = true;
        }

        if (!isThere)
            chronometres.Add(new Chronometre(nameNew));

    }
    public void stopChronometre(string nameNew)
    {
        for (int i = 0; i < chronometres.Count; i++)
        {
            Chronometre currents = chronometres[i];
            if (currents.name == nameNew)
            {
                chronometres.Remove(currents);
            }
        }
    }
    public float getTimeChronometregByName(string name)
    {
        float timeReturn = 0.0f;

        foreach (Chronometre item in chronometres)
        {
            if (item.name == name)
                timeReturn = item.currentTime;
        }

        return timeReturn;
    }
    public bool isChronometreThere(string name)
    {
        bool isThere = false;
        foreach (Chronometre item in chronometres)
        {
            if (item.name == name)
                isThere = true;
        }

        return isThere;
    }

    //Save Methode:
    public void loadTime()
    {
        TimerData data = SaveSystem.loadTime();

        if (data != null)
        {
            timers = data.timers;
            chronometres = data.chronometres;
        }
        else
            loadNew();
    }
    public void loadNew()
    {
        timers = new List<Timer>();
        chronometres = new List<Chronometre>();
    }
    public void saveTime()
    {
        SaveSystem.saveTime(instance);
    }

    //Logic methode:
    public void gameIsDone()
    {
        if (ScoreManager.instance.timeFinishGame == 0)
        {
            string nameChrono = StartManagerScenePrincipale.getNameTimer();
            float time = getTimeChronometregByName(nameChrono);

            string phone_name = SystemInfo.deviceName;
            string Player_name = SettingManager.instance.playerName;
            string time_str = Transformation.transformTime((int)time, true);

            //Manage time, current game:
            ScoreManager.instance.timeFinishGame = time;
            stopChronometre(nameChrono);

            //Manage best time and send to leaderboard:
            bool havepaid = ScoreManager.instance.isPlayerPaid;
            PlayfabManager.instance.createdNewUpdateRequest((int)time, havepaid);
            PlayfabManager.instance.sendUpdateScore();
            
            //Analytic
            AnalyticsResult r = Analytics.CustomEvent(CUSTOM_EVENT_NAME, new Dictionary<string, object> { { phone_name + "/ " + Player_name, time_str } });
        }
    }
}
