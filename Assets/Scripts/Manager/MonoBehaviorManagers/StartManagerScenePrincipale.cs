using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using System;

public class StartManagerScenePrincipale : MonoBehaviour
{
    private const string NAME_TIMER = "GameChronometre";
    private const string CUSTOM_EVENT_NAME = "Start_MiniMining";
    private int compteur = 0;

    private void Awake()
    {
        Application.targetFrameRate = 30;
    }
    // Start is called before the first frame update
    void Start()
    {
        print(Application.persistentDataPath);
        SettingManager inst = SettingManager.instance;
        if (!inst.firstThingsDone[5])
        {
            string player_name = SystemInfo.deviceName;
            string date = DateTime.Now.ToString();
            AnalyticsResult r = Analytics.CustomEvent(CUSTOM_EVENT_NAME, new Dictionary<string, object> { { player_name, date } });

            inst.firstThingsDone[5] = true;
        }

        if (ScoreManager.instance.timeFinishGame == 0)
        {
            if (!TimeManager.instance.isChronometreThere(NAME_TIMER))
            {
                TimeManager.instance.AddChronometre(NAME_TIMER);
            }
        }

        if (ScoreManager.instance.isDeadNotSaveble)
        {
            EventsGameState.instance.dead(typeMort.Quit);
        }

        //SettingManager.instance.bestTimeFinishGame = 800;
        //SettingManager.instance.bestTimeFinishGameFree = 1000;
    }

    public static string getNameTimer()
    {
        return NAME_TIMER;
    }
}
