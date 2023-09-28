using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager  instance = null;

    //TextBox a gérer
    [Header("TextBox:")]
    [SerializeField] private TextMeshProUGUI goldText;

    //Window a gérer
    [Header("Windows:")]
    [SerializeField] private GameObject improuvWindow;
    [SerializeField] private GameObject UiGameState;
    [SerializeField] private GameObject UIControle;
    [SerializeField] private GameObject inventaire;
    [SerializeField] private GameObject UIInfo;
    [SerializeField] private GameObject blackScreen;
    [Space()]
    [SerializeField] private GameObject deathWindow;
    [SerializeField] private GameObject deathAllScreen;

    [Header("Text files:")]
    [SerializeField] private TextAsset textCauseDeath;

    [Header("Script:")]
    [SerializeField] private PanelCoinShopManager coinShopManager;

    [Header("Color:")]
    [SerializeField] private Color TextMove;
    [SerializeField] private Color TextIdle;

    private List<string> stringsCauseDeath;

    private string currentSceneToLoad = "";
    private int currentMoney;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        EventsGameScore.instance.onChangeGold += setAllText;
        EventsGameState.instance.onDeadType += setVisibleDeath;

        currentMoney = ScoreManager.instance.gold;
        goldText.SetText(Transformation.transformMoney(currentMoney));
    }

    private void setAllText()
    {
        StopAllCoroutines();
        StartCoroutine(addGoldProgressly());
    }

    //Management windows:
    private void setVisibleDeath(typeMort type)
    {
        if (stringsCauseDeath == null)
            stringsCauseDeath = TextReader.getTextParagraph(textCauseDeath);

        TextMeshProUGUI txt = deathWindow.GetComponent<EasyComponentsGetter>().getTxt(0);
        txt.text = stringsCauseDeath[(int)type];

        deathWindow.SetActive(true);
        deathAllScreen.SetActive(true);
        UIControle.SetActive(false);
        inventaire.SetActive(true);
        
    }
    public void setVisibleRevive()
    {
        deathAllScreen.SetActive(false);
        deathWindow.SetActive(false);
        blackScreen.SetActive(false);

        UIControle.SetActive(true);
        inventaire.SetActive(true);
        UIInfo.SetActive(true);
    }

    public void setVisibleStart()
    {
        blackScreen.SetActive(false);

        UIControle.SetActive(true);
        inventaire.SetActive(true);
        UIInfo.SetActive(true);
    }
    public void SetVisibleLoading(string nameSceneLoad)
    {
        Animator anim = blackScreen.GetComponent<Animator>();
        float time = AnimationManager.getTimeStateByName(anim, "FadeIN");

        blackScreen.SetActive(true);
        anim.Play("FadeIN");
        currentSceneToLoad = nameSceneLoad;

        Invoke("loadSceneByByName", time);
    }
    public void closeAllInfo()
    {
        UIInfo.SetActive(false);
        UiGameState.SetActive(false);
        inventaire.SetActive(false);
    }

    private void loadSceneByByName()
    {
        SceneManager.LoadScene(currentSceneToLoad);
    }

    //Ienumerator
    IEnumerator addGoldProgressly()
    {
        int lastMoney = currentMoney;
        int moneyNow = ScoreManager.instance.gold;
        int invers = 1;
        int stepSize;
        int step = 10;

        stepSize = (int) (Math.Abs(moneyNow - lastMoney) / step);

        if (moneyNow < lastMoney)
            invers = -1;

        goldText.color = TextMove;
        for (int i = 1; i <= step; i++)
        {
            int tot;

            if(i == step)
                tot = moneyNow;
            else
                tot = lastMoney + (i * invers * stepSize);

            goldText.SetText(Transformation.transformMoney(tot));
            yield return new WaitForSeconds(0.05f);
        }
        goldText.color = TextIdle;
        currentMoney = moneyNow;
    }

    //Script Manager:
    public void setBuyOtherConsumable()
    {
        coinShopManager.setBuyOtherConsumable();
    }
    public void wonMiniDig()
    {
        SettingManager.instance.skinsUnlock[(int)EnumSkinName.SkinMini] = 1;
        SettingManager.instance.indexSkinSelected = (int)EnumSkinName.SkinMini;
        PlayerManagerAnim.instance.changeAnimatorController(EnumSkinName.SkinMini);

    }
}
