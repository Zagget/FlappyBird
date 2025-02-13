using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Name")]
    [SerializeField] private string playerFileName;
    [SerializeField] private string leaderboardFileName;

    private PlayerData playerData;
    private LeaderboardData leaderboardData;

    private List<IDataPersistance<PlayerData>> playerDataObjects;
    private List<IDataPersistance<LeaderboardData>> leaderboarddataObjects;
    private FileDataHandler playerDataHandler;
    private FileDataHandler leaderboardDataHandler;
    private static DataPersistanceManager instance;
    public static DataPersistanceManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateAndLoad()
    {
        this.playerDataHandler = new FileDataHandler(Application.persistentDataPath, playerFileName);
        this.leaderboardDataHandler = new FileDataHandler(Application.persistentDataPath, leaderboardFileName);

        this.playerDataObjects = FindAllPlayerDataObjects();
        this.leaderboarddataObjects = FindAllLeaderBoardDataObjects();

        LoadPlayerData();
        LoadLeaderboardData();
    }

    public void NewPlayerData()
    {
        this.playerData = new PlayerData();
    }
    public void NewLeaderBoardData()
    {
        this.leaderboardData = new LeaderboardData();
    }

    public void LoadPlayerData()
    {
        this.playerData = playerDataHandler.Load<PlayerData>();
        if (this.playerData == null)
        {
            Debug.Log("No player data found, creating new");
            NewPlayerData();
        }

        foreach (var obj in playerDataObjects)
        {
            obj.LoadData(playerData);
        }
        Debug.Log("Player data loaded");
    }

    public void LoadLeaderboardData()
    {
        this.leaderboardData = leaderboardDataHandler.Load<LeaderboardData>();
        if (this.leaderboardData == null)
        {
            Debug.Log("No leaderboard data found, creating new");
            NewLeaderBoardData();
        }

        foreach (var obj in leaderboarddataObjects)
        {
            obj.LoadData(leaderboardData);
        }
        Debug.Log("Leaderboard data loaded");
    }

    public void SavePlayerData()
    {
        try
        {
            foreach (var obj in playerDataObjects)
            {
                obj.SaveData(playerData);
            }
            playerDataHandler.Save(playerData);
            Debug.Log("Player data saved successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving player data: {e.Message}");
        }
    }

    public void SaveLeaderboardData()
    {
        try
        {
            foreach (var obj in leaderboarddataObjects)
            {
                obj.SaveData(leaderboardData);
            }
            leaderboardDataHandler.Save(leaderboardData);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving leaderboard data: {e.Message}");
        }
    }

    private List<IDataPersistance<PlayerData>> FindAllPlayerDataObjects()
    {
        IEnumerable<IDataPersistance<PlayerData>> dataPersistancesObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance<PlayerData>>();

        return new List<IDataPersistance<PlayerData>>(dataPersistancesObjects);
    }

    private List<IDataPersistance<LeaderboardData>> FindAllLeaderBoardDataObjects()
    {
        IEnumerable<IDataPersistance<LeaderboardData>> dataPersistancesObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance<LeaderboardData>>();

        return new List<IDataPersistance<LeaderboardData>>(dataPersistancesObjects);
    }

}