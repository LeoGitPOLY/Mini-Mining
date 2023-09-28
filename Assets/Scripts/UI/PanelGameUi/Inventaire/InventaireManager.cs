using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventaireManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private GameObject BlockScreen;
    [SerializeField] private Animator animator;
    

    [Header("Components")]
    [SerializeField] private ProgressBar ProgressBar;
    [SerializeField] private PanelTreuil PanelTreuil;
    [SerializeField] private PanelCargo PanelCargo;

    private bool isEnable = false;

    // Start is called before the first frame update
    void Start()
    {
        isEnable = BlockScreen.activeSelf;
        restartAllInfo();
    }
   

    private void restartAllInfo()
    {
        ImprouvementManager improuvement = ImprouvementManager.instance;
        ScoreManager score = ScoreManager.instance;

        //Progresse Bar:
        ProgressBar.maximum = improuvement.FuelStats[improuvement.FuelLevel];
        ProgressBar.current = score.fuel;

        //Treuils:
        int nbTreuil = improuvement.TreuilStats[improuvement.ChainsLevel] - score.treuils.Count;
        int nbChains = improuvement.ChainsStats[improuvement.ChainsLevel] - score.nbCorde;
        int nombreMaxTreuil = improuvement.TreuilStats[improuvement.ChainsLevel];

        PanelTreuil.setPanelTreuils(nbTreuil, nbChains, nombreMaxTreuil);

        //Cargo
        PanelCargo.setPanelCargo();
    }

    public void OpenInventaire()
    {
        isEnable = !isEnable;
        string animName;

        if (isEnable)
        {
            animName = "Open";
            restartAllInfo();
        }
        else
            animName = "Close";

        AnimationManager.ChangeAnimationState(animator, animName);
        BlockScreen.SetActive(isEnable);
    }
}
