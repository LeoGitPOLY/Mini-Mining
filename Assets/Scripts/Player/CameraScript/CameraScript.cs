using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [Header("Transform:")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform bonderie;

    [Header("Ofset:")]
    [SerializeField] [Range(1, 10)] private float speedOfset;
    [SerializeField] [Range(0,3)] private float ofsetOfset;

    // Background Components:
    private List<ParrallaxEffect> backgroundElements;

    //Camera Components:
    private Camera cameraComponents;

    private Vector2 maxPosCamClamp;
    private Vector2 minPosCamClamp;

    private float vertExtent;
    private float horzExtent;

    private Vector3 positionNoOfset;

    //Bonderie Components:
    private Vector3 posBonderie;
    private Vector3 scalBonderie;

    //Trigger Components:
    private float camScale;

    private float timeScale;
    private float timeMove;

    private Vector2 maxPosInter;
    private Vector2 minPosInter;

    //Boolean:
    private bool isScaling;
    private bool isMoving;
    private bool isParrallax;


    private void Start()
    {
        cameraComponents = GetComponent<Camera>();
        transform.position = playerTransform.position + Vector3.back * 10;

        isScaling = false;
        isMoving = false;
        isParrallax = false;

        vertExtent = cameraComponents.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;

        posBonderie = bonderie.position;
        scalBonderie = bonderie.localScale;

        //maxPosCamClamp = new Vector2(1000, 1000);
        //minPosCamClamp = new Vector2(-1000, -1000);

        maxPosCamClamp = new Vector2(posBonderie.x + (scalBonderie.x / 2 - horzExtent), posBonderie.y + (scalBonderie.y / 2 - vertExtent));
        minPosCamClamp = new Vector2(posBonderie.x - (scalBonderie.x / 2 - horzExtent), posBonderie.y - (scalBonderie.y / 2 - vertExtent));

        maxPosInter = maxPosCamClamp;
        minPosInter = minPosCamClamp;

        positionNoOfset = transform.position;
    }
    private void FixedUpdate()
    {
        setCameraPosition();
        moveElementsParrallax();
    }

    //Update call function
    private void setCameraPosition()
    {
        if (isScaling)
        {
            cameraComponents.orthographicSize = LeanSmooth.linear(cameraComponents.orthographicSize, camScale, timeScale);

            vertExtent = cameraComponents.orthographicSize;
            horzExtent = vertExtent * Screen.width / Screen.height;

            if (cameraComponents.orthographicSize == camScale)
                isScaling = false;
        }
        if (isMoving)
        {
            minPosCamClamp = LeanSmooth.linear(minPosCamClamp, minPosInter, timeMove);
            maxPosCamClamp = LeanSmooth.linear(maxPosCamClamp, maxPosInter, timeMove);

            if (minPosCamClamp == minPosInter && maxPosCamClamp == maxPosInter)
                isMoving = false;
        }

        //Position NO ofset:
        float x_Cam = Mathf.Clamp(playerTransform.position.x, minPosCamClamp.x, maxPosCamClamp.x);
        float y_Cam = Mathf.Clamp(playerTransform.position.y, minPosCamClamp.y, maxPosCamClamp.y);
        float z_Cam = transform.position.z;

        Vector3 positionCam = new Vector3(x_Cam, y_Cam, z_Cam);
        positionNoOfset = positionCam;

        //Position WITH ofset:
        float speedY = speedOfset * Mathf.Pow(Mathf.Abs(transform.position.y - positionCam.y), ofsetOfset);
        float speedX = speedOfset * Mathf.Pow(Mathf.Abs(transform.position.x - positionCam.x), ofsetOfset);

        speedY = speedY > 0.5f ? speedY : 0.5f;
        speedX = speedX > 0.5f ? speedX : 0.5f;

        float posReelY = LeanSmooth.linear(transform.position.y, positionCam.y, speedY);
        float posReelX = LeanSmooth.linear(transform.position.x, positionCam.x, speedX);

        transform.position = new Vector3(posReelX, posReelY, positionCam.z);
    }
    private void moveElementsParrallax()
    {
        if (isParrallax)
            foreach (ParrallaxEffect item in backgroundElements)
            {
                item.moveParrallax(transform);
            }
    }

    public void EnteringZone(TrigerCam trigger)
    {
        if (trigger != null)
        {
            isMoving = true;
            isScaling = true;
            isParrallax = trigger.isParrallax();

            timeScale = trigger.getTimeScale();
            timeMove = trigger.getTimeMoving();

            CameraDefase(trigger.getPosMin(), trigger.getPosMax(), trigger.getSizeCam());
        }
    }

    private void CameraDefase(Vector2 minPos, Vector2 maxPos, float camScaleToCome)
    {
        camScale = camScaleToCome;

        float demiVertExtentToCome = camScaleToCome;
        float demiHorzExtentToCome = (camScaleToCome * Screen.width / Screen.height);

        Vector2 screenMin = new Vector2(positionNoOfset.x, positionNoOfset.y);
        Vector2 screenMax = new Vector2(positionNoOfset.x, positionNoOfset.y);

        //POUR LES X:
        if (maxPos.x != 10000)
        {
            maxPosInter.x = maxPos.x - demiHorzExtentToCome;
            maxPosCamClamp.x = screenMax.x;
        }
        else
            maxPosInter.x = posBonderie.x + (scalBonderie.x / 2 - demiHorzExtentToCome);

        if (minPos.x != -10000)
        {
            minPosInter.x = minPos.x + demiHorzExtentToCome;
            minPosCamClamp.x = screenMin.x;
        }
        else
            minPosInter.x = posBonderie.x - (scalBonderie.x / 2 - demiHorzExtentToCome);

        //POUR LES Y:
        if (maxPos.y != 10000)
        {
            maxPosInter.y = maxPos.y - demiVertExtentToCome;
            maxPosCamClamp.y = screenMax.y;
        }
        else
            maxPosInter.y = posBonderie.y + (scalBonderie.y / 2 - demiVertExtentToCome);

        if (minPos.y != -10000)
        {
            minPosInter.y = minPos.y + demiVertExtentToCome;
            minPosCamClamp.y = screenMin.y;
        }
        else
            minPosInter.y = posBonderie.y - (scalBonderie.y / 2 - demiVertExtentToCome);
    }

    public void subcribeParralax(ParrallaxEffect element)
    {
        if (backgroundElements == null)
            backgroundElements = new List<ParrallaxEffect>();

        backgroundElements.Add(element);
    }
    public IEnumerator cameraShakeHaflHalf(float magnitude, float duration, float waitBefore)
    {
        yield return new WaitForSeconds(waitBefore);

        float elapse = 0.0f;

        while (elapse < duration / 2)
        {
            float x = Random.Range(-1f, 1f) * magnitude * (elapse / duration) * 2;
            float y = Random.Range(-1f, 1f) * magnitude * (elapse / duration) * 2;

            transform.position += new Vector3(x, y);

            elapse += Time.deltaTime;
            yield return null;
        }

        while (elapse < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position += new Vector3(x, y);

            elapse += Time.deltaTime;
            yield return null;
        }
    }
    public void setPositionDirect(Vector2 newPosition)
    {
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        maxPosCamClamp = transform.position;
        minPosCamClamp = transform.position;
    }
}

