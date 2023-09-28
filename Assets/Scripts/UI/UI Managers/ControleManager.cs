using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum sideContoller
{
    Right,
    Left,
}
public class ControleManager : MonoBehaviour
{
    public static ControleManager instance = null;
    [SerializeField] [Range(0, 1)] private float alphaLevelVisible_Dig = 0.3f;
    [SerializeField] [Range(0, 1)] private float alphaLevelNotVisible_Dig = 0;

    [SerializeField] [Range(0, 1)] private float alphaLevelVisible_Dep = 1;
    [SerializeField] [Range(0, 1)] private float alphaLevelNotVisible_Dep = 0.3f;


    [SerializeField] private Image[] imgs_Dig;
    [SerializeField] private Image[] imgs_Dep;
    [SerializeField] private RectTransform[] rectTransforms;

    [SerializeField] private GameObject horizontaleJoystick;
    [SerializeField] private GameObject verticalJoystick;
    [SerializeField] private GameObject BothJoystick;

    [SerializeField] private JoystickBrawlstar joysIns;
    [SerializeField] private FixedJoystick joystick;

    // pos = {(maxX, maxY), (minX, minY)}
    private const float distXCent = 0.1f;
    private const float distYTop = 0;
    private const float distYTopJoy = 0.15f;

    private Vector2[] posJoystickRight = { new Vector2(0.5f - distXCent, 1 - distYTopJoy), new Vector2(0, 0) };
    private Vector2[] posDragZoneRight = { new Vector2(1, 1 - distYTop), new Vector2(0.5f - distXCent, 0) };

    private Vector2[] posJoystickLeft = { new Vector2(1, 1 - distYTopJoy), new Vector2(0.5f + distXCent, 0) };
    private Vector2[] posDragZoneLeft = { new Vector2(0.5f + distXCent, 1 - distYTop), new Vector2(0, 0) };

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    private void Start()
    {
        setControleVisible(SettingManager.instance.VisibilityControle);
        setControleSide(SettingManager.instance.LeftHandedControle ? sideContoller.Left : sideContoller.Right);
    }
    private void Update()
    {
        AxisOptionsVisible(joystick.AxisOptions);
    }

    public void setControleVisible(bool isVisible)
    {
        foreach (Image item in imgs_Dig)
        {
            if (isVisible)
                item.color = new Color(item.color.r, item.color.b, item.color.g, alphaLevelVisible_Dig);
            else
                item.color = new Color(item.color.r, item.color.b, item.color.g, alphaLevelNotVisible_Dig);
        }
        foreach (Image item in imgs_Dep)
        {
            if (isVisible)
                item.color = new Color(item.color.r, item.color.b, item.color.g, alphaLevelVisible_Dep);
            else
                item.color = new Color(item.color.r, item.color.b, item.color.g, alphaLevelNotVisible_Dep);
        }
    }

    public void setControleSide(sideContoller side)
    {
        if (side == sideContoller.Right)
        {
            //DragPad:
            rectTransforms[0].anchorMax = posDragZoneRight[0];
            rectTransforms[0].anchorMin = posDragZoneRight[1];
            rectTransforms[0].localScale = new Vector2(1, 1);

            //JoystickPad:
            rectTransforms[1].anchorMax = posJoystickRight[0];
            rectTransforms[1].anchorMin = posJoystickRight[1];
        }

        if (side == sideContoller.Left)
        {
            //DragPad:
            rectTransforms[0].anchorMax = posDragZoneLeft[0];
            rectTransforms[0].anchorMin = posDragZoneLeft[1];
            rectTransforms[0].localScale = new Vector2(-1, 1);

            //if affichage
            if (rectTransforms.Length >= 3)
                rectTransforms[2].localScale = new Vector2(-1, 1);

            //JoystickPad:
            rectTransforms[1].anchorMax = posJoystickLeft[0];
            rectTransforms[1].anchorMin = posJoystickLeft[1];
        }

        joysIns.resetIdlePosition();
    }

    private void AxisOptionsVisible(AxisOptions axisOption)
    {
        switch (axisOption)
        {
            case AxisOptions.Both:
                if (joystick.BlockLeftRight)
                {
                    horizontaleJoystick.SetActive(false);
                    verticalJoystick.SetActive(true);
                    BothJoystick.SetActive(false);
                }
                else
                {
                    horizontaleJoystick.SetActive(false);
                    verticalJoystick.SetActive(false);
                    BothJoystick.SetActive(true);
                }
                break;
            case AxisOptions.Horizontal:
                if (!joystick.BlockLeftRight)
                {
                    horizontaleJoystick.SetActive(true);
                    verticalJoystick.SetActive(false);
                    BothJoystick.SetActive(false);
                }
                break;
            default:
                break;
        }
    }
}
