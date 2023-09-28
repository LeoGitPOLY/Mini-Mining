using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterManager : MonoBehaviour
{
    [Header("Teleporteurs:")]
    [SerializeField] GameObject[] teleporteurs;
    [SerializeField] private Animator animPlayer;

    private Animator[] animTeleporteurs = new Animator[2];
    private bool isTeleporting;

    const string ANIMATION_NAME = "Teleport";
    const string ANIMATION_PLAYER_SRINK = "Srink";
    const string ANIMATION_PLAYER_BIGGER = "Getbigger";

    private void Start()
    {
        animTeleporteurs[0] = teleporteurs[0].GetComponentInChildren<Animator>();
        animTeleporteurs[1] = teleporteurs[1].GetComponent<Animator>();
        gestionTeleporter();

        isTeleporting = false;
    }

    public void teleportButton(int indexTeleporteur)
    {
        float timeAnim = AnimationManager.getTimeStateByName(animTeleporteurs[indexTeleporteur], ANIMATION_NAME);
        animTeleporteurs[indexTeleporteur].Play(ANIMATION_NAME);

        if (!isTeleporting)
        {
            StartCoroutine(changeWorldPlace(timeAnim, indexTeleporteur));
            StartCoroutine(animationTaillePlayer(timeAnim));
            isFirstTime();
            isTeleporting = true;
        }
    }
    private IEnumerator changeWorldPlace(float waitForSeconde, int indexTeleporteur)
    {
        yield return new WaitForSeconds(waitForSeconde);
        EventsGameState.instance.teleport(indexTeleporteur);
        isTeleporting = false;
        gestionChaine(indexTeleporteur);
    }
    private IEnumerator animationTaillePlayer(float waitForSeconde)
    {
        const float timeBeforeSrink = 0.8f;
        yield return new WaitForSeconds(timeBeforeSrink);
        animPlayer.Play(ANIMATION_PLAYER_SRINK);
        yield return new WaitForSeconds(waitForSeconde - timeBeforeSrink);
        animPlayer.Play(ANIMATION_PLAYER_BIGGER);

    }
    private void isFirstTime()
    {
        if(ScoreManager.instance.teleporterFind == 1)
        {
            ScoreManager.instance.teleporterFind = 2;
            gestionTeleporter();
        }
    }
    private void gestionTeleporter()
    {
        int state = ScoreManager.instance.teleporterFind;

        if (state == 0)
        {
            teleporteurs[0].SetActive(false);
            teleporteurs[1].SetActive(false);
        }
        else if (state == 1)
        {
            teleporteurs[0].SetActive(true);
            teleporteurs[1].SetActive(false);
        }
        else if (state == 2)
        {
            teleporteurs[0].SetActive(true);
            teleporteurs[1].SetActive(true);
        }
    }
    private void gestionChaine(int indexTeleporteur)
    {
        const int profondeurTel = 68;
        ImprouvementManager improuv = ImprouvementManager.instance;

        //Vers le top monde
        if(indexTeleporteur == 0)
        {
            ScoreManager.instance.removeAllChains();
            ScoreManager.instance.nbCorde = 0;
            print("TO THE TOP");
        }
        //vers le bas
        else if(indexTeleporteur == 1)
        {
            ScoreManager.instance.removeAllChains();
            ScoreManager.instance.nbCorde = profondeurTel;
            print("TO BAS");
        }
    }
}
