using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedJoystick : Joystick
{
    private EventsControlsUI controlsUI;

    private void Update()
    {
        if(controlsUI == null)
            controlsUI = EventsControlsUI.instance;

        controlsUI.ChangeMovHoriz(Horizontal);
        controlsUI.ChangeMovVerti(Vertical);
    }
}