using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance = null;

    [Header("Player:")]
    [SerializeField] private GameObject player;

    [Header("Camera:")]
    [SerializeField] private CameraScript camera;

    [Header("Information:")]
    [SerializeField] Transform spawnPlayer;
    [SerializeField] Transform RevivePoint;
    [SerializeField] Transform teleportTopPoint;
    [SerializeField] Transform teleportMinePoint;

    [Header("Data:")]
    public Vector2 playerPosition;
    public List<int> lastChains;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

    }
    private void Update()
    {
        playerPosition = player.transform.position;
    }
    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.loadPlayer();

        if (data != null)
        {
            Vector2 position = new Vector2();
            position.x = data.position[0];
            position.y = data.position[1];

            playerPosition = position;
            lastChains = data.lastChain;

            player.transform.position = playerPosition;
        }
        else
        {
            loadNew();
        }
    }

    public void loadNew()
    {
        playerPosition = spawnPlayer.position;

        lastChains = new List<int>();
        for (int i = 0; i < 200; i++)
        {
            lastChains.Add(0);
        }
        for (int i = 0; i < 3; i++)
        {
            lastChains[i] += 1;
        }


        player.transform.position = playerPosition;
    }
    public void loadRevive()
    {
        playerPosition = RevivePoint.position;

        player.transform.position = playerPosition;
    }
    public void loadFinish()
    {
        playerPosition = RevivePoint.position;
    }

    public void loadTeleport(int indexTeleport)
    {
        if (indexTeleport == 0)
        {
            playerPosition = teleportTopPoint.position;
            camera.setPositionDirect(playerPosition);
        }
        else if (indexTeleport == 1)
        {
            playerPosition = teleportMinePoint.position;
            camera.setPositionDirect(playerPosition);
        }
        player.transform.position = playerPosition;

    }

    public void savePlayer()
    {
        SaveSystem.savePlayer(instance);
    }
}
