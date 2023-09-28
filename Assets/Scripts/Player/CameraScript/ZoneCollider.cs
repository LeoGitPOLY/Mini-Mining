using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneCollider : MonoBehaviour
{
    [SerializeField] private CameraScript camScript;
    [SerializeField] private torcheManager torcheMan;
    [SerializeField] private RopeManager ropeMan;
    [SerializeField] private PlayerMouvement playerMouvement;
    [SerializeField] private SaveManager saveManager;

    private string currentID = "";

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "triggerZone")
        {
            if (!collision.gameObject.name.Equals(currentID))
            {
                TrigerCam trigerCam = collision.GetComponent<TrigerCam>();
                TriggerPlayerInfo triggerPlayer = collision.GetComponent<TriggerPlayerInfo>();

                camScript.EnteringZone(trigerCam);
                torcheMan.setBoolLight(triggerPlayer.getBoolLight());
                ropeMan.localDeathByChains = triggerPlayer.getBoolDeathByChains();
                playerMouvement.isLosingFuel = triggerPlayer.getLoseFuel();
                saveManager.setLocalIsSave(triggerPlayer.getIsSaving());

                if (triggerPlayer.getIsBackGroundSound())
                    AudioManager.instance.restartMusic();
                else
                    AudioManager.instance.stopMusic();

                if(triggerPlayer.getIndexSound() != 10000)
                {
                    StopAllCoroutines();
                    StartCoroutine(FxSoundManager.instance.changeSound("zoneSound", triggerPlayer.getIndexSound(), true, true, true));
                }
                else
                {
                    FxSoundManager.instance.stopSound("zoneSound");
                }

                currentID = collision.gameObject.name;
            }
        }
    }
     public void setTriggerStay2D(GameObject zoneCam)
    {
        if (zoneCam.tag == "triggerZone")
        {
            if (!zoneCam.gameObject.name.Equals(currentID))
            {
                TrigerCam trigerCam = zoneCam.GetComponent<TrigerCam>();
                TriggerPlayerInfo triggerPlayer = zoneCam.GetComponent<TriggerPlayerInfo>();

                camScript.EnteringZone(trigerCam);
                torcheMan.setBoolLight(triggerPlayer.getBoolLight());
                ropeMan.localDeathByChains = triggerPlayer.getBoolDeathByChains();
                playerMouvement.isLosingFuel = triggerPlayer.getLoseFuel();

                currentID = zoneCam.gameObject.name;

            }
        }
    }
}
