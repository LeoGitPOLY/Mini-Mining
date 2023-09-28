using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Rendering.Universal;


public class BunkerManager : MonoBehaviour
{
    [Header("Rocket:")]
    [SerializeField] private GameObject gameObjectFusee;
    [SerializeField] private GameObject[] collidersRockets;
    [SerializeField] private Transform positionAscenseurFinal;
    [SerializeField] private GameObject particules;
    [SerializeField] private Light2D LightFire;
    [SerializeField] private CameraScript cam;


    
    public void RocketFly()
    {
        TimeManager.instance.gameIsDone();
        StartCoroutine(RocketFlyEnumerator());
    }

    private IEnumerator RocketFlyEnumerator()
    {
        //ReplacePlayerTop:
        EventsGameState.instance.finish();

        //CloseAllUI:
        UIManager.instance.closeAllInfo();

        //CLOSE WALL:
        collidersRockets[0].SetActive(true);

        //LiftPlateform:
        Vector2 positionTarget = positionAscenseurFinal.position;
        Vector2 positionCurrent = collidersRockets[1].transform.position;
        float vel = 0.5f;

        while (positionTarget != positionCurrent)
        {
            positionCurrent = LeanSmooth.linear(positionCurrent, positionTarget, vel);
            collidersRockets[1].transform.position = positionCurrent;
            yield return null;
        }
        print("endPlateform");

        //Wait:
        yield return new WaitForSeconds(2);
        print("endWait");

        //StartCamera:
        StartCoroutine(cam.cameraShakeHaflHalf(0.05f, 20, 3));

        //StartParticule:
        particules.SetActive(true);

        //Fire And LiftUp:
        float innerRadFire = 1f;
        float outerRadFire = 6f;
        float intensFire = 0;

        while (innerRadFire < 4.5)
        {
            float aleatoireVar = Random.Range(-0.5f, 0.5f);

            innerRadFire += 0.02f;
            intensFire += 0.05f;

            LightFire.pointLightOuterRadius = outerRadFire;
            LightFire.pointLightInnerRadius = innerRadFire;
            LightFire.intensity = intensFire + aleatoireVar;

            if (innerRadFire > 3)
                gameObjectFusee.transform.position += new Vector3(0, 0.02f);

            yield return new WaitForSeconds(0.1f);
        }
        print("endFire");

        //UICLoseWindow:
        UIManager.instance.SetVisibleLoading(SceneName.SCENE_SPACE);

        //ContinueUp:
        while (gameObjectFusee.transform.position.y < 13)
        {
            gameObjectFusee.transform.position += new Vector3(0, 0.02f);

            yield return new WaitForSeconds(0.1f);
        }

    }

}
