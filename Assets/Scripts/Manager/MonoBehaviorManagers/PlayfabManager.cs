using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class RequestUpdate
{
    private float bestTime;
    private bool havePaid;

    public RequestUpdate(float bestTime, bool havePaid)
    {
        BestTime = bestTime;
        HavePaid = havePaid;
    }

    public float BestTime { get => bestTime; set => bestTime = value; }
    public bool HavePaid { get => havePaid; set => havePaid = value; }
}
public enum typeLeaderboard
{
    Free,
    Normal,
    Both,
    Default
}
public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager instance;

    //Private properties:
    private bool isready = false;
    private bool isreadyIteration = false;
    private bool isreadyPosition = false;
    private bool isLoading = false;

    private int freeTime = 0;
    private int normalTime = 0;
    private int positionPlayer = 11; //Limite pour pas etre top 10 

    private RequestUpdate currentUpdate;
    private typeLeaderboard typeRequest = typeLeaderboard.Default;

    //MonoBehaviour methode:
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }
    private void Start()
    {
        gestionGodDigStart();
    }

    //Public methode: 
    public void createdNewUpdateRequest(string timepaid)
    {
        int time = int.Parse(timepaid.Substring(1));
        int paidint = int.Parse(timepaid[0] + "");
        bool paid = paidint == 0 ? false : true;

        //print("time:" + time + " paid: " + paid);
        createdNewUpdateRequest(time, paid);
    }
    public void createdNewUpdateRequest(float time, bool paid)
    {
        RequestUpdate requestUpdate = new RequestUpdate(time, paid);
        SettingManager.instance.updateTime.Add(requestUpdate);
    }
    public void sendUpdateScore()
    {
        if (!isLoading)
        {
            isLoading = true;
            StartCoroutine(loopThroughAllRequest());
        }
    }
    public void gestionGodDigGameDone()
    {
        StartCoroutine(verificationPosition());
    }
    public void gestionGodDigStart()
    {
        if (SettingManager.instance.skinsUnlock[8] == 1)
        {
            StartCoroutine(verificationPosition());
        }
    }
    //IEnumerator methodes:
    private IEnumerator loopThroughAllRequest()
    {
        List<RequestUpdate> requestUpdates = SettingManager.instance.updateTime;
        int i = 0;
        foreach (var item in requestUpdates)
        {
            currentUpdate = item;

            StopCoroutine(sendUpdateScoreLogic());
            StartCoroutine(sendUpdateScoreLogic());


            yield return new WaitUntil(() => isreadyIteration);
            isreadyIteration = false;
            i++;
        }
        GereReussiteloadingAll();
    }
    private IEnumerator sendUpdateScoreLogic()
    {
        print("SendUpdate: " + "(Time: " + currentUpdate.BestTime + ", Paid: " + currentUpdate.HavePaid);

        CheckWifiConnection();
        yield return new WaitUntil(() => isready);
        isready = false;

        ConnectPlayfabServer();
        yield return new WaitUntil(() => isready);
        isready = false;

        GetTimePlayer();
        yield return new WaitUntil(() => isready);
        isready = false;

        CheckIfSendScore();
        yield return new WaitUntil(() => isready);
        isready = false;

        SendScoreOnPlayFab();
        yield return new WaitUntil(() => isready);
        isready = false;

        ChangeStatePlayerPlayfab();
        yield return new WaitUntil(() => isready);
        isready = false;
        isreadyPosition = false;

        //Wait pour que les serveurs updates
        yield return new WaitForSeconds(2);

        gestionGodDigGameDone();
        yield return new WaitUntil(() => isreadyPosition);
        isready = false;
        isreadyPosition = false;

        GereReussiteloading();
        print("[+]SENT");
    }
    private IEnumerator verificationPosition()
    {
        print("VerificationPosition");
        CheckWifiConnection();
        yield return new WaitUntil(() => isready);
        isready = false;

        ConnectPlayfabServer();
        yield return new WaitUntil(() => isready);
        isready = false;

        GetPositionPlayer();
        yield return new WaitUntil(() => isready);
        isready = false;

        GereReussiteGodDig();
        print("[+]GoldDig verification " + positionPlayer);
    }
    //Void Methodes:
    private void GereError()
    {
        StopAllCoroutines();
        isLoading = false;
        isreadyPosition = false;
        positionPlayer = 11;
        print("[+] Error");
    }
    private void GereReussiteGodDig()
    {
        if (positionPlayer < 10)
        {
            SettingManager.instance.skinsUnlock[(int) EnumSkinName.SkinGodDig] = 1;
        }
        else
        {
            SettingManager.instance.skinsUnlock[(int)EnumSkinName.SkinGodDig] = 0;

            if (SettingManager.instance.indexSkinSelected == (int)EnumSkinName.SkinGodDig)
            {
                SettingManager.instance.indexSkinSelected = (int) EnumSkinName.SkinNormal;          
                PlayerManagerAnim.instance.changeAnimatorController(EnumSkinName.SkinNormal);
            }
            if (SettingManager.instance.indexIconSelected == (int)EnumSkinName.SkinGodDig)
            {
                SettingManager.instance.indexIconSelected = (int)EnumSkinName.SkinNormal;
                PlayfabAPI.instance.SetUserDataSkin(((int)EnumSkinName.SkinNormal).ToString());
            }
        }
        isreadyPosition = true;
    }

    private void GereReussiteloading()
    {
        isreadyIteration = true;
        StopCoroutine(sendUpdateScoreLogic());
        print("[+] reussite Loading");
    }
    private void GereReussiteloadingAll()
    {
        print("[+] reussite Loading all");

        StopAllCoroutines();
        isLoading = false;

        SettingManager.instance.updateTime = new List<RequestUpdate>();
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
            OnError();
        }
    }
    private void ConnectPlayfabServer()
    {
        PlayfabAPI.instance.login(OnSuccessLogin, OnError);
    }
    private void GetTimePlayer()
    {
        PlayfabAPI.instance.GetInfoPlayerOnLeaderboard(PlayfabAPI.NORMAL_LEADERBOARD, OnTimePlayerResultNormal, OnError);
        PlayfabAPI.instance.GetInfoPlayerOnLeaderboard(PlayfabAPI.FREE_LEADERBOARD, OnTimePlayerResultFree, OnError);
    }
    private void GetPositionPlayer()
    {
        PlayfabAPI.instance.GetInfoPlayerOnLeaderboard(PlayfabAPI.NORMAL_LEADERBOARD, OnPositionPlayerResult, OnError);
    }
    private void CheckIfSendScore()
    {
        RequestUpdate req = currentUpdate;
        typeRequest = typeLeaderboard.Default;

        print("Check if send score: " + "(bestFree: " + freeTime + " bestnormal: " + normalTime + ") (req is paid: " + req.HavePaid + " req time: " + req.BestTime + ")");

        if (req == null)
            OnError();


        if (freeTime == 0 && !req.HavePaid)
        {
            if (req.BestTime <= normalTime || normalTime == 0)
                typeRequest = typeLeaderboard.Both;
            else
                typeRequest = typeLeaderboard.Free;

            isready = true;
            return;
        }

        if (req.HavePaid)
        {
            if (req.BestTime <= normalTime || normalTime == 0)
            {
                typeRequest = typeLeaderboard.Normal;
                isready = true;
            }
            else
                GereReussiteloading();
        }
        else
        {
            if (req.BestTime <= normalTime && req.BestTime < freeTime)
            {
                typeRequest = typeLeaderboard.Both;
                isready = true;
            }
            else if (req.BestTime <= freeTime)
            {

                typeRequest = typeLeaderboard.Free;
                isready = true;
            }
            else if (req.BestTime <= normalTime)
            {
                typeRequest = typeLeaderboard.Normal;
                isready = true;
            }
            else
                GereReussiteloading();
        }
        print("Type request: " + typeRequest);
    }
    private void SendScoreOnPlayFab()
    {
        float time = currentUpdate.BestTime;
        switch (typeRequest)
        {
            case typeLeaderboard.Free:
                PlayfabAPI.instance.SetLeaderBoard((int)time, false, OnSucessUpdateLeaderBoard, OnError);
                break;
            case typeLeaderboard.Normal:
                PlayfabAPI.instance.SetLeaderBoard((int)time, true, OnSucessUpdateLeaderBoard, OnError);
                break;
            case typeLeaderboard.Both:
                PlayfabAPI.instance.SetLeaderBoard((int)time, false, OnSucessUpdateLeaderBoard, OnError);
                PlayfabAPI.instance.SetLeaderBoard((int)time, true, OnSucessUpdateLeaderBoard, OnError);
                break;
            case typeLeaderboard.Default:
                break;
        }
        isready = true;
    }
    private void ChangeStatePlayerPlayfab()
    {
        RequestUpdate req = currentUpdate;

        if (typeRequest == typeLeaderboard.Both || typeRequest == typeLeaderboard.Normal)
        {
            string havePaidStr = req.HavePaid ? "1" : "0";
            PlayfabAPI.instance.SetUserDataPayment(havePaidStr);

        }

        //Change name (To be sure that every score have a name)
        PlayfabAPI.instance.UpdateUserName(SettingManager.instance.playerName);
        isready = true;
    }

    //Results methodes:
    public void OnSuccessLogin(LoginResult result)
    {
        isready = true;
        print("[0]Login success");
    }
    public void OnSucessUpdateLeaderBoard(UpdatePlayerStatisticsResult result)
    {
        isready = true;
        print("[0]Update API success");
    }
    public void OnTimePlayerResultNormal(GetLeaderboardAroundPlayerResult result)
    {
        int time = -(result.Leaderboard[0].StatValue);
        normalTime = time;
        print("Paid: " + time);
        isready = true;
    }
    public void OnTimePlayerResultFree(GetLeaderboardAroundPlayerResult result)
    {
        int time = -(result.Leaderboard[0].StatValue);
        freeTime = time;
        print("Free: " + time);
        isready = true;
    }
    public void OnPositionPlayerResult(GetLeaderboardAroundPlayerResult result)
    {
        isready = true;
        positionPlayer = result.Leaderboard[0].Position;
        print("Result playfab position" + positionPlayer);
    }
    public void OnError(PlayFabError error)
    {
        GereError();
    }
    public void OnError()
    {
        GereError();
    }
}
