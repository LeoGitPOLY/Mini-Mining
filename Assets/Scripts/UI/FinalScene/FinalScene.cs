using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class FinalScene : MonoBehaviour
{
    [SerializeField] private TextAsset textRemerciment;

    private EasyComponentsGetter getter;
    private List<string> stringsRemerciment;
    private int timeWait = 30;

    // Start is called before the first frame update
    void Start()
    {
        stringsRemerciment = TextReader.getTextParagraphBYSlash(textRemerciment);
        getter = GetComponent<EasyComponentsGetter>();

        setAllText();
        Invoke("setVisibleButtons", timeWait);
    }

    private void setAllText()
    {
        ScoreDataV2 data = SaveSystem.loadScore();
        string time = Transformation.transformTime((int)data.timeFinishGame, true);

        getter.getTxt(0).SetText(stringsRemerciment[0]);
        getter.getTxt(1).SetText(stringsRemerciment[1]);
        getter.getTxt(2).SetText(stringsRemerciment[2]);
        getter.getTxt(3).SetText(stringsRemerciment[3]);
        getter.getTxt(4).SetText(stringsRemerciment[4] + " " + time);
    }

    private void setVisibleButtons()
    {
        getter.setActiveGameObject(0, true);
    }

    public void goToEarth()
    {
        SceneManager.LoadScene(SceneName.SCENE_LOADING);
    }
}
