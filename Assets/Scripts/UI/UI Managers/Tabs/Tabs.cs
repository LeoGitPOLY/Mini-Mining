using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(Image))]
public class Tabs : MonoBehaviour, IPointerClickHandler, IComparable
{
    [SerializeField] private TabsManager tabsManager;
    [SerializeField] private int indexComparable = 0;

    public UnityEvent onTabSelected;

    private Image backGround;

    void Awake()
    {
        backGround = GetComponent<Image>();
        tabsManager.Subscibe(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tabsManager.OnTabSelected(this);
    }

    public void clickScript()
    {
        tabsManager.OnTabSelected(this);
    }

    public Image getBackgound()
    {
        return backGround;
    }

    public void Select()
    {
        onTabSelected?.Invoke();
    }

    public int CompareTo(object obj)
    {
        return indexComparable;
    }
    public int getComparable()
    {
        return indexComparable;
    }
}
