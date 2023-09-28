using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickOutside : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField] private bool destroyOnExit;
    [SerializeField] private GameObject windowToClose;

    private bool hasFocus = false;
    private bool notWorking = false;


    public void Update()
    {
        if (!hasFocus && !notWorking)
            if (Input.GetMouseButtonDown(0))
            {
                if (destroyOnExit)
                {
                    if (windowToClose != null)
                        Destroy(windowToClose);
                    else
                        Destroy(gameObject);
                }
                else
                {
                    if (windowToClose != null)
                        windowToClose.gameObject.SetActive(false);
                    else
                        this.gameObject.SetActive(false);
                }
                    
            }

    }

    public void destroyThis()
    {
        if (windowToClose != null)
            Destroy(windowToClose);
        else
            Destroy(gameObject);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hasFocus = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hasFocus = true;
    }
    public bool NotWorking { get => notWorking; set => notWorking = value; }
}



