using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Cinamachine : MonoBehaviour
{
    /*
     * NE JAMAIS LIRE CE CODE!!!
     * D�GUEULASSE SVPP JE SUIS PLUS CAPABLE DE ME FORCER
     */
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject background;
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D LightFire;
    [SerializeField] private Animator animBlackScreen;

    [Header("Player Skin:")]
    [SerializeField] SpriteRenderer slotPlayerDecollage;
    [SerializeField] Image slotPlayerMoon;
    [SerializeField] SkinTheme skinTheme;

    [Header("scenecomplete:")]
    [SerializeField] GameObject decollage;
    [SerializeField] GameObject finalScene;

    private void Start()
    {
        determineSkin();

        StartCoroutine(fadeINOUTAfter());
        startBlocsDescente();
        StartCoroutine(cameraShake(0.1f, 10));
        StartCoroutine(lightShake(10));
    }

    private void determineSkin()
    {
        //TODO version mini mining
        SettingManager.instance.loadSettings();

        int settingIndex = SettingManager.instance.indexSkinSelected;
        Sprite spr = skinTheme.allSkin[settingIndex].SpriteToShow;

        slotPlayerDecollage.sprite = spr;
        slotPlayerMoon.sprite = spr;
    }

    private void startBlocsDescente()
    {
        Animator anim = background.GetComponent<Animator>();
        anim.Play("DescenteBlocs");
    }
    private IEnumerator cameraShake(float magnitude, float duration)
    {
        float elapse = 0.0f;
        Vector3 firstPositon = cam.transform.position;

        while (elapse < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            firstPositon -= new Vector3(0, 0.04f);

            cam.transform.position = firstPositon + new Vector3(x, y);

            elapse += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator lightShake(float duration)
    {
        float elapse = 0.0f;

        float innerRadFire = 4.5f;
        float outerRadFire = 6f;
        float intensFire = 1f;

        while (elapse < duration)
        {
            float aleatoireVar = Random.Range(-0.3f, 0.3f);
            float aleatoireIntense = Random.Range(-0.1f, 0.1f);

            LightFire.pointLightOuterRadius = outerRadFire + aleatoireVar;
            LightFire.pointLightInnerRadius = innerRadFire + aleatoireVar;
            LightFire.intensity = intensFire + aleatoireIntense;

            elapse += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator fadeINOUTAfter()
    {
        animBlackScreen.Play("FadeOUT");
        yield return new WaitForSeconds(10);
        animBlackScreen.Play("FadeIN");
        yield return new WaitForSeconds(2);
        decollage.SetActive(false);
        finalScene.SetActive(true);
        animBlackScreen.Play("FadeOUT");
    }
}