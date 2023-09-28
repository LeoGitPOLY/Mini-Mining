using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuseumManager : MonoBehaviour, panel
{
    [SerializeField] private GameObject prefabFind;
    [SerializeField] private GameObject prefab_NOT_Find;

    [SerializeField] private Mysterious[] allMysteriouss;
    [SerializeField] private TextAsset textForMysteriouss;

    [SerializeField] private Transform parentPanel;
    [SerializeField] private GameObject PanelBig;


    private GameObject[] allPrefab;
    private List<string> strForMysteriouss;

    public void demarer()
    {
        setFind();
        strForMysteriouss = TextReader.getTextParagraphBYSlash(textForMysteriouss);
    }

    private void OnEnable()
    {
        if (ScoreManager.instance != null)
            setFind();
    }
    public void setFind()
    {
        int[] score = ScoreManager.instance.MysteryUnlock;

        if (allPrefab != null)
            foreach (GameObject item in allPrefab)
                Destroy(item);

        allPrefab = new GameObject[6];

        for (int i = 0; i < allPrefab.Length; i++)
        {
            GameObject game;

            if (score[i] == 0)
                game = Instantiate(prefab_NOT_Find, parentPanel);
            else
            {
                game = Instantiate(prefabFind, parentPanel);
                game.GetComponent<PrefabBaseManager>().getImgSkin().sprite = allMysteriouss[i].spr_Bloc;

                ButtonMuseumSlot button = game.GetComponent<ButtonMuseumSlot>();
                button.IndexSlot = i;
                button.InstanceLinked = this;
                
            }

            allPrefab[i] = game;
        }
    }

    public void setPanelFind(int index)
    {
        EasyComponentsGetter ecg = PanelBig.GetComponent<EasyComponentsGetter>();

        ecg.getTxt(0).SetText(strForMysteriouss[index * 2]);
        ecg.getTxt(1).SetText(strForMysteriouss[index * 2 + 1]);
        ecg.getImage(0).sprite = allMysteriouss[index].spr_Museum;

        PanelBig.SetActive(true);
    }
}
