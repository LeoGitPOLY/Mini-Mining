using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrefabBaseManager : MonoBehaviour
{

    //Childs
    [Header("Images:")]
    [SerializeField] private Image imgSkin;
    [SerializeField] private Image imgSeconde;

    [Header("Main Texts:")]
    [SerializeField] private TextMeshProUGUI typeMatTxt;
    [SerializeField] private TextMeshProUGUI unitsMatTxt;
    [SerializeField] private TextMeshProUGUI prixByMatTxt;
    [SerializeField] private TextMeshProUGUI prixTotTxt;

    [Header("others:")]
    [SerializeField] private Toggle toggle;
    [SerializeField] private TextMeshProUGUI otherText1;
    [SerializeField] private TextMeshProUGUI otherText2;

    public int prixTotal { get; set; }
    public int units { get; set; }
    public int prixByMat { get; set; }
    public EnumCell type { get; set; }

    //ALL SET ALL:
    public void setAll(Sprite img, EnumCell type, int units, int prixUnitaire)
    {
        this.prixTotal = units * prixUnitaire;
        this.type = type;
        this.units = units;
        this.prixByMat = prixUnitaire;

        imgSkin.sprite = img;
        typeMatTxt.text = type.ToString();
        unitsMatTxt.text = units.ToString();
        prixByMatTxt.text = prixUnitaire.ToString() + " $";
        prixTotTxt.text = Transformation.transformMoney(prixTotal) + " $";
    }
    public void setAll(Sprite img, EnumCell type, string name, int units, int prixUnitaire)
    {
        this.prixTotal = units * prixUnitaire;
        this.type = type;
        this.units = units;
        this.prixByMat = prixUnitaire;

        imgSkin.sprite = img;
        typeMatTxt.text = name;
        unitsMatTxt.text = units.ToString();
        prixByMatTxt.text = prixUnitaire.ToString() + " $";
        prixTotTxt.text = Transformation.transformMoney(prixTotal) + " $";
    }
    public void setAll(Sprite img, EnumCell type, int units)
    {
        this.type = type;
        this.units = units;

        imgSkin.sprite = img;
        typeMatTxt.text = type.ToString();
        unitsMatTxt.text = units.ToString();
    }
    public void setAll(Sprite img, EnumCell type, string name, int units)
    {
        this.type = type;
        this.units = units;

        imgSkin.sprite = img;
        typeMatTxt.text = name;
        unitsMatTxt.text = units.ToString();
    }


    public void setNouveauPrixByMat(int prixUnitaire)
    {
        prixByMat = prixUnitaire;
        prixTotal = prixByMat * units;

        prixByMatTxt.text = prixUnitaire.ToString() + " $";
        prixTotTxt.text = Transformation.transformMoney(prixTotal) + " $";
    }

    //GETTER:
    public bool IsSelected()
    {
        return toggle.isOn;
    }
    public void setSelected(bool selected)
    {
        toggle.isOn = selected;
    }

    public Image getImgSkin()
    {
        return imgSkin;
    }
    public Image getImgSeconde()
    {
        return imgSeconde;
    }
    public TextMeshProUGUI getTypeMatTxt()
    {
        return typeMatTxt;
    }
    public TextMeshProUGUI getUnitsMatTxt()
    {
        return unitsMatTxt;
    }
    public TextMeshProUGUI getPrixByMatTxt()
    {
        return prixByMatTxt;
    }
    public TextMeshProUGUI getPrixTotTxt()
    {
        return prixTotTxt;
    }
    public TextMeshProUGUI getOtherText1()
    {
        return otherText1;
    }
    public TextMeshProUGUI getOtherText2()
    {
        return otherText2;
    }
}
