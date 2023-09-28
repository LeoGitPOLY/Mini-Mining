using System.Collections.Generic;

[System.Serializable]
public class SettingsDataV2
{
    public List <bool> firstThingsDone;

    //SETTINGS:
    public float BackGroundVolume;
    public float SoundEffetsVolume;
    public bool VisibilityControle;
    public bool LeftHandedControle;
    public bool AssistedChainsControle;

    //LeaderBoardinfo
    public string playerName;
    public int indexIconSelected;

    //SKINS:
    public List<int> skinsUnlock;
    public int indexSkinSelected;

    //TIME:
    public List<RequestUpdate> updateTime;

    public SettingsDataV2(SettingManager settingManager)
    {
        firstThingsDone = settingManager.firstThingsDone;

        BackGroundVolume = settingManager.BackGroundVolume;
        SoundEffetsVolume = settingManager.SoundEffetsVolume;

        VisibilityControle = settingManager.VisibilityControle;
        LeftHandedControle = settingManager.LeftHandedControle;
        AssistedChainsControle = settingManager.AssistedChainsControle;

        skinsUnlock = settingManager.skinsUnlock;
        indexSkinSelected = settingManager.indexSkinSelected;
        indexIconSelected = settingManager.indexIconSelected;

        playerName = settingManager.playerName;
        updateTime = settingManager.updateTime;
    }
}

[System.Serializable]
public class SettingsData
{
    public List<bool> firstThingsDone;

    //SETTINGS:
    public float BackGroundVolume;
    public float SoundEffetsVolume;
    public bool VisibilityControle;
    public bool LeftHandedControle;
    public bool AssistedChainsControle;

    //SKINS:
    public List<int> skinsUnlock;
    public int indexSkinSelected;

    public SettingsData(SettingManager settingManager)
    {
        firstThingsDone = settingManager.firstThingsDone;

        BackGroundVolume = settingManager.BackGroundVolume;
        SoundEffetsVolume = settingManager.SoundEffetsVolume;

        VisibilityControle = settingManager.VisibilityControle;
        LeftHandedControle = settingManager.LeftHandedControle;
        AssistedChainsControle = settingManager.AssistedChainsControle;

        skinsUnlock = settingManager.skinsUnlock;
        indexSkinSelected = settingManager.indexSkinSelected;
    }
}