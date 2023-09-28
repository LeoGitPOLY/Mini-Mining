using System;
using UnityEngine;

public class GeneralTabs : MonoBehaviour
{
    private int indexTab;
    private Action<int> methodeToRun;

    public void RunMethode()
    {
        methodeToRun?.Invoke(indexTab);
    }

    public int IndexTab { get => indexTab; set => indexTab = value; }
    public Action<int> MethodeToRun { get => methodeToRun; set => methodeToRun = value; }
}
