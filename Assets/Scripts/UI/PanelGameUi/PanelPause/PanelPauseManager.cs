using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelPauseManager : MonoBehaviour
{
    [SerializeField] private TabsManager tabsManager;
    public void setVisibleSettings()
    {
        this.gameObject.SetActive(true);
        tabsManager.OnTabSelected(0);
    }

    public void setVisibleSkinShop()
    {
        this.gameObject.SetActive(true);
        tabsManager.OnTabSelected(1);
    }

    public void setVisibleMoney()
    {
        this.gameObject.SetActive(true);
        tabsManager.OnTabSelected(2);
    }
    public void setVisibleLeaderBoard()
    {
        this.gameObject.SetActive(true);
        tabsManager.OnTabSelected(3);
    }
}
