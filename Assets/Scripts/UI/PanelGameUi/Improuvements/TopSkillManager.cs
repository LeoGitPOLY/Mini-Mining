using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(Image))]
public class TopSkillManager : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerExitHandler
{
    [Header("Imports:")]
    [SerializeField] private Transform parentBars;
    [SerializeField] private Transform parentInfoCase;
    [SerializeField] private GameObject prefabCaseGrise;

    [Header("Texts:")]
    [SerializeField] private GameObject childTextPrix;
    [SerializeField] private TextAsset textStats;

    [Header("image:")]
    [SerializeField] private Sprite pressedBtn;
    [SerializeField] private Sprite NormalBtn;
    [SerializeField] private Sprite[] skinColorBar;

    [Header("Info:")]
    [SerializeField] private bool haveGolden;
    [SerializeField] private EnumLevelName nameImprouvement;

    [Header("Color:")]
    [SerializeField] private Color colorSelected;
    [SerializeField] private Color colorNotSelected;


    private List<GameObject> BarAmeliorations;
    private List<string> StatsString;

    private ScoreManager score;
    private ImprouvementManager improuv;

    private void Start()
    {
        score = ScoreManager.instance;
        improuv = ImprouvementManager.instance;

        //Set Hit boxButton:
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;

        //ReadText:
        if (StatsString == null)
            StatsString = TextReader.getTextParagraphBYComma(textStats);

        //Instantiate all barAmelioration (7):
        const int nbBar = 7;
        const float spacingY = 0.05f;
        BarAmeliorations = new List<GameObject>();

        for (int i = 1; i <= nbBar; i++)
        {
            GameObject barInter = Instantiate(prefabCaseGrise, parentBars);
            RectTransform rectTransform = barInter.GetComponent<RectTransform>();

            BarAmeliorations.Add(barInter);

            float max_Y = (1f / nbBar) * (i - 1);
            float min_y = (1f / nbBar) * i;

            if (i == nbBar)
            {
                rectTransform.anchorMax = new Vector2(1, 1 - max_Y);
                rectTransform.anchorMin = new Vector2(0, 1 - min_y);

                if (!haveGolden)
                {
                    Destroy(barInter);
                }
            }
            else
            {
                rectTransform.anchorMax = new Vector2(1 - spacingY, 1 - max_Y);
                rectTransform.anchorMin = new Vector2(spacingY, 1 - min_y);
            }
        }

        //Modifie all barAmelioration:
        for (int i = 0; i < BarAmeliorations.Count; i++)
        {
            CaseImprouvementPrefab script = BarAmeliorations[i].GetComponent<CaseImprouvementPrefab>();
            EasyComponentsGetter getter = BarAmeliorations[i].GetComponent<EasyComponentsGetter>();
            script.instatiateCase(nameImprouvement, i, StatsString[i], parentInfoCase, this);

            if (i < improuv.getLevel(nameImprouvement))
            {
                getter.getImage(0).sprite = skinColorBar[i];
            }

            if (i == improuv.getLevel(nameImprouvement) - 1)
            {
                getter.getTxt(0).color = colorSelected;
                getter.getGameObject(0).SetActive(true);
            }
            else
            {
                getter.getTxt(0).color = colorNotSelected;
            }
        }

        setTextPrixNext();
    }

    //Amelioration Script:
    public void amelioration()
    {
        int prix = improuv.getPrice(nameImprouvement);

        if (ScoreManager.instance.haveEnoughtGold(prix) && prix != 0)
        {
            score.addGold(-prix);
            improuv.addLevels(nameImprouvement);
            DesignLevelImprouvement(improuv.getLevel(nameImprouvement) - 1);
            setTextPrixNext();
        }
    }
    private void DesignLevelImprouvement(int index)
    {
        CaseImprouvementPrefab script = BarAmeliorations[index].GetComponent<CaseImprouvementPrefab>();
        EasyComponentsGetter getter = BarAmeliorations[index].GetComponent<EasyComponentsGetter>();

        getter.getImage(0).sprite = skinColorBar[index];
        script.improuv();

        
        if (index - 1 >= 0)
        {
            EasyComponentsGetter getterBefore = BarAmeliorations[index - 1].GetComponent<EasyComponentsGetter>();
            getterBefore.getTxt(0).color = colorNotSelected;
            getterBefore.getGameObject(0).SetActive(false);
        }
        getter.getTxt(0).color = colorSelected;
        getter.getGameObject(0).SetActive(true);
    }

    //Gérer les Textes:
    private void setTextPrixNext()
    {
        int levelNext = improuv.getLevel(nameImprouvement) + 1;
        int prixNext = improuv.getPrice(nameImprouvement);

        string strTextNext;

        if (levelNext == 7 && haveGolden)
            strTextNext = "Gold Level: " + Transformation.transformMoney(prixNext) + "$";
        else if (levelNext > 7 || (levelNext > 6 && !haveGolden))
            strTextNext = "";
        else
            strTextNext = "Level " + levelNext + ": " + Transformation.transformMoney(prixNext) + "$";

        childTextPrix.GetComponent<TextMeshProUGUI>().SetText(strTextNext);
    }

    //Gérer les boutons:
    public void OnPointerClick(PointerEventData eventData)
    {

        Image img = GetComponent<Image>();

        img.sprite = NormalBtn;
        childTextPrix.SetActive(true);

        amelioration();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = pressedBtn;
        childTextPrix.SetActive(false);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = NormalBtn;
        childTextPrix.SetActive(true);
    }
}
