using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabsManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> windows;

    [Header("sprite:")]
    [SerializeField] private Sprite tabSelected;
    [SerializeField] private Sprite tabIdle;
    [SerializeField] private Color colorSelected;
    [SerializeField] private Color colorIdle;

    [Header("Start:")]
    [SerializeField] bool isStart;
    [SerializeField] int indexStart;

    private List<Tabs> tabs;

    private void Start()
    {
        if (isStart)
            OnTabSelected(indexStart);
    }
    public void Subscibe(Tabs tab)
    {
        if (tabs == null)
            tabs = new List<Tabs>();

        tabs.Add(tab);

    }
    public void OnTabSelectedSprite(int indexSelect)
    {
        foreach (Tabs item in tabs)
        {
            if (item.getComparable() == indexSelect)
                 ResetSprites(item);
        }
       
    }
    public void OnTabSelected(int indexSelect)
    {
        foreach (Tabs item in tabs)
        {
            if(item.getComparable() == indexSelect) 
                OnTabSelected(item);
        }
       
    }

    public void OnTabSelected(Tabs tab)
    {
        tab.Select();
        ResetSprites(tab);
        ResetWindows(tab.transform.GetSiblingIndex());
    }

    private void ResetSprites(Tabs tab)
    {
        ResetTabs();

        tab.getBackgound().sprite = tabSelected;
        tab.getBackgound().color = colorSelected;
    }

    private void ResetTabs()
    {
        foreach (Tabs item in tabs)
        {
            item.getBackgound().sprite = tabIdle;
            item.getBackgound().color = colorIdle;
        }
    }

    private void ResetWindows(int index)
    {
        for (int i = 0; i < windows.Count; i++)
        {
            if (i == index)
                windows[i].SetActive(true);
            else
                windows[i].SetActive(false);
        }
    }

}
