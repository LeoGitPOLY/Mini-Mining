using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelTreuil : MonoBehaviour
{
    [SerializeField] private GameObject[] slot;
    [SerializeField] private TextMeshProUGUI textChains;


    //Prefab:
    [SerializeField] private Sprite IconTreuil;
    [SerializeField] private Sprite IconVide;



    public void setPanelTreuils(int nombreTreuil, int nombreChains, int nombreMaxTreuil)
    {
        int compteur = 0;

        if(ImprouvementManager.instance.ChainsLevel == 7) 
        {
            slot[5].SetActive(true);
            slot[6].SetActive(true);
        }
        else
        {
            slot[5].SetActive(false);
            slot[6].SetActive(false);

        }

        for (int i = 0; i < slot.Length; i++)
        {
            CanvasGroup cg = slot[i].GetComponent<CanvasGroup>();


            if (compteur < nombreMaxTreuil)
            {
                Image img = slot[i].GetComponent<Image>();

                cg.alpha = 1;

                if (compteur < nombreTreuil)
                    img.sprite = IconTreuil;
                else
                    img.sprite = IconVide;
            }
            else
            {
                cg.alpha = 0;

            }
            compteur++;
        }
        textChains.text = "X " + nombreChains.ToString();
    }
}
