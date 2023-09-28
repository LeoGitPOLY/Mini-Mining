using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiTutorielManager : MonoBehaviour
{
    public static UiTutorielManager instance = null;

    [SerializeField] GameObject[] gameObjects;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

    }

    public void setTextByIndex(int index, string text)
    {
        EasyComponentsGetter getter = gameObjects[index].GetComponent<EasyComponentsGetter>();

        StopAllCoroutines();
        StartCoroutine(writeText(text, getter));
    }

    /*
     * SET VISIBLE:
     */
    public void setVisibleByIndex(int index, bool isVisible)
    {
        gameObjects[index].SetActive(isVisible);
    }

    /*
     * GET GAMEOBJECT:
     */
    public GameObject getGameobjectByIndex(int index)
    {
        return gameObjects[index];
    }

    IEnumerator writeText(string text, EasyComponentsGetter getter)
    {
        if (getter != null)
        {
            int nbCharStrIndex = text.Length;

            for (int i = 0; i < nbCharStrIndex; i++)
            {
                string textBD = text.Substring(0, i) + "<color=#00000000>" + text.Substring(i, nbCharStrIndex - i) + " </color>";
                getter.getTxt(0).SetText(textBD);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

}
