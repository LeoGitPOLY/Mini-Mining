using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(Image))]
public class DragMovDig : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IDragHandler
{
    [SerializeField] [Range(0f, 1f)] float sensibilityVer;
    [SerializeField] [Range(0f, 1f)] float sensibilityHor;


    private Vector2 start;
    private Vector2 now;
    private Vector2 scaleZone;

    public int dig_Vertical;
    public int dig_horizontale;

    private bool on_Click = false;
    private bool on_dig = false;

    //Double tap
    private float time;
    private int tapCount;

    private float timeMaxClick = 0.3f;
    private float timeMaxBetweenClick = 0.3f;


    //Contrainte:
    private bool blockDig = false;
    private bool blockDoubleTap = false;

    //Instance Event:
    private EventsControlsUI controlsUI;


    private void Start()
    {
        EventsGameState.instance.onDead += reset;

        if (Time.deltaTime > 0.2f)
        {
            timeMaxBetweenClick *= 3;
            timeMaxClick *= 3;
        }

        on_Click = false;
        on_dig = false;

        dig_horizontale = 0;
        dig_Vertical = 0;

        tapCount = 0;
        time = 0;

        RectTransform rectTran = GetComponent<RectTransform>();
        scaleZone = new Vector2(rectTran.rect.width, rectTran.rect.height);
        controlsUI = EventsControlsUI.instance;
    }


    private void Update()
    {


        time += Time.deltaTime;

        if (on_Click && !on_dig)
        {
            if (Math.Abs(start.x - now.x) > (scaleZone.x * sensibilityHor))
            {
                if (now.x < start.x)
                {
                    dig_horizontale = -1;
                }
                else
                {
                    dig_horizontale = 1;
                }
                on_dig = true;
            }
            else if (Math.Abs(start.y - now.y) > (scaleZone.y * sensibilityVer))
            {
                if (now.y < start.y)
                {
                    dig_Vertical = -1;
                }
                else
                {
                    dig_Vertical = 1;
                }
                on_dig = true;
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        start = eventData.position;
        now = eventData.position;
        on_Click = true;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        start = eventData.position;
        now = eventData.position;
        on_Click = true;

        if (tapCount != 1)
        {
            tapCount = 0;
            time = 0;
        }
        else if (tapCount == 1 && time < timeMaxBetweenClick)
        {
            time = 0;
        }
        else
        {
            tapCount = 0;
            time = 0;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        on_Click = false;
        on_dig = false;

        dig_horizontale = 0;
        dig_Vertical = 0;

        if (tapCount == 0 && time < timeMaxClick)
        {
            tapCount = 1;
            time = 0;
        }
        else if (tapCount == 1 && time < timeMaxClick)
        {
            tapCount = 2;

            if (!blockDoubleTap)
            {
                controlsUI.DoubleTap();
                print("double click");
            }
        }
        else
        {
            tapCount = 0;
            time = 0;
        }

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        on_Click = false;
        on_dig = false;

        dig_horizontale = 0;
        dig_Vertical = 0;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!on_dig)
            now = eventData.position;
    }

    public void reset()
    {
        on_Click = false;
        on_dig = false;

        dig_horizontale = 0;
        dig_Vertical = 0;

        tapCount = 0;
        time = 0;
    }

    public void setBlockBoubleTap(bool isBlock)
    {
        blockDoubleTap = isBlock;
    }

    public void setBlockDig(bool isBlock)
    {
        blockDig = isBlock;
    }

    public bool getBlockDoubleTap()
    {
        return blockDoubleTap;
    }

    public bool getBlockDig()
    {
        return blockDig;
    }
}
