using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class CaseImprouvementPrefab : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI textCaseLevel;
    [SerializeField] private GameObject InfoCasePrefab;

    private Transform parentInfoCase;

    //Composent:
    private Animator animator;
    private string OPEN_INFO = "OpenInfo";
    private string CLOSE_INFO = "CloseInfo";
    private string IMPROUV = "Ameliore";
    private TopSkillManager parentTopSkill;

    //Info:
    private EnumLevelName nameImprouv;
    private int indexImprouv;
    private string stringinfoCase;


    public void instatiateCase(EnumLevelName _nameImprouv, int _indexImprouv, string _infoCase, Transform _parentInfoCase, TopSkillManager _parentTopSkill)
    {
        animator = GetComponent<Animator>();

        stringinfoCase = _infoCase;
        nameImprouv = _nameImprouv;
        indexImprouv = _indexImprouv + 1;
        parentInfoCase = _parentInfoCase;
        parentTopSkill = _parentTopSkill;

        if (indexImprouv != 7)
            textCaseLevel.SetText(nameImprouv + " " + indexImprouv);
        else
            textCaseLevel.SetText("Golden " + nameImprouv);
    }



    public void ShowInfo()
    {
        GameObject caseInfoInter = Instantiate(InfoCasePrefab, parentInfoCase);
        EasyComponentsGetter getter = caseInfoInter.GetComponent<EasyComponentsGetter>();
        InfoImprouvements info = caseInfoInter.GetComponent<InfoImprouvements>();
        bool isBuyable = indexImprouv == (ImprouvementManager.instance.getLevel(nameImprouv) + 1);
        
        //Set Texte:
        if (indexImprouv != 7)
            getter.getTxt(0).SetText("Level " + indexImprouv + ": " + nameImprouv);
        else
            getter.getTxt(0).SetText("Gold Level: " + nameImprouv);

        if (isBuyable)
        {
            getter.getGameObject(0).SetActive(true);
        }
        getter.getTxt(1).SetText("Price: " + Transformation.transformMoney(ImprouvementManager.instance.getPriceByLevel(nameImprouv, indexImprouv - 1)) + " $");
        getter.getTxt(2).SetText(stringinfoCase);
        info.setInfo(parentTopSkill);
    }

    public void hideInfo()
    {
        animator.Play(CLOSE_INFO);
        print(CLOSE_INFO);
    }

    public void improuv()
    {
        float timer = AnimationManager.getTimeStateByName(animator, IMPROUV);

        animator.Play(IMPROUV);
        Invoke("PlayAfterAnimImprouv", timer);

    }


    public void OnPointerClick(PointerEventData eventData)
    {       
            ShowInfo();   
    }
}
