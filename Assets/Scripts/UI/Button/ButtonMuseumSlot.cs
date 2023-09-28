using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonMuseumSlot : MonoBehaviour, IPointerClickHandler
{
    private int indexSlot;
    private MuseumManager instanceLinked;

    public void OnPointerClick(PointerEventData eventData)
    {
        instanceLinked.setPanelFind(indexSlot);
    }

    public int IndexSlot { set => indexSlot = value; }
    public MuseumManager InstanceLinked { set => instanceLinked = value; }
}
