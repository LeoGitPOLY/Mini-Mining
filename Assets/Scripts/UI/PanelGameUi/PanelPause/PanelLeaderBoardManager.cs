using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public enum ErrorType
{
    joinLeaderboardError,
    defaultError
}
public class PanelLeaderBoardManager : MonoBehaviour
{
    [Header("Windows:")]
    [SerializeField] private GameObject inputFieldName;
    [SerializeField] private GameObject skinToSelect;
    [SerializeField] private GameObject loadingName;
    [SerializeField] private GameObject loadingLeaderboard;
    [SerializeField] private GameObject errorWindow;
    [SerializeField] private GameObject block;

    [Header("Prefab:")]
    [SerializeField] private GameObject prefabPlayerLeaderBoard;
    [SerializeField] private GameObject prefabSeparator;

    [Header("Listener:")]
    [SerializeField] private ListenerMonoBehaviour listenerPanelLeaderBoard;

    [Header("Images:")]
    [SerializeField] private Sprite ImgInvisible;
    [SerializeField] private Color colorYourPosition;

    [Space()]
    [SerializeField] private GameObject containerPlayerLeaderBoard;
    [SerializeField] private EasyComponentsGetter getterErrorWindow;
    [SerializeField] private EasyComponentsGetter getterLeaderBoard;
    [SerializeField] private Toggle toggleFreeLeaderboard;

    [Header("Icons:")]
    [SerializeField] private Sprite[] topPositionsIcon;
    [SerializeField] private Sprite paymentIcon;
    [SerializeField] private SkinTheme skins;

    //Reference des gamesobject:
    private List<GameObject> playerLeaderBoard;

    //Private properties:
    private bool isready = false;
    private bool isLoading = false;
    private bool isShowFree = false;
    private string errorType = ErrorType.defaultError.ToString();

    private List<PlayerLeaderboardEntry> leaderEntries;
    private List<Dictionary<string, UserDataRecord>> dataPlayers;
    private int positionPlayer;
    private int timePlayer;



    private void Awake()
    {
        listenerPanelLeaderBoard.start += StartPanel;
        listenerPanelLeaderBoard.enable += EnablePanel;
    }

    private void EnablePanel()
    {
        getterLeaderBoard.getGameObject(6).SetActive(!PlayfabAPI.instance.isConnectedWifi());
    }

    private void StartPanel()
    {
        string name = SettingManager.instance.playerName;

        //Name is random, so show input field name
        if (name[0] == '#')
        {
            OpenCloseBoxName(true);
        }
        else
        {
            RedrawLeaderBoardInfos();
        }
    }

    //Public methodes for call:
    public void ChangeName(bool openLeaderBoard = true)
    {
        isLoading = true;

        if (openLeaderBoard)
            StartCoroutine(ChangeNameLeaderboard());
    }
    public void RedrawLeaderBoardInfos()
    {
        isLoading = true;

        StartCoroutine(GenerateLeaderboard());
    }

    /*
     * LOGIC METHODES:
     */

    //IEnumerator methodes:
    private IEnumerator ChangeNameLeaderboard()
    {
        string newName = getterLeaderBoard.getTxt(3).text;

        //Start loading...
        loadingName.SetActive(true);

        CheckWifiConnection();
        yield return new WaitUntil(() => isready);
        isready = false;

        ConnectPlayfabServer();
        yield return new WaitUntil(() => isready);
        isready = false;

        CheckUsername(newName);
        yield return new WaitUntil(() => isready);
        isready = false;

        ChangeNamePlayFab(newName);
        yield return new WaitUntil(() => isready);
        isready = false;

        ChangeIconPlayFab(0);

        SettingManager.instance.playerName = newName;

        //Stop loading...
        loadingName.SetActive(false);
        OpenCloseBoxName(false);

        RedrawLeaderBoardInfos();
        block.SetActive(false);
    }
    public void ChangeIconLeaderBoard(int index)
    {
        ChangeIconPlayFab(index);
    }
    private IEnumerator GenerateLeaderboard()
    {
        //Start loading...
        loadingLeaderboard.SetActive(true);

        //Clear Screen
        CloseOpenleaderboard(false);

        CheckWifiConnection();
        yield return new WaitUntil(() => isready);
        isready = false;

        ConnectPlayfabServer();
        yield return new WaitUntil(() => isready);
        isready = false;

        GetLeaderboardInfos();
        yield return new WaitUntil(() => isready);
        isready = false;

        dataPlayers = new List<Dictionary<string, UserDataRecord>>();
        foreach (var item in leaderEntries)
        {
            GetUserData(item.PlayFabId);
            yield return new WaitUntil(() => isready);
            isready = false;
        }

        GetTimePlayer();
        yield return new WaitUntil(() => isready);
        isready = false;

        GetPositionPlayer();
        yield return new WaitUntil(() => isready);
        isready = false;

        SetInfoPlayer();
        InstantiatePlayersLeaderBoard();

    }

    //Void Methodes:
    private void SetInfoPlayer()
    {
        //Open/Close gameobjects:
        bool haveFinalTime = timePlayer != 0;
        getterLeaderBoard.setActiveGameObject(0, !haveFinalTime);
        getterLeaderBoard.setActiveGameObject(1, haveFinalTime);
        getterLeaderBoard.setActiveGameObject(2, haveFinalTime);
        getterLeaderBoard.setActiveGameObject(3, haveFinalTime);
        getterLeaderBoard.setActiveGameObject(4, haveFinalTime);
        getterLeaderBoard.setActiveGameObject(5, true);

        //Set name player:
        string name = SettingManager.instance.playerName;
        getterLeaderBoard.getTxt(0).SetText("-" + name + "-");

        //Set Skin Icon:
        getterLeaderBoard.getImage(0).sprite = skins.allSkin[SettingManager.instance.indexIconSelected].SpriteToShow;

        //Set Position Player:
        int pos = positionPlayer + 1;
        getterLeaderBoard.getTxt(2).SetText("#" + pos);

        //Set best time:
        if (haveFinalTime)
        {
            //string time = Transformation.transformTime((int)SettingManager.instance.bestTimeFinishGame, true);
            string time = Transformation.transformTime(timePlayer, true);
            getterLeaderBoard.getTxt(1).SetText(time);
        }
    }
    private void InstantiatePlayersLeaderBoard()
    {
        if (playerLeaderBoard == null)
        {
            playerLeaderBoard = new List<GameObject>();

            int nomberOfScores = 20;

            if (nomberOfScores > leaderEntries.Count)
                nomberOfScores = leaderEntries.Count;


            for (int i = 0; i < nomberOfScores; i++)
            {
                GameObject inter = Instantiate(prefabPlayerLeaderBoard, containerPlayerLeaderBoard.transform);
                EasyComponentsGetter getter = inter.GetComponent<EasyComponentsGetter>();

                //Set position LeaderBoard:
                if (i == 0 || i == 1 || i == 2)
                {
                    getter.getImage(0).sprite = topPositionsIcon[i];
                }
                else
                {
                    getter.setActiveGameObject(0, false);
                    getter.getTxt(0).SetText("" + (i + 1));
                }

                //Place separator:
                if (nomberOfScores != (i + 1))
                {
                    GameObject interSeparator = Instantiate(prefabSeparator, containerPlayerLeaderBoard.transform);
                }

                playerLeaderBoard.Add(inter);
            }
        }

        for (int i = 0; i < playerLeaderBoard.Count; i++)
        {
            if (i < leaderEntries.Count)
            {
                EasyComponentsGetter getter = playerLeaderBoard[i].GetComponent<EasyComponentsGetter>();
                Image ImageBoxPlayer = playerLeaderBoard[i].GetComponent<Image>();

                // Display name:
                string nameToDisplay = leaderEntries[i].DisplayName;

                if (nameToDisplay[0] == '#')
                    nameToDisplay = nameToDisplay.Substring(1);
                getter.getTxt(1).SetText(nameToDisplay);

                // Display time:
                getter.getTxt(2).SetText(Transformation.transformTime((int)leaderEntries[i].StatValue * -1, true));

                // Display version:
                if (dataPlayers[i].ContainsKey(PlayfabAPI.NAME_DATA_VERSION))
                {
                    string version = dataPlayers[i][PlayfabAPI.NAME_DATA_VERSION].Value;
                    getter.getTxt(3).SetText(version);
                }

                // Display skin selected:
                if (dataPlayers[i].ContainsKey(PlayfabAPI.NAME_DATA_SKINSELECTION))
                {
                    int indexSkin = int.Parse(dataPlayers[i][PlayfabAPI.NAME_DATA_SKINSELECTION].Value);
                    getter.getImage(1).sprite = skins.allSkin[indexSkin].SpriteToShow;
                }

                // Display if payment:
                if (dataPlayers[i].ContainsKey(PlayfabAPI.NAME_DATA_PAYMENT))
                {
                    int payment = int.Parse(dataPlayers[i][PlayfabAPI.NAME_DATA_PAYMENT].Value);

                    if (payment == 1 && !isShowFree)
                        getter.getImage(2).sprite = paymentIcon;
                    else
                        getter.getImage(2).sprite = ImgInvisible;
                }

                // Color if it's your position:
                if (positionPlayer == i && timePlayer != 0)
                {
                    ImageBoxPlayer.color = colorYourPosition;
                }
                else
                {
                    ImageBoxPlayer.color = new Color(1, 1, 1, 0);
                }
            }
        }

        //Stop loading...
        GereReussiteloading();

        //Show the screen
        CloseOpenleaderboard(true);
    }
    private void CloseOpenleaderboard(bool isActive)
    {
        containerPlayerLeaderBoard.SetActive(isActive);
    }
    private void GereErrorType()
    {
        int index = int.Parse(errorType[errorType.Length - 1].ToString());
        string error = errorType.Substring(0, errorType.Length - 1);

        if (error == ErrorType.joinLeaderboardError.ToString())
        {
            TextAsset textError = Resources.Load<TextAsset>("Text/InformartionText/ErrorJoinLeaderboard");
            List<string> strError = TextReader.getTextParagraph(textError);

            getterErrorWindow.getTxt(0).SetText(strError[index]);

            Animator anim = errorWindow.GetComponent<Animator>();
            anim.Play("FadeOut");
        }

        UIManagerPop.instance.setSomethingWhentWrong();

        loadingName.SetActive(false);
        loadingLeaderboard.SetActive(false);
        block.SetActive(false);

        isLoading = false;
        StopAllCoroutines();
    }
    private void GereReussiteloading()
    {
        StopAllCoroutines();

        loadingName.SetActive(false);
        loadingLeaderboard.SetActive(false);

        isLoading = false;
    }

    //Verification methodes
    private void CheckWifiConnection()
    {
        if (PlayfabAPI.instance.isConnectedWifi())
        {
            print("YES WIFI BITCH");
            isready = true;
        }
        else
        {
            print("NO WIFI BITCH");
            errorType = ErrorType.joinLeaderboardError.ToString() + "0";
            OnError();
        }
    }
    private void CheckUsername(string name)
    {
        const int spaceZero = 8203;

        if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name) || (int)(name[0]) == spaceZero)
        {
            errorType = ErrorType.joinLeaderboardError.ToString() + "2";
            print(errorType);
            OnError();
        }
        else
        {
            PlayfabAPI.instance.getContent(OnContentResult, OnError);
            isready = true;
        }
    }
    private void ConnectPlayfabServer()
    {
        errorType = ErrorType.joinLeaderboardError + "1";
        PlayfabAPI.instance.login(OnSuccessLogin, OnError);
    }
    private void ChangeNamePlayFab(string name)
    {
        errorType = ErrorType.joinLeaderboardError + "1";
        PlayfabAPI.instance.UpdateUserName(name, OnDisplayNameResult, OnError);
    }
    private void ChangeIconPlayFab(int index)
    {
        errorType = ErrorType.joinLeaderboardError + "1";
        PlayfabAPI.instance.SetUserDataSkin(index.ToString());
    }

    private void GetLeaderboardInfos()
    {
        string nameLeaderboard = isShowFree ? PlayfabAPI.FREE_LEADERBOARD : PlayfabAPI.NORMAL_LEADERBOARD;
        PlayfabAPI.instance.GetLeaderboard(new Vector2Int(0, 20), nameLeaderboard, OnLeaderboardResult, OnError);
    }
    private void GetUserData(string idPlayer)
    {
        PlayfabAPI.instance.GetUserDatas(idPlayer, OnDataPlayerResult, OnError);
    }
    private void GetPositionPlayer()
    {
        string nameLeaderboard = isShowFree ? PlayfabAPI.FREE_LEADERBOARD : PlayfabAPI.NORMAL_LEADERBOARD;
        PlayfabAPI.instance.GetInfoPlayerOnLeaderboard(nameLeaderboard, OnPositionPlayerResult, OnError);
    }
    private void GetTimePlayer()
    {
        string nameLeaderboard = isShowFree ? PlayfabAPI.FREE_LEADERBOARD : PlayfabAPI.NORMAL_LEADERBOARD;
        PlayfabAPI.instance.GetInfoPlayerOnLeaderboard(nameLeaderboard, OnTimePlayerResult, OnError);
    }

    //Results methodes:
    public void OnSuccessLogin(LoginResult result)
    {
        isready = true;
        print("[0]Login success");
    }
    public void OnDisplayNameResult(UpdateUserTitleDisplayNameResult result)
    {
        isready = true;
        print("[0]Change Name success");
    }
    public void OnLeaderboardResult(GetLeaderboardResult result)
    {
        isready = true;
        leaderEntries = result.Leaderboard;
        print("[0]get leaderboard sucess");
    }
    public void OnDataPlayerResult(GetUserDataResult result)
    {
        isready = true;
        dataPlayers.Add(result.Data);
    }
    public void OnPositionPlayerResult(GetLeaderboardAroundPlayerResult result)
    {
        isready = true;
        positionPlayer = result.Leaderboard[0].Position;
    }
    public void OnTimePlayerResult(GetLeaderboardAroundPlayerResult result)
    {
        int time = -(result.Leaderboard[0].StatValue);
        timePlayer = time;
        isready = true;
    }
    public void OnContentResult(GetTitleDataResult result)
    {
        isready = true;
    }
    public void OnError(PlayFabError error)
    {
        GereErrorType();
    }
    public void OnError()
    {
        GereErrorType();
    }

    //Small methodes:
    public void OpenCloseSkinSelection(bool isActive)
    {
        skinToSelect.SetActive(isActive);
    }
    public void OpenCloseBoxName(bool isActive)
    {
        inputFieldName.SetActive(isActive);
    }
    public void changeFreeLeaderboard()
    {
        if (!isLoading)
        {
            isShowFree = toggleFreeLeaderboard.isOn;
            RedrawLeaderBoardInfos();
        }
        else
        {
            toggleFreeLeaderboard.isOn = isShowFree;
        }

    }
    public void restartLeaderBoard()
    {
        if (!isLoading)
        {
            RedrawLeaderBoardInfos();
        }
    }
}
