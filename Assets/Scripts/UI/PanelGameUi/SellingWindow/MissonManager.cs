using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MissonManager : MonoBehaviour
{
    [SerializeField] private float timeMissions;
    [SerializeField] private List<Mission> missions;

    [SerializeField] private PopOnEnter eventListenerOpen;


    [Header("Components:")]
    [SerializeField] private TextMeshProUGUI indexMission;
    [SerializeField] private TextMeshProUGUI Demande;
    [SerializeField] private TextMeshProUGUI Prix;
    [SerializeField] private GameObject bulleAccept;
    [SerializeField] private Image imgBck;

    private int indexInter = -1;
    private bool isBlock = false;
    private bool noMore = false;
    private const string NAME_TIME = "MissionTime";

    private void Start()
    {
        isBlock = TimeManager.instance.commanderMethode(NAME_TIME, deblockMission);
        eventListenerOpen.isOpenEvent += openMissionManager;

        openMissionManager();
    }
    private void Update()
    {
        if (isBlock && !noMore)
        {
            int time = (int)TimeManager.instance.getTimeRemaningByName(NAME_TIME) + 1;
            indexMission.text = Transformation.transformTime(time);
        }

    }

    private void openMissionManager()
    {
        if (isBlock)
            blockMission();
        else
            showMission();
    }

    private void showMission()
    {
        int index = ScoreManager.instance.indexMission;

        if (index >= missions.Count)
        {
            noMoreMission();
            return;
        }

        Mission mission = missions[index];

        if (indexInter != index)
        {
            imgBck.color = Color.white;
            indexMission.text = "Mission " + (index + 1);
            Demande.text = mission.nombreDemande + " " + mission.typeDemande;
            Prix.text = Transformation.transformMoney(mission.prix) + "$";

            indexInter = index;
        }
        ScoreManager score = ScoreManager.instance;

        if (score.isThereBlocCargo(mission.nombreDemande, mission.typeDemande))
            bulleAccept.SetActive(true);
        else
            bulleAccept.SetActive(false);

    }

    public void clickMission()
    {
        if (isBlock)
            return;

        ScoreManager score = ScoreManager.instance;
        Mission mission = missions[score.indexMission];

        if (score.isThereBlocCargo(mission.nombreDemande, mission.typeDemande))
        {
            score.addGold(mission.prix);
            score.removeBlocCargo(mission.nombreDemande, mission.typeDemande);
            score.indexMission++;

            if (score.indexMission < missions.Count)
            {
                blockMission();
                TimeManager.instance.AddTime(NAME_TIME, timeMissions, deblockMission);
            }
            else
                noMoreMission();
        }
    }

    private void blockMission()
    {
        Demande.text = "Waiting";
        Prix.text = "";
        imgBck.color = Color.gray;
        isBlock = true;
    }

    private void deblockMission()
    {
        isBlock = false;
        showMission();
    }
    private void noMoreMission()
    {
        Demande.text = "No more";
        Demande.color = Color.white;

        Prix.text = "";
        imgBck.color = Color.black;
        indexMission.text = "";
        bulleAccept.SetActive(false);

        noMore = true;
    }

}
