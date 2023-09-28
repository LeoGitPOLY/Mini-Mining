using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCallNextTutoriel : MonoBehaviour
{
    [SerializeField] ScinematicController scinematicController;

    private bool oneTime = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!oneTime)
        {
            scinematicController.SwitchScene();
            Destroy(this.gameObject);
            oneTime = true;
        }
    }
}
