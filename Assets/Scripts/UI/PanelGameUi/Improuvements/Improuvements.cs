using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Improuvements : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        firstTimePop();
    }

    private void firstTimePop()
    {
        if (!SettingManager.instance.firstThingsDone[4])
        {
            UIManagerPop.instance.instantiateImprouvements();
            SettingManager.instance.firstThingsDone[4] = true;
        }
    }
}
