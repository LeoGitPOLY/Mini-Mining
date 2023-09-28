using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class contentFaciliter : MonoBehaviour
{
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private DragMovDig movDig;

    [Header("Changeble Settings")]
    [SerializeField] private bool isActive;
    [SerializeField] [Range(0, 1)] private float valueMov;

    private EventsControlsUI controlsUI;
    private void Start()
    {
        controlsUI = EventsControlsUI.instance;

        if (isActive)
            joystick.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        arrowsChecker();
        awsdChecker();
        TreuilChecker();

    }

    private void arrowsChecker()
    {
        // LEFT ARROW --------------------------
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movDig.dig_horizontale = -1;
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            movDig.dig_horizontale = 0;

        }

        // RIGHT ARROW --------------------------
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            movDig.dig_horizontale = 1;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            movDig.dig_horizontale = 0;
        }

        // UP ARROW --------------------------
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            movDig.dig_Vertical = 1;
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            movDig.dig_Vertical = 0;
        }

        // DOWN ARROW --------------------------
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            movDig.dig_Vertical = -1;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            movDig.dig_Vertical = 0;
        }
    }
    private void awsdChecker()
    {
        // LEFT ARROW --------------------------
        if (Input.GetKey(KeyCode.A))
        {
            controlsUI.ChangeMovHoriz(-valueMov);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            controlsUI.ChangeMovHoriz(0);
        }

        // RIGHT ARROW --------------------------
        if (Input.GetKey(KeyCode.D))
        {
            controlsUI.ChangeMovHoriz(valueMov);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            controlsUI.ChangeMovHoriz(0);
        }

        // UP ARROW --------------------------
        if (Input.GetKey(KeyCode.W))
        {
            controlsUI.ChangeMovVerti(valueMov);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            controlsUI.ChangeMovVerti(0);
        }

        // DOWN ARROW --------------------------
        if (Input.GetKey(KeyCode.S))
        {
            controlsUI.ChangeMovVerti(-valueMov);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            controlsUI.ChangeMovVerti(0);
        }
    }
    private void TreuilChecker()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            controlsUI.DoubleTap();
        }
    }
}
