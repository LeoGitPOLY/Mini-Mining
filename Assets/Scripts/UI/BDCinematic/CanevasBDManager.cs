using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanevasBDManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> panelsBD;
    [SerializeField] private Animator animatorFadeInOut;
    [SerializeField] private TextAsset textBD;

    private EasyComponentsGetter getter;
    private List<string> stringsBD;
    private int indexAt = 0;

    // Start is called before the first frame update
    void Start()
    {
        stringsBD = TextReader.getTextParagraph(textBD);

        setBDVisible();
    }
    private void setBDVisible()
    {
        getter = panelsBD[indexAt].GetComponent<EasyComponentsGetter>();
        foreach (GameObject item in panelsBD)
        {
            item.SetActive(false);
        }
        panelsBD[indexAt].SetActive(true);
        getter.setActiveGameObject(1, true);

        StartCoroutine(writeText());
    }

    private void changePanelBD()
    {
        indexAt++;

        if (indexAt < panelsBD.Count)
            setBDVisible();
        else
        {
            PlayerData data = SaveSystem.loadPlayer();
            
            if(data == null)
                SceneManager.LoadScene(SceneName.SCENE_TUTORIEL);
            else
                SceneManager.LoadScene(SceneName.SCENE_LOADING);
        }
    }

    //Public Methodes:
    public void btnNextFrame()
    {
        float time = AnimationManager.getTimeStateByName(animatorFadeInOut, "fadeInOut");
        animatorFadeInOut.Play("fadeInOut");

        Invoke("changePanelBD", (time / 2) - 0.1f);
    }
    public void skipEcritureBD()
    {
        StopAllCoroutines();
        getter.getTxt(0).SetText(stringsBD[indexAt]);
        getter.setActiveGameObject(0, true);
        getter.setActiveGameObject(1, false);
    }

    //IEnumerator Methodes:
    IEnumerator writeText()
    {
        int nbCharStrIndex = stringsBD[indexAt].Length;

        for (int i = 0; i < nbCharStrIndex; i++)
        {
            string textBD = stringsBD[indexAt].Substring(0, i) +"<color=#00000000>" + stringsBD[indexAt].Substring(i, nbCharStrIndex - i) + " </color>";
            getter.getTxt(0).SetText(textBD);
            yield return null;
        }
        print("done");
        getter.setActiveGameObject(0, true);
        getter.setActiveGameObject(1, false);
    }
}
