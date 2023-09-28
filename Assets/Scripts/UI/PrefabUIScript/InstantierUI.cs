using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantierUI : MonoBehaviour
{
    [SerializeField] private Transform pointToSpan;
    [SerializeField] private GameObject prefabToSpan;
    [SerializeField] private InteractiveScinematic[] scinematic;

    [SerializeField] private CellTheme theme;
    public void InstatiateNewDigUI(EnumCell cell)
    {
        GameObject prefab = Instantiate(prefabToSpan, this.gameObject.transform);
        EasyComponentsGetter getter = prefab.GetComponent<EasyComponentsGetter>();
        Animator anim = prefab.GetComponent<Animator>();

        anim.Play("NewDigFadeOut");
        getter.getGameObject(0).transform.position = Camera.main.WorldToScreenPoint( pointToSpan.position);
        getter.getTxt(0).SetText(MaterielName.getNameMat(cell));
    }

    public void InstatiateNewBlocUi(int index)
    {
        GameObject prefab = Instantiate(prefabToSpan, pointToSpan);
        EasyComponentsGetter getter = prefab.GetComponent<EasyComponentsGetter>();

        string textNewBloc = "New block found: " + MaterielName.getNameMat(theme.allCell[index].typeCell);
        
        getter.getTxt(0).SetText(textNewBloc);
        getter.getImage(0).sprite = theme.allCell[index].sprite[0];
    }
    public void InstatiateWarning(string title, string text, int index)
    {
        GameObject prefab = Instantiate(prefabToSpan, pointToSpan);
        EasyComponentsGetter getter = prefab.GetComponent<EasyComponentsGetter>();
        Animator anim = prefab.GetComponent<Animator>();

        anim.Play("OpenWarningInfo");
        getter.getTxt(0).SetText(text);
        getter.getTxt(1).SetText(title);

        const float time = 3.5f;
        scinematic[index].demarrer(time);
    }
    public void InstatiateCoffre(int indexState, int coins)
    {
        GameObject prefab = Instantiate(prefabToSpan, pointToSpan);
        EasyComponentsGetter getter = prefab.GetComponent<EasyComponentsGetter>();

        string textNewCoffre = "New chest found: + " + coins + " coins";


        getter.getImage(0).sprite = theme.allCell[(int) EnumCell.Treasure].sprite[indexState];
        getter.getTxt(0).SetText(textNewCoffre);
    }
    public void InstatiateNewMysteroidUi()
    {
        GameObject prefab = Instantiate(prefabToSpan, pointToSpan);
    }

    public void InstatiateSelling(string stringTxtTitle, string stringTxt)
    {
        GameObject prefab = Instantiate(prefabToSpan, pointToSpan);
        EasyComponentsGetter getter = prefab.GetComponent<EasyComponentsGetter>();

        getter.getTxt(0).SetText(stringTxtTitle);
        getter.getTxt(1).SetText(stringTxt);

        const float time = 3f;
        scinematic[0].demarrer(time);
    }
    public void InstatiateImprouvement(string stringTxtTitle, string stringTxt)
    {
        GameObject prefab = Instantiate(prefabToSpan, pointToSpan);
        EasyComponentsGetter getter = prefab.GetComponent<EasyComponentsGetter>();

        getter.getTxt(0).SetText(stringTxtTitle);
        getter.getTxt(1).SetText(stringTxt);

        const float time = 3f;
        scinematic[0].demarrer(time);
    }

    public void InstatiateWaitWinch(string stringTxt)
    {
        GameObject prefab = Instantiate(prefabToSpan, pointToSpan);
        EasyComponentsGetter getter = prefab.GetComponent<EasyComponentsGetter>();

        getter.getTxt(0).SetText(stringTxt);
    }
}
