using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScinematicController : MonoBehaviour
{
    [Header("Panel Explication:")]
    [SerializeField] private PanelExplicationTutorielManagerAnim panelExpl;

    [Header("Parserinfo:")]
    [SerializeField] private TutorielParser parserInfo;

    [Header("TriggerZoneCamera")]
    [SerializeField] private ZoneCollider zoneCollider;
    [Space()]
    [SerializeField] private GameObject TriggerCamera1;
    [SerializeField] private GameObject TriggerCamera2;
    [SerializeField] private GameObject TriggerCamera3;
    [SerializeField] private GameObject TriggerCamera4;

    [Header("Texte:")]
    [SerializeField] private TextAsset textTutoriel;

    private DragMovDig dragZone;
    private PlayerMouvement mouvPlayer;
    private UiTutorielManager localInstance;

    private int indexAtScene = 1;
    private List<string> stringsTutoriel;

    // Start is called before the first frame update
    void Start()
    {
        localInstance = UiTutorielManager.instance;

        dragZone = localInstance.getGameobjectByIndex(6).GetComponent<DragMovDig>();
        mouvPlayer = GameObject.FindObjectOfType<PlayerMouvement>();

        EventsControlsUI.instance.onDoubleTap += SwitchScene;
        stringsTutoriel = TextReader.getTextParagraph(textTutoriel);

        setScenceIndex();
    }
    private void setScenceIndex()
    {
        switch (indexAtScene)
        {
            case 1:
                StartCoroutine(Scene1());
                break;
            case 2:
                SwitchScene();
                StartCoroutine(Scene3());
                break;
            case 3:
                StartCoroutine(Scene3());
                break;
            case 4:
                StartCoroutine(Scene4());
                break;
            case 5:
                StartCoroutine(Scene5());
                break;
            case 6:
                StartCoroutine(Scene6());
                break;
            case 7:
                StartCoroutine(Scene7());
                break;
            case 8:
                Scene8();
                break;
            case 9:
                Scene9();
                break;
            case 10:
                StartCoroutine(Scene10());
                break;
            case 11:
                SceneManager.LoadScene(SceneName.SCENE_LOADING);
                break;
            default:
                break;
        }
    }

    //ALL SCENE:
    IEnumerator Scene1()
    {
        //Cam:
        zoneCollider.setTriggerStay2D(TriggerCamera1);

        //BulleText:
        localInstance.setVisibleByIndex(0, true);
        localInstance.setTextByIndex(0, stringsTutoriel[indexAtScene - 1]);

        //wait
        yield return new WaitForSeconds(0.5f);

        //AllScreenAccept:
        localInstance.setVisibleByIndex(2, true);

        //BlockBoubleTap&&BlockDig:
        dragZone.setBlockBoubleTap(true);
        dragZone.setBlockDig(true);
    }
    IEnumerator Scene2()
    {
        //Cam:
        zoneCollider.setTriggerStay2D(TriggerCamera2);

        //BulleText:
        localInstance.setVisibleByIndex(0, true);
        localInstance.setTextByIndex(0, stringsTutoriel[indexAtScene - 1]);

        //REMOVE:
        //AllScreenAccept:
        localInstance.setVisibleByIndex(2, false);


        //NEW APPARITION:
        //Wait:
        yield return new WaitForSeconds(0.2f);
        //LeftRight:
        localInstance.setVisibleByIndex(1, true);
    }
    IEnumerator Scene3()
    {
        //Cam:
        zoneCollider.setTriggerStay2D(TriggerCamera2);

        //BulleText:
        localInstance.setVisibleByIndex(0, true);
        localInstance.setTextByIndex(0, stringsTutoriel[indexAtScene - 1]);

        //REMOVE:
        //LeftRight:
        localInstance.setVisibleByIndex(1, false);
        //AllScreenAccept:
        localInstance.setVisibleByIndex(2, false);

        //NEW APPARITION:
        //ArrowDirection:
        localInstance.setVisibleByIndex(4, true);
        //SelectionBloc:
        localInstance.setVisibleByIndex(5, true);
        localInstance.getGameobjectByIndex(5).gameObject.transform.position = new Vector2(-1, 0);
        //Wait:
        yield return new WaitForSeconds(0.5f);

        //Explication:
        panelExpl.setExplication(1);
        localInstance.setVisibleByIndex(15, true);

        //Joystick:
        localInstance.setVisibleByIndex(13, true);
    }
    IEnumerator Scene4()
    {
        //Cam:
        zoneCollider.setTriggerStay2D(TriggerCamera2);

        //BulleText:
        localInstance.setVisibleByIndex(0, true);
        localInstance.setTextByIndex(0, stringsTutoriel[indexAtScene - 1]);

        //REMOVE:
        //Joystick:
        localInstance.getGameobjectByIndex(3).GetComponent<Joystick>().reset(typeMort.Chains);
        localInstance.getGameobjectByIndex(3).GetComponent<CanvasGroup>().alpha = 1;
        localInstance.getGameobjectByIndex(14).GetComponent<JoystickBrawlstar>().resetIdlePosition();
        localInstance.setVisibleByIndex(13, false);
        EventsControlsUI.instance.ChangeMovHoriz(0);

        //ArrowDirection:
        localInstance.setVisibleByIndex(4, false);

        //NEW APPARITION:
        //SelectionBloc:
        localInstance.setVisibleByIndex(5, true);
        localInstance.getGameobjectByIndex(5).gameObject.transform.position = new Vector2(0, 0);
        //wait
        yield return new WaitForSeconds(0.5f);

        //Explication:
        panelExpl.setExplication(2);

        //DragZone:
        localInstance.setVisibleByIndex(6, true);
        dragZone.setBlockDig(false);
        //ArrowDig:
        localInstance.setVisibleByIndex(7, true);


    }
    IEnumerator Scene5()
    {
        //Cam:
        zoneCollider.setTriggerStay2D(TriggerCamera3);

        //BulleText:
        localInstance.setVisibleByIndex(0, true);
        localInstance.setTextByIndex(0, stringsTutoriel[indexAtScene - 1]);

        //REMOVE:
        //DragZone:
        localInstance.getGameobjectByIndex(6).GetComponent<DragMovDig>().reset();
        localInstance.setVisibleByIndex(6, false);
        //ArrowDig:
        localInstance.setVisibleByIndex(7, false);

        //NEW APPARITION:
        //SelectionBloc:
        localInstance.getGameobjectByIndex(5).gameObject.transform.position = new Vector2(2, -2);
        //wait:
        yield return new WaitForSeconds(1f);

        //Explication:
        panelExpl.setExplication(3);

        //Joystick:
        localInstance.setVisibleByIndex(13, true);
        //localInstance.getGameobjectByIndex(3).GetComponent<Animator>().enabled = false;


    }
    IEnumerator Scene6()
    {
        //Cam:
        zoneCollider.setTriggerStay2D(TriggerCamera3);

        //BulleText:
        localInstance.setVisibleByIndex(0, true);
        localInstance.setTextByIndex(0, stringsTutoriel[indexAtScene - 1]);

        //REMOVE:
        //Joystick:
        localInstance.getGameobjectByIndex(3).GetComponent<Joystick>().reset(typeMort.Chains);
        localInstance.getGameobjectByIndex(14).GetComponent<JoystickBrawlstar>().resetIdlePosition();
        localInstance.setVisibleByIndex(13, false);
        //SelectionBloc:
        localInstance.setVisibleByIndex(5, false);

        //NEW APPARITION:
        //wait:
        yield return new WaitForSeconds(0.5f);

        //Explication:
        panelExpl.setExplication(4);

        //DragZone:
        localInstance.setVisibleByIndex(6, true);
        dragZone.setBlockDig(true);
        dragZone.setBlockBoubleTap(false);
        //DoubleTapText:
        localInstance.setVisibleByIndex(12, true);
    }
    IEnumerator Scene7()
    {
        //Cam:
        zoneCollider.setTriggerStay2D(TriggerCamera4);

        //BulleText:
        localInstance.setVisibleByIndex(0, true);
        localInstance.setTextByIndex(0, stringsTutoriel[indexAtScene - 1]);

        //REMOVE:
        //DoubleTapText:
        localInstance.setVisibleByIndex(12, false);

        //NEW APPARITION:
        //SelectionBloc:
        localInstance.setVisibleByIndex(5, true);
        localInstance.getGameobjectByIndex(5).gameObject.transform.position = new Vector2(2, -3);
        //DragZone:
        dragZone.setBlockDig(false);
        dragZone.setBlockBoubleTap(true);
        //wait:
        yield return new WaitForSeconds(0.5f);
        //Explication:
        panelExpl.setExplication(5);

        //Joystick:
        localInstance.setVisibleByIndex(13, true);
        //ArrowDig:
        localInstance.setVisibleByIndex(8, true);
        //BlocDeplacement:
        mouvPlayer.BlockHorizontaleMov = true;
    }
    private void Scene8()
    {
        //REMOVE:
        //Explication:
        localInstance.setVisibleByIndex(15, false);

        //NEW APPARITION:
        //SelectionBloc:
        localInstance.getGameobjectByIndex(5).gameObject.transform.position = new Vector2(2, -4);
    }
    private void Scene9()
    {
        //REMOVE:
        //SelectionBloc:
        localInstance.setVisibleByIndex(5, false);
    }
    IEnumerator Scene10()
    {
        //REMOVE:
        //BulleText:
        localInstance.setVisibleByIndex(0, false);
        //Joystick:
        localInstance.setVisibleByIndex(13, false);
        //ArrowDig:
        localInstance.setVisibleByIndex(8, false);
        //DragZone:
        localInstance.setVisibleByIndex(6, false);

        //NEW APPARITION:
        //PanelNoir
        localInstance.setVisibleByIndex(9, true);
        Animator anim = localInstance.getGameobjectByIndex(9).GetComponent<Animator>();
        anim.Play("FadeIN");
        //wait:
        yield return new WaitForSeconds(AnimationManager.getTimeStateByName(anim, "FadeIN") + 0.3f);
        //BulleText:
        localInstance.setVisibleByIndex(0, true);
        localInstance.setTextByIndex(0, stringsTutoriel[7]);
        //PanelAutre:
        localInstance.setVisibleByIndex(10, true);
        Animator anim2 = localInstance.getGameobjectByIndex(10).GetComponent<Animator>();
        anim2.Play("FadeINquick");

        //wait:
        yield return new WaitForSeconds(5f);
        //next:
        localInstance.setVisibleByIndex(11, true);
    }

    //SWITCH SCENE:
    public void SwitchScene()
    {
        indexAtScene++;
        setScenceIndex();
    }

    //METHODE UNIQUE TUTORIEL:
    public void setInfoExplication()
    {
        List<int> indexExplication = new List<int>() { 0, 0, 0, 1, 2, 3, 4, 5 };

        if (indexExplication[indexAtScene] != 0)
            panelExpl.setInfo(indexExplication[indexAtScene]);
    }

    public void setLeftHanded(bool leftHanded)
    {
        parserInfo.IsLeftHanded = leftHanded;

        SettingManager.instance.LeftHandedControle = leftHanded;
        ControleManager.instance.setControleSide(leftHanded ? sideContoller.Left : sideContoller.Right);
    }

}
