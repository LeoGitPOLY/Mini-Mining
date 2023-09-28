using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManagerPop : MonoBehaviour
{
    public static UIManagerPop instance;

    //Canvas parents:
    [Header("Canvas:")]
    [SerializeField] private Canvas UIInfoCanvas;

    [Header("GameObject")]
    [SerializeField] private GameObject[] popPrefabWarnings;
    [SerializeField] private GameObject flechePrefabWarning;
    [SerializeField] private GameObject popLevelRequire;
    [SerializeField] private GameObject popFuelRequire;
    [SerializeField] private GameObject somethingWhentWrong;
    [SerializeField] private GameObject notEnoughtMoney;

    [Header("Instantier:")]
    [SerializeField] private InstantierUI instantierNewBloc;
    [SerializeField] private InstantierUI instantierNewType;
    [SerializeField] private InstantierUI instantierInfo;
    [SerializeField] private InstantierUI instantierNewMysterouid;
    [SerializeField] private InstantierUI instantierNewCoffre;
    [SerializeField] private InstantierUI instantierSelling;
    [SerializeField] private InstantierUI instantierImprouvement;
    [SerializeField] private InstantierUI instantierWaitWinch;


    [Header("Texts:")]
    [SerializeField] private TextAsset textInfo;
    private List<string> stringsInfo;

    private bool nextPopLevel = true;
    private bool cargoFull = false;
    private bool noTreuilLeft = false;
    private bool coldDownDone = true;
    private int currentFuelStat = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }
    private void Start()
    {
        //Fuel:
        EventsGameScore.instance.onChangeFuel += setFuelRequire;
        setFuelRequire();

        //Cargo:
        EventsGameScore.instance.onChangeCargo += setCargoFull;
        setCargoFull();

        //Treuil Left:
        EventsGameScore.instance.onChangeTreuil += setNoTreuilLeft;
        setNoTreuilLeft();
    }

    public void setLevelRequire(string levelRequire)
    {
        float timeDefault = 4f;
        string stringPop = "REQUIRES DIG: LEVEL " + levelRequire;
        GameObject pop;

        if (nextPopLevel)
        {
            nextPopLevel = false;

            pop = Instantiate(popLevelRequire, UIInfoCanvas.transform);
            TextMeshProUGUI textPop = pop.GetComponentInChildren<TextMeshProUGUI>();

            textPop.SetText(stringPop);

            Invoke("setNextPopLevel", timeDefault);
        }
    }
    public void setFuelRequire()
    {
        bool setToActif = true;
        int indexColor;

        int pourcentage = (int)(ScoreManager.instance.fuel / ImprouvementManager.instance.getStats(EnumLevelName.Fuel) * 100);
        //Arrondir les pourcentages
        if (pourcentage <= 10)
        {
            pourcentage = 10;
            indexColor = 0;
        }
        else if (pourcentage <= 20)
        {
            pourcentage = 20;
            indexColor = 1;
        }
        else if (pourcentage <= 40)
        {
            pourcentage = 40;
            indexColor = 2;
        }
        else
        {
            indexColor = 0;
            pourcentage = 0;
            setToActif = false;
        }

        if (pourcentage != currentFuelStat)
        {
            EasyComponentsGetter getter = popPrefabWarnings[0].GetComponent<EasyComponentsGetter>();
            Animator anim = popPrefabWarnings[0].GetComponent<Animator>();
            string stringPop = pourcentage + "%";

            popPrefabWarnings[0].SetActive(setToActif);
            getter.getTxt(0).SetText(stringPop);
            getter.getImage(0).sprite = getter.getSprite(indexColor);

            anim.Play("PopIn");

            currentFuelStat = pourcentage;

            instantiateInfo(0);
        }
    }
    public void setCargoFull()
    {
        bool isFull = ScoreManager.instance.cargo.Count >= ImprouvementManager.instance.CargoStats[ImprouvementManager.instance.CargoLevel];

        if (cargoFull != isFull)
        {
            Animator anim = popPrefabWarnings[1].GetComponent<Animator>();

            popPrefabWarnings[1].SetActive(isFull);
            anim.Play("PopIn");

            cargoFull = isFull;

            instantiateInfo(1);
        }
    }
    public void setNoTreuilLeft()
    {
        bool noTreuil = (ImprouvementManager.instance.getStats(EnumLevelName.Chains, true) - ScoreManager.instance.treuils.Count) == 0;

        if (noTreuilLeft != noTreuil)
        {
            Animator anim = popPrefabWarnings[2].GetComponent<Animator>();

            popPrefabWarnings[2].SetActive(noTreuil);
            anim.Play("PopIn");

            noTreuilLeft = noTreuil;

            instantiateInfo(2);
        }
    }
    public void setSomethingWhentWrong()
    {
        Animator anim = somethingWhentWrong.GetComponent<Animator>();

        somethingWhentWrong.SetActive(true);
        anim.Play("FadeInWaitFadeOut");
    }
    public void setnotEnoughtMoney()
    {
        Animator anim = notEnoughtMoney.GetComponent<Animator>();

        notEnoughtMoney.SetActive(true);
        anim.Play("FadeInWaitFadeOut");
    }

    //Instantiate:
    public void instantiateNewBloc(EnumCell type)
    {
        instantierNewBloc.InstatiateNewDigUI(type);
    }
    public void instantiateNewType(int indexType)
    {
        instantierNewType.InstatiateNewBlocUi(indexType);
    }
    public void instantiateInfo(int index)
    {
        if (stringsInfo == null)
            stringsInfo = TextReader.getTextParagraph(textInfo);

        if (!SettingManager.instance.firstThingsDone[index])
        {
            if (index < popPrefabWarnings.Length)
            {
                Animator selectionAnim = popPrefabWarnings[index].GetComponent<EasyComponentsGetter>().getGameObject(0).GetComponent<Animator>();
                Animator flecheAnim = flechePrefabWarning.GetComponent<Animator>();

                selectionAnim.Play("PopSelection");
                flecheAnim.Play("PopSelection");
            }

            instantierInfo.InstatiateWarning(stringsInfo[index * 2], stringsInfo[index * 2 + 1], index);
            SettingManager.instance.firstThingsDone[index] = true;
        }
    }
    public void instantiateImprouvements()
    {
        if (stringsInfo == null)
            stringsInfo = TextReader.getTextParagraph(textInfo);

        instantierImprouvement.InstatiateImprouvement(stringsInfo[8], stringsInfo[9]);
        
    }
    public void instantiateNewMysteroid()
    {
        instantierNewMysterouid.InstatiateNewMysteroidUi();
    }
    public void instantiateNewCoffre(int indexState, int coins)
    {
        instantierNewCoffre.InstatiateCoffre(indexState, coins);
    }
    public void instantiateSelling()
    {
        if (stringsInfo == null)
            stringsInfo = TextReader.getTextParagraph(textInfo);

        instantierSelling.InstatiateSelling(stringsInfo[6], stringsInfo[7]);
    }
    public void instantiateWaitWinch()
    {
        if (stringsInfo == null)
            stringsInfo = TextReader.getTextParagraph(textInfo);

        if (coldDownDone)
        {
            instantierWaitWinch.InstatiateWaitWinch(stringsInfo[10]);
            coldDownDone = false;
            StartCoroutine(coldDown(3));
        }
    }
    private void setNextPopLevel()
    {
        nextPopLevel = true;
    }
    private IEnumerator coldDown(float time)
    {
        yield return new WaitForSeconds(time);
        coldDownDone = true;
    }
}
