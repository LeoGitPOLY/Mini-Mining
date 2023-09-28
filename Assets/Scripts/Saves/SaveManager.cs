using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private bool isLoad;
    [SerializeField] private bool develloperMode;
    [SerializeField] private bool godMode;

    //Save by time
    [SerializeField] private float interpolationPeriod;
    private float time = 0.0f;

    private string nameScene;
    private bool localSaving;

    private void Start()
    {
        nameScene = SceneManager.GetActiveScene().name;

        if (nameScene == SceneName.SCENE_PRINCIPALE)
            loadScenePrincipal();
        if (nameScene == SceneName.SCENE_TUTORIEL)
            loadTutoriel();

        localSaving = true;
    }
    private void loadScenePrincipal()
    {
        EventsGameState.instance.onRestart += resetSave;
        EventsGameState.instance.onRevive += reviveManager;
        EventsGameState.instance.onFinish += finishGame;
        EventsGameState.instance.onDead += saveAll;
        EventsGameState.instance.onTeleport += teleport;

        if (isLoad)
        {
            //LOAD VERSION:
            VersionManager.instance.loadVersion();

            ScoreManager.instance.LoadScore();
            ImprouvementManager.instance.LoadImprouvements();

            TimeManager.instance.loadTime();
            PlayerManager.instance.LoadPlayer();
            AudioManager.instance.loadSoundProgression();

            gridManager.instance.LoadMonde();
            SettingManager.instance.loadSettings();

            //LOAD PARSER:
            loadParser();
        }
        else
        {
            ScoreManager.instance.loadNew();
            ImprouvementManager.instance.loadNew();

            TimeManager.instance.loadNew();
            PlayerManager.instance.loadNew();
            AudioManager.instance.loadNew();

            gridManager.instance.LoadNew();
            SettingManager.instance.loadNew();
        }

        //Ajout: si develloper alors CHANGE les stats
        if (develloperMode)
        {
            ScoreManager.instance.loadDevelloperMode();
            ImprouvementManager.instance.loadDevelopperMode();
            SettingManager.instance.loadDevelloperMode();
        }

        //Ajout: si godMODE alors CHANGE les stats
        if (godMode)
        {
            gridManager.instance.loadGodMode();
        }

        //Send time request if necessary:
        sendTimeRequest();
    }
    private void loadParser()
    {
        GameObject obj = GameObject.Find("TutorielParser");

        if (obj != null)
        {
            TutorielParser parser = obj.GetComponent<TutorielParser>();

            bool leftHanded = parser.loadInfoParser();

            ControleManager.instance.setControleSide(leftHanded ? sideContoller.Left : sideContoller.Right);
            SettingManager.instance.LeftHandedControle = leftHanded;

            print(leftHanded);
            parser.deleteParser();

        }
    }

    private void loadTutoriel()
    {
        localSaving = false;
        gridManager.instance.LoadNew();
        SettingManager.instance.loadTutoriel();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        time += Time.deltaTime;

        if (time >= interpolationPeriod)
        {
            time = 0.0f;

            saveAll();
            sendTimeRequest();
        }
    }

    private void OnApplicationQuit()
    {
        saveAll();
        sendTimeRequest();
        print("saveQuit");
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            saveAll();
            sendTimeRequest();
        }
    }

    private void loadEmpty()
    {
        ImprouvementManager.instance.loadNew();
        ScoreManager.instance.loadNew();
        TimeManager.instance.loadNew();

        gridManager.instance.loadEmptyResize();
        PlayerManager.instance.loadNew();
    }

    public void saveAll()
    {
        if (nameScene == SceneName.SCENE_PRINCIPALE && localSaving)
        {
            ScoreManager.instance.saveScore();
            ImprouvementManager.instance.saveScore();
            TimeManager.instance.saveTime();

            PlayerManager.instance.savePlayer();
            SaveSystem.saveMonde(Utils.gridMonde, Utils.gridExploration);

            AudioManager.instance.saveSoundProgression();

            SettingManager.instance.saveSettings();
            VersionManager.instance.saveVersion();

            print("saveALLL");
        }
    }
    private void reviveManager()
    {
        PlayerManager.instance.loadRevive();
        ScoreManager.instance.loadRevive();
        gridManager.instance.RecenterGrid();

        UIManager.instance.setVisibleRevive();

        EventsOnChains.instance.TreuilActif = new Vector2Int(-1000, -1000);
    }
    private void resetSave()
    {
        loadEmpty();
        saveAll();
    }
    private void finishGame()
    {
        PlayerManager.instance.loadFinish();
        ScoreManager.instance.loadRevive();

        bool lastState = localSaving;

        localSaving = true;
        saveAll();
        localSaving = lastState;
    }

    private void teleport(int indexTeleport)
    {
        PlayerManager.instance.loadTeleport(indexTeleport);
        gridManager.instance.RecenterGrid();
    }

    private void sendTimeRequest()
    {
        List<RequestUpdate> requestUpdates = SettingManager.instance.updateTime;

        if (requestUpdates.Count != 0)
        {
            PlayfabManager.instance.sendUpdateScore();
        }
    }

    public void setLocalIsSave(bool isSaving)
    {
        if (!isSaving)
            saveAll();

        localSaving = isSaving;
    }
}
