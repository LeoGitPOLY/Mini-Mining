using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public static SettingManager instance = null;

    //GAMESTATE:
    //0:FuelWarning, 1:CargoWarning, 2:TreuilWarning, 3:Selling truck, 4:Improuvements shop, 5:First time game
    public List<bool> firstThingsDone { get; set; }

    //SETTINGS:
    public float BackGroundVolume { get; set; }
    public float SoundEffetsVolume { get; set; }
    public bool VisibilityControle { get; set; }
    public bool LeftHandedControle { get; set; }
    public bool AssistedChainsControle { get; set; }
    
    //LeaderBoardinfo
    public string playerName { get; set; }
    public int indexIconSelected { get; set; }

    //SKINS:
    public List<int> skinsUnlock { get; set; }
    public int indexSkinSelected { get; set; }

    //TIME:
    /*
     * bestTimeFinishGame (CONSERVÉ SUR PLAYFAB) est le meilleur temps du joueur pour l'entiereté de l'installation du jeu
     * - bestTimeFinishGame == 0: Alors le joueur n'a jamais terminié la partie
     * - bestTimeFinishGame != 0: Alors le joueur a un meilleur temps, il peut le battre en recommencant la partie
     */
    public List<RequestUpdate> updateTime { get; set; }

    /*
     * Methode MONOBEAVIOUR 
     */
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    /*
     * Methode SAVE 
     */
    public void saveSettings()
    {
        SaveSystem.saveSettings(instance);
    }
  
    public void loadSettings()
    {
        string version_code = VersionManager.VERSION_CODE_MINI_MINING;
        string version_current = VersionManager.instance.currentVersion;

        //Load version (V1) on code version (V2):
        if (version_code == EnumVersion.V2 && version_current == EnumVersion.V1)
        {
            SettingsData dataV1 = SaveSystem.loadSettingsV1();

            if (dataV1 != null)
                load_V1_On_V2_(dataV1);
            else
                loadNew();
        }
        else if (version_code == version_current)
        {
            SettingsDataV2 data = SaveSystem.loadSettings();

            if (data != null)
                loadCurrentVersion(data);
            else
                loadNew();
        }
    }
    /*
     * LOAD PUBLIC (doesn't care about mini mining version) :
     */
    public void loadNew()
    {
        firstThingsDone = new List<bool> { false, false, false, false, false, false };

        BackGroundVolume = 0.3f;
        SoundEffetsVolume = 0.2f;
        VisibilityControle = false;
        LeftHandedControle = false;
        AssistedChainsControle = true;

        skinsUnlock = new List<int>() { 1, 0, 0, 0, 0, 0, 0, 0, 0 };
        indexSkinSelected = 0;
        indexIconSelected = 0;

        playerName = NameController.getRandomName();
        updateTime = new List<RequestUpdate>();
    }
    public void loadDevelloperMode()
    {
        firstThingsDone = new List<bool> { true, true, true, true, true, true };

        VisibilityControle = false;
        LeftHandedControle = false;
        AssistedChainsControle = true;

        skinsUnlock = new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1 };

        playerName = "Akakamak Studio";
    }
    public void loadTutoriel()
    {
        firstThingsDone = new List<bool> { true, true, true, true, true, true };

        BackGroundVolume = 0.3f;
        SoundEffetsVolume = 0.2f;
        VisibilityControle = true;
        LeftHandedControle = false;
        AssistedChainsControle = true;

        skinsUnlock = new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
        indexSkinSelected = 0;
        indexIconSelected = 0;

        playerName = "Tutoriel";
        updateTime = new List<RequestUpdate>();
    }

    /*
     * LOAD PRIVATE (care about mini mining version) :
     */
    private void loadCurrentVersion(SettingsDataV2 data)
    {
        firstThingsDone = data.firstThingsDone;

        BackGroundVolume = data.BackGroundVolume;
        SoundEffetsVolume = data.SoundEffetsVolume;

        VisibilityControle = data.VisibilityControle;
        LeftHandedControle = data.LeftHandedControle;
        AssistedChainsControle = data.AssistedChainsControle;

        skinsUnlock = data.skinsUnlock;
        indexSkinSelected = data.indexSkinSelected;
        indexIconSelected = data.indexIconSelected;

        playerName = data.playerName;
        updateTime = data.updateTime;
    }
    /// <summary>
    /// Changement version (V1) on code version (V2):
    /// - nombre de skin unlock (de 7 à 9 skins) -> (XXXXXXNXN)
    /// - Ajout nom du player
    /// - Ajout de l'index de l'icon selected
    /// - Ajout d'une request qui peut etre en cours d'envoie
    /// </summary>
    private void load_V1_On_V2_(SettingsData data)
    {
        print("v1 on v2 pour improuvement");
        //Load comme avant:
        //<
        firstThingsDone = data.firstThingsDone;

        BackGroundVolume = data.BackGroundVolume;
        SoundEffetsVolume = data.SoundEffetsVolume;

        VisibilityControle = data.VisibilityControle;
        LeftHandedControle = data.LeftHandedControle;
        AssistedChainsControle = data.AssistedChainsControle;
        //>

        //Ajustement skins:
        skinsUnlock = data.skinsUnlock;
        indexSkinSelected = data.indexSkinSelected;

        int[] new_Emplacement = { 0, 1, 2, 3, 4, 5, 7};
        List<int> temp_skinUnlock = new List<int>() { 1, 0, 0, 0, 0, 0, 0, 0, 0 };

        for (int i = 0; i < new_Emplacement.Length; i++)
        {
            if(skinsUnlock[i] == 1)
            {
                temp_skinUnlock[new_Emplacement[i]] = 1;
            }
        }

        skinsUnlock = temp_skinUnlock;
        indexSkinSelected = new_Emplacement[indexSkinSelected];

        //Ajout du nom du player:
        playerName = NameController.getRandomName();

        //Index icon selected:
        indexIconSelected = 0;

        //Ajout de la request (Prendre pour aquis personne a paye):
        //Dans V1: currentTime == 0 si game pas terminé/en cours
        float lastTimeOldVersion = ScoreManager.instance.timeFinishGame;
        updateTime = new List<RequestUpdate>();

        if (lastTimeOldVersion != 0)
        {
            RequestUpdate newUpdateTime = new RequestUpdate(lastTimeOldVersion, false);    
            updateTime.Add(newUpdateTime);
        }
    }
}
