using UnityEngine;

[System.Serializable]
public class VersionData
{
    public string version;

    public VersionData(VersionManager manager)
    {
        version = manager.currentVersion;
    }
}

