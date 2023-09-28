using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Analytics;

public class PanelSettingManager : MonoBehaviour, panel
{
    [SerializeField] private TabsManager[] tabsManagers;
    [SerializeField] private Slider[] slidersManagers;

    [SerializeField] private SaveManager saveMan;

    private EasyComponentsGetter getter;
    private string timerName;

    private int compteursCheat = 0;
    private const string CUSTOM_EVENT_NAME = "ClickDanyLaS";

    public void demarer()
    {
        //Tabs buttons:
        setControleVisible(SettingManager.instance.VisibilityControle);
        setLeftHanded(SettingManager.instance.LeftHandedControle);
        setAssistedChainsContole(SettingManager.instance.AssistedChainsControle);

        //Sliders
        AudioManager.instance.changeVolume(SettingManager.instance.BackGroundVolume);
        FxSoundManager.instance.changeGeneralVolume(SettingManager.instance.SoundEffetsVolume);
    }

    // MONOBEHAVIOUR METHODE:
    void Start()
    {
        getter = GetComponent<EasyComponentsGetter>();
        timerName = StartManagerScenePrincipale.getNameTimer();

        //Tabs buttons:
        tabsManagers[0].OnTabSelectedSprite(SettingManager.instance.VisibilityControle ? 1 : 0);
        tabsManagers[1].OnTabSelectedSprite(SettingManager.instance.LeftHandedControle ? 0 : 1);
        tabsManagers[2].OnTabSelectedSprite(SettingManager.instance.AssistedChainsControle ? 1 : 0);

        //Sliders:
        SliderBackgroundSound(SettingManager.instance.BackGroundVolume);
        slidersManagers[0].value = SettingManager.instance.BackGroundVolume;

        SliderSoundEffect(SettingManager.instance.SoundEffetsVolume);
        slidersManagers[1].value = SettingManager.instance.SoundEffetsVolume;
    }

    private void Update()
    {
        int time;

        if (ScoreManager.instance.timeFinishGame == 0)
            time = (int)TimeManager.instance.getTimeChronometregByName(timerName);
        else
            time = (int)ScoreManager.instance.timeFinishGame;

        getter.getTxt(2).SetText(Transformation.transformTime(time, true));
    }

    /*
     * METHODE BUTTON PUBLIC:
     */

    //TAB BUTTONS:
    public void setControleVisible(bool isVisible)
    {
        ControleManager.instance.setControleVisible(isVisible);
        SettingManager.instance.VisibilityControle = isVisible;
    }
    public void setLeftHanded(bool leftHanded)
    {
        SettingManager.instance.LeftHandedControle = leftHanded;
        ControleManager.instance.setControleSide(leftHanded ? sideContoller.Left : sideContoller.Right);
    }
    public void setAssistedChainsContole(bool chainControl)
    {
        SettingManager.instance.AssistedChainsControle = chainControl;
    }

    //ACTION BUTTONS:
    public void ReplayTutoriel()
    {
        saveMan.saveAll();
        SceneManager.LoadScene(SceneName.SCENE_TUTORIEL);
    }
    public void ReplayBD()
    {
        saveMan.saveAll();
        SceneManager.LoadScene(SceneName.SCENE_BD_SCINEMATIC);
    }

    public void danyLaS_YTB()
    {
        string player_name = SystemInfo.deviceName;
        string date = DateTime.Now.ToString();
        AnalyticsResult r = Analytics.CustomEvent(CUSTOM_EVENT_NAME, new Dictionary<string, object> { { player_name, date } });

        saveMan.saveAll();
        Application.OpenURL("https://www.youtube.com/channel/UCZTivrqAWvFKgv_1WrUi4WA");
    }
    public void danyLaS_Spotify()
    {
        string player_name = SystemInfo.deviceName;
        string date = DateTime.Now.ToString();
        AnalyticsResult r = Analytics.CustomEvent(CUSTOM_EVENT_NAME, new Dictionary<string, object> { { player_name, date } });

        saveMan.saveAll();
        Application.OpenURL("https://open.spotify.com/artist/5S1NuxS4X7cnStF6tu4oNu");
    }

    //SLIDER:
    public void SliderBackgroundSound(float value)
    {
        string str = "" + (int)(value * 100);
        getter.getTxt(0).SetText(str);

        AudioManager.instance.changeVolume(value);
        SettingManager.instance.BackGroundVolume = value;
    }
    public void SliderSoundEffect(float value)
    {
        string str = "" + (int)(value * 100);
        getter.getTxt(1).SetText(str);

        FxSoundManager.instance.changeGeneralVolume(value);
        SettingManager.instance.SoundEffetsVolume = value;
    }


    //CHEATS!
    public void addCompteurCheat()
    {
        TimeManager.instance.AddTime("CheatTimer", 5, resetTimerCheat);
        compteursCheat++;

        if (compteursCheat >= 10)
        {
            if (getter.getTxt(0).text == "32" && getter.getTxt(1).text == "45")
            {
                //THROW POP!
                print("CHEAT");
                ScoreManager.instance.addGold(1000000000);
            }

            compteursCheat = 0;
            TimeManager.instance.stopTime("CheatTimer");
        }
    }
    private void resetTimerCheat()
    {
        compteursCheat = 0;
    }
}
