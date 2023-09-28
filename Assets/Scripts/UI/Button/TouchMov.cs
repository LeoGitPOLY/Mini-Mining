using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(Image))]
public class TouchMov : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    public UnityEvent click;
    public UnityEvent released;

    public void OnPointerDown(PointerEventData eventData)
    {
        click?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        click?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        released?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        released?.Invoke();
    }

    
}
