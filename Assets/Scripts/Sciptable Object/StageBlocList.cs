using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBlocList : MonoBehaviour
{
    [Header("Dirt:")]
    [SerializeField] private List<int> stageDirt;
    [SerializeField] private int totalBlocDirt;

    [Header("Stone:")]
    [SerializeField] private List<int> stageStone;
    [SerializeField] private int totalBlocStone;

    [Header("Corundum:")]
    [SerializeField] private List<int> stageCorundum;
    [SerializeField] private int totalBlocCorundum;

    [Header("Obsidian:")]
    [SerializeField] private List<int> stageObsidian;
    [SerializeField] private int totalBlocObsidian;

    [Header("Tot:")]
    [SerializeField] private int totBlocs;
    private List<int> stageTotal;

    [Header("Bonderies:")]
    [SerializeField] private Transform bonderies;
    [SerializeField] private Transform Bunker;

    public List<int> getStageTot()
    {
        stageTotal = new List<int>();

        stageTotal.AddRange(stageDirt);
        stageTotal.AddRange(stageStone);
        stageTotal.AddRange(stageCorundum);
        stageTotal.AddRange(stageObsidian);

        return stageTotal;
    }

    public void calculTot()
    {
        getStageTot();
        totBlocs = 0;

        for (int i = 0; i < stageTotal.Count; i++)
        {
            totBlocs += stageTotal[i];
        }

        totalBlocDirt = 0;
        foreach (int item in stageDirt)
        {
            totalBlocDirt += item;
        }

        totalBlocStone = 0;
        foreach (int item in stageStone)
        {
            totalBlocStone += item;
        }

        totalBlocCorundum = 0;
        foreach (int item in stageCorundum)
        {
            totalBlocCorundum += item;
        }

        totalBlocObsidian = 0;
        foreach (int item in stageObsidian)
        {
            totalBlocObsidian += item;
        }

        
    }
    public void resizeMonde()
    {
        BonderiesManager manager = bonderies.GetComponent<BonderiesManager>();

        manager.resetTransformInfo(totBlocs);
        
    }
}
