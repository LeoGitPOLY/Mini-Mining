using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlayerInfo : MonoBehaviour
{
    [SerializeField] private bool boolLight;
    [SerializeField] private bool boolDeathByChains;
    [SerializeField] private bool loseFuel;
    [SerializeField] private bool isSaving;
    [SerializeField] private bool isBackGroundSound;
    [SerializeField] private int indexFxSound;

    public bool getBoolLight()
    {
        return boolLight;
    }
    public bool getBoolDeathByChains()
    {
        return boolDeathByChains;
    }
    public bool getLoseFuel()
    {
        return loseFuel;
    }
    public bool getIsSaving()
    {
        return isSaving;
    }
    public bool getIsBackGroundSound()
    {
        return isBackGroundSound;
    }
    public int getIndexSound()
    {
        return indexFxSound;
    }

}
