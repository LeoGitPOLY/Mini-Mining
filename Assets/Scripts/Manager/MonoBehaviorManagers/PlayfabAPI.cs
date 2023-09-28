using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using TMPro;
using System;
using System.Collections;
using System.Net.NetworkInformation;

public class PlayfabAPI : MonoBehaviour
{
    public static PlayfabAPI instance = null;

    private string playfabIdPlayer = "";
    [SerializeField] TextMeshProUGUI debug;

    //Constante:
    private const string CUSTUM_ID = "sjsxnsnioos564";

    public const string NAME_DATA_VERSION = "versionData";
    public const string NAME_DATA_SKINSELECTION = "skinselectionData";
    public const string NAME_DATA_PAYMENT = "paymentData";

    public const string FREE_LEADERBOARD = "MiniMiningV2Free";
    public const string NORMAL_LEADERBOARD = "MiniMiningV2";

    public const string CONTENT_KEY = "WrongNames";

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }
    private void Start()
    {
        //StartCoroutine(generateLeaderboardScores());
        //StartCoroutine(sendTest());
    }
    public IEnumerator generateLeaderboardScores()
    {
        const bool isFreeLeaderboard = true;
        TextAsset textName = Resources.Load<TextAsset>("Text/TimePlayerFill");
        List<string> strNames = TextReader.getTextParagraph(textName);
        int nbInfo = 5;
        print("TTTTTTTT"+strNames[0]);
        //for (int i = 0; i < (int)(strNames.Count / nbInfo); i++)
        //{
        //    LoginCustum(i);
        //    yield return new WaitForSeconds(1);

        //    //1 info:
        //    UpdateUserName(strNames[i * nbInfo]);

        //    //2 info:
        //    int ff = Int16.Parse(strNames[i * nbInfo + 1]) * -1;
        //    string name = isFreeLeaderboard ? FREE_LEADERBOARD : NORMAL_LEADERBOARD;

        //    if (int.Parse(strNames[i * nbInfo + 3]) == 0)
        //        SendLeaderBoard(ff, name);


        //    //3-5 info:
        //    SetUserData(strNames[i * nbInfo + 4], strNames[i * nbInfo + 3], strNames[i * nbInfo + 2]);

        //    print(strNames[i * nbInfo] + ":" + strNames[i * nbInfo + 1] + "/" + strNames[i * nbInfo + 3] + "/" + strNames[i * nbInfo + 4]);

        //}
        LoginCustum(0);
        yield return new WaitForSeconds(1);

        //1 info:
        UpdateUserName("Sharlenerex");

        //2 info:     
         SendLeaderBoard(-21600, FREE_LEADERBOARD);


        //3-5 info:
        SetUserData("7","0","V1");
    }
    public IEnumerator sendTest()
    {
        LoginCustum(122121225);
        yield return new WaitForSeconds(1.5f);
        int time = -10227;
        //SendLeaderBoard(time, NORMAL_LEADERBOARD);
        SendLeaderBoard(time, FREE_LEADERBOARD);
        UpdateUserName("FirePatrick ");
    }

    //Methode Réellement utilisé:
    public void LoginCustum(int i)
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = i.ToString() + CUSTUM_ID,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccessLogin, OnError);
    }
    public void login(Action<LoginResult> methodeResult, Action<PlayFabError> methodeError)
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            //CustomId = "2891jwnwaqqqq1231212eeepom1",
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, methodeResult, methodeError);
    }
    public void UpdateUserName(string name, Action<UpdateUserTitleDisplayNameResult> methodeResult, Action<PlayFabError> methodeError)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, methodeResult, methodeError);
    }
    public void UpdateUserName(string name)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }
    public void GetLeaderboard(Vector2Int startEndPos, string leaderboardName, Action<GetLeaderboardResult> methodeLeaderboardResult, Action<PlayFabError> methodeError)
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = leaderboardName,
            StartPosition = startEndPos.x,
            MaxResultsCount = startEndPos.y,
        };
        PlayFabClientAPI.GetLeaderboard(request, methodeLeaderboardResult, methodeError);
    }
    public void SetLeaderBoard(int time, bool payement, Action<UpdatePlayerStatisticsResult> methodeLeaderboardResult, Action<PlayFabError> methodeError)
    {
        string name = payement ? NORMAL_LEADERBOARD : FREE_LEADERBOARD;
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = name,
                    Value = -time
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, methodeLeaderboardResult, methodeError);
    }
    public void SetUserData(string indexSkinSelected, string payment, string version = "V2")
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>() {
            {NAME_DATA_VERSION, version},
            {NAME_DATA_SKINSELECTION, indexSkinSelected},
            {NAME_DATA_PAYMENT, payment}
            },
            Permission = UserDataPermission.Public
        };
        PlayFabClientAPI.UpdateUserData(request, OnSucessDataSent, OnError);
    }
    public void SetUserDataSkin(string indexSkinSelected, string version = "V2")
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>() {
            {NAME_DATA_VERSION, version},
            {NAME_DATA_SKINSELECTION, indexSkinSelected},
            },
            Permission = UserDataPermission.Public
        };
        PlayFabClientAPI.UpdateUserData(request, OnSucessDataSent, OnError);
    }
    public void SetUserDataPayment(string payment, string version = "V2")
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>() {
            {NAME_DATA_VERSION, version},
            {NAME_DATA_PAYMENT, payment}
            },
            Permission = UserDataPermission.Public
        };
        PlayFabClientAPI.UpdateUserData(request, OnSucessDataSent, OnError);
    }
    public void GetUserDatas(string playerID, Action<GetUserDataResult> methodeDataResults, Action<PlayFabError> methodeError)
    {
        var request = new GetUserDataRequest
        {
            PlayFabId = playerID,
            Keys = new List<string> { NAME_DATA_VERSION, NAME_DATA_PAYMENT, NAME_DATA_SKINSELECTION }
        };
        PlayFabClientAPI.GetUserData(request, methodeDataResults, methodeError);
    }
    public void GetInfoPlayerOnLeaderboard(string leaderboardName, Action<GetLeaderboardAroundPlayerResult> methodePositionResult, Action<PlayFabError> methodeError)
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = leaderboardName,
            MaxResultsCount = 1
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, methodePositionResult, methodeError);
    }
    public void getContent(Action<GetTitleDataResult> methodeContentResult, Action<PlayFabError> methodeError)
    {
        var request = new GetTitleDataRequest
        {
            Keys = new List<string> { CONTENT_KEY }
        };
        PlayFabClientAPI.GetTitleData(request, methodeContentResult, methodeError);
    }
    public bool isConnectedWifi()
    {
        bool isConnectedToWifi = false;
        var interfaces = NetworkInterface.GetAllNetworkInterfaces();
        foreach (var iface in interfaces)
        {
            if (iface.OperationalStatus == OperationalStatus.Up && iface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
            {
                isConnectedToWifi = true;
                break;
            }
        }
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            isConnectedToWifi = true;
        }
        return isConnectedToWifi;
    }

    //NOT USE IN THE FINAL BUILD
    public void loginCurrentPlayer()
    {
        Debug.Log("Login");
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccessLogin, OnError);
    }
    public void SendLeaderBoard(int time, string name)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = name,
                    Value = time
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);
    }
    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "TestMini1",
            StartPosition = 0,
            MaxResultsCount = 10,
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardResult, OnError);
    }
    public void GetUserData()
    {
        var request = new GetUserDataRequest
        {

            Keys = null
        };
        PlayFabClientAPI.GetUserData(request, OnSucessDataReceived, OnError);
    }

    //Methodes infos fonctionnement
    void OnSuccessLogin(LoginResult result)
    {
        // playfabIdPlayer = result.InfoResultPayload.AccountInfo.PlayFabId;
        Debug.Log("Succed: " + result + " Player id: " + playfabIdPlayer);
        debug.SetText("Succed: " + result + " Player id: " + playfabIdPlayer);
    }
    void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Sucess update playFab: " + result);
        debug.SetText("Succed update playFab: " + result + "Player id: " + playfabIdPlayer);
    }
    public void OnLeaderboardResult(GetLeaderboardResult result)
    {
        foreach (var item in result.Leaderboard)
        {
            Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
        }
    }
    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Sucess update username: " + result);
        debug.SetText("Succed update username: " + result + "Player id: " + playfabIdPlayer);
    }
    void OnSucessDataSent(UpdateUserDataResult result)
    {
        Debug.Log("Successfully updated user data");
        debug.SetText("Successfully updated user data");
    }
    void OnSucessDataReceived(GetUserDataResult result)
    {
        if (result.Data == null || !result.Data.ContainsKey("PlayerName"))
        {
            Debug.Log("PlayerName not found");
            debug.SetText("PlayerName not found");
        }
        else
        {
            Debug.Log("PlayerName: " + result.Data["PlayerName"].Value);
            debug.SetText("PlayerName: " + result.Data["PlayerName"].Value);
        }
    }
    void OnError(PlayFabError error)
    {
        Debug.Log("Error in playfab: " + error);
        debug.SetText("Error in playfab: " + error);
    }
}
