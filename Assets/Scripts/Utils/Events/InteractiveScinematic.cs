using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class StageObject
{
    [SerializeField] public GameObject gameObjects;

    [Header("Time")]
    [SerializeField] public float timeBeforeActive;

    [Header("Location")]
    [SerializeField] public Transform location;
    [SerializeField] public Vector2 offsetLocation;
}

[System.Serializable]
public class ZoneNext
{
    [SerializeField] public GameObject gameObjects;

    [Header("Time")]
    [SerializeField] public float timeBeforeActive;

    [Header("Location")]
    [SerializeField] public Transform transform;
    [SerializeField] public Vector2 offsetLocation;
    [SerializeField] public Vector2 offsetDimension;

    [SerializeField] public UnityEvent actions;
}

[System.Serializable]
public class BlockScreen
{
    [Header("Time")]
    [SerializeField] public float timeBeforeActive;
    [SerializeField] public float timeDuration;

    [Header("Location")]
    [SerializeField] public Transform transform;
    [SerializeField] public Vector2 offsetLocation;
    [SerializeField] public Vector2 offsetDimension;
}

[System.Serializable]
public class Stage
{
    [SerializeField] public string name;
    [SerializeField] public float timeNext;
    [SerializeField] public StageObject[] stageObjects;
    [SerializeField] public ZoneNext stageZoneNext;
    [SerializeField] public BlockScreen blockScreen;
}

public class InteractiveScinematic : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject screenOverlay;
    [SerializeField] private GameObject blocScreenWindow;
    [SerializeField] private Stage[] stages;

    private int indexAt = 0;
    private List<GameObject> refInstance;
    private GameObject refZoneNext;

    //Demarer une scinematique
    public void demarrer(float time)
    {
        refInstance = new List<GameObject>();
        StartCoroutine(waitStartScinematic(time));
    }

    private void instanceInteractiveScinemetic()
    {
        Stage stage = stages[indexAt];

        foreach (StageObject objt in stage.stageObjects)
        {
            setComposanteStageObjet(objt);
        }

        setComposanteZoneNext(stage.stageZoneNext);
        setComposanteBlockScreen(stage.blockScreen);
    }

    //Gestion des objets de l'etape selectionnee
    private void setComposanteStageObjet(StageObject objt)
    {
        GameObject instance = Instantiate(objt.gameObjects, parent.transform);

        instance.transform.position = new Vector2(objt.location.position.x + objt.offsetLocation.x,
                                                  objt.location.position.y + objt.offsetLocation.y);
        instance.SetActive(false);
        StartCoroutine(waitSetVisible(instance, objt.timeBeforeActive));
        refInstance.Add(instance);
    }

    //Gestion du bouton next de l'etape selectionnee
    private void setComposanteZoneNext(ZoneNext zoneNext)
    {
        if (zoneNext.gameObjects == null)
        {
            StartCoroutine(waitNext(2));
            return;
        }

        GameObject instanceNext = Instantiate(zoneNext.gameObjects, parent.transform);

        instanceNext.transform.position = new Vector2(zoneNext.transform.position.x + zoneNext.offsetLocation.x,
                                                      zoneNext.transform.position.y + zoneNext.offsetLocation.y);

        RectTransform rectToHave = zoneNext.transform.gameObject.GetComponent<RectTransform>();
        instanceNext.GetComponent<RectTransform>().sizeDelta = new Vector2(rectToHave.sizeDelta.x + zoneNext.offsetDimension.x,
                                                                           rectToHave.sizeDelta.y + zoneNext.offsetDimension.y);

        Button button = instanceNext.GetComponent<Button>();
        button.onClick.AddListener(() => zoneNext.actions.Invoke());
        button.onClick.AddListener(nextInteractiveScinematic);

        instanceNext.SetActive(false);
        StartCoroutine(waitSetVisible(instanceNext, zoneNext.timeBeforeActive));
        refZoneNext = instanceNext;
    }

    //Gestion du block screen de l'etape selectionnee
    private void setComposanteBlockScreen(BlockScreen blocScreen)
    {
        if (blocScreen.transform == null)
            return;
        blocScreenWindow.transform.position = new Vector2(blocScreen.transform.position.x + blocScreen.offsetLocation.x,
                                                      blocScreen.transform.position.y + blocScreen.offsetLocation.y);

        RectTransform rectToHave = blocScreen.transform.gameObject.GetComponent<RectTransform>();
        blocScreenWindow.GetComponent<RectTransform>().sizeDelta = new Vector2(rectToHave.sizeDelta.x + blocScreen.offsetDimension.x,
                                                                           rectToHave.sizeDelta.y + blocScreen.offsetDimension.y);

        blocScreenWindow.SetActive(false);
        StartCoroutine(waitSetVisible(blocScreenWindow, blocScreen.timeBeforeActive));
        StartCoroutine(waitSetVisible(blocScreenWindow, blocScreen.timeDuration, false));
    }

    private IEnumerator waitSetVisible(GameObject objt, float waitForSecond, bool isVisible = true)
    {
        yield return new WaitForSeconds(waitForSecond);
        objt.SetActive(isVisible);
    }
    private IEnumerator waitStartScinematic(float waitForSecond)
    {
        yield return new WaitForSeconds(waitForSecond);
        parent.SetActive(true);

        if (screenOverlay != null)
            screenOverlay.SetActive(true);

        instanceInteractiveScinemetic();
    }
    private IEnumerator waitNext(float waitForSecond)
    {
        yield return new WaitForSeconds(waitForSecond);
        nextInteractiveScinematic();
    }

    //Pubic methode:
    public void nextInteractiveScinematic()
    {
        indexAt++;

        foreach (GameObject item in refInstance) { Destroy(item); }
        Destroy(refZoneNext);

        refInstance.Clear();
        refZoneNext = null;

        if (stages.Length > indexAt)
        {
            StartCoroutine(waitStartScinematic(stages[indexAt - 1].timeNext));
        }
        else
        {
            if (screenOverlay != null)
                screenOverlay.SetActive(false);
            parent.SetActive(false);
        }
    }
}
