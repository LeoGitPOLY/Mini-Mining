using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class JoystickBrawlstar : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private GameObject joystick;
    [SerializeField] private Canvas canvas;
    
    [Header("Offset:")]
    [SerializeField] private int offset_X;
    [SerializeField] private int offset_Y;

    private RectTransform joystickRect;
    private Joystick joystickComp;

    private Vector2 idlePosition;

    private void Awake()
    {
        joystickRect = joystick.GetComponent<RectTransform>();
        joystickComp = joystickRect.GetComponent<FixedJoystick>();
    }
    private void Start()
    {
        resetIdlePosition();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        joystickRect.anchoredPosition = eventData.position / canvas.scaleFactor;
        joystickComp.OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystickRect.anchoredPosition = idlePosition;
        joystickComp.OnPointerUp(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        joystickComp.OnDrag(eventData);
    }
    public void resetIdlePosition()
    {
        if (joystickComp == null || joystickRect == null)
            return;
        sideContoller side = SettingManager.instance.LeftHandedControle ? sideContoller.Left : sideContoller.Right;

        if (sideContoller.Left == side)
            idlePosition = new Vector2((canvas.GetComponent<RectTransform>().rect.width - joystickRect.sizeDelta.x / 2) - offset_X, (joystickRect.sizeDelta.y / 2) + offset_Y);
        if (sideContoller.Right == side)
            idlePosition = new Vector2( (joystickRect.sizeDelta.x / 2) + offset_X, (joystickRect.sizeDelta.y / 2) + offset_Y);

        joystickRect.anchoredPosition = idlePosition;
    }
}
