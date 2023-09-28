using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class NomalButton : MonoBehaviour,IPointerDownHandler, IPointerClickHandler, IPointerExitHandler
{
    [Header("Bouton Image:")]
    [SerializeField] private Sprite idleState;
    [SerializeField] private Sprite pressedState;

    [Header("Action:")]
    [SerializeField] private UnityEvent action;

    private Image imageComponent;
    private TextMeshProUGUI text;


    // Start is called before the first frame update
    void Start()
    {
        imageComponent = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        action?.Invoke();
        imageComponent.sprite = idleState;
        text.alpha = 1;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        imageComponent.sprite = pressedState;
        text.alpha = 0.6f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        imageComponent.sprite = idleState;
        text.alpha = 1;
    }
}
