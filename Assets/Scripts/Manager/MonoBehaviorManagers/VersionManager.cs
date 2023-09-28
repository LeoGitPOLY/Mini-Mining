using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionManager : MonoBehaviour
{
    public static VersionManager instance;

    // CHANGE VERSION ON EACH MAJOR UPDATE:
    public const string VERSION_CODE_MINI_MINING = EnumVersion.V2;

    // CURRENT VERSION ON READ:
    public string currentVersion;
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
    private void Start()
    {
        currentVersion = VERSION_CODE_MINI_MINING;
    }


    /*
    * Methode SAVE 
    */
    public void saveVersion()
    {
        SaveSystem.saveVersion(instance);
    }
    public void loadVersion()
    {
        VersionData data = SaveSystem.loadVersion();

        if (data != null)
        {
            currentVersion = data.version;
        }
        else
        {
            currentVersion = EnumVersion.V1;
        }
    }

}
