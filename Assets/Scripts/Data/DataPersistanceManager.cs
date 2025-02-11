using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Name")]
    [SerializeField] private string playerfileName;
    [SerializeField] private string leaderboardfileName;

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
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        this.playerDataHandler = new FileDataHandler(Application.persistentDataPath, playerfileName);
        this.leaderboardDataHandler = new FileDataHandler(Application.persistentDataPath, leaderboardfileName);

        this.playerDataObjects = FindAllPlayerDataPersistanceObjects();
        this.leaderboarddataObjects = FindAllLeaderBoardDataPersistanceObjects();

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
    }

    public void SavePlayerData()
    {
        foreach (var obj in playerDataObjects)
        {
            obj.SaveData(playerData);
        }
        playerDataHandler.Save(playerData);
    }

    public void SaveLeaderboardData()
    {
        foreach (var obj in leaderboarddataObjects)
        {
            obj.SaveData(leaderboardData);
        }
        leaderboardDataHandler.Save(leaderboardData);
    }

    private List<IDataPersistance<PlayerData>> FindAllPlayerDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance<PlayerData>> dataPersistancesObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance<PlayerData>>();

        return new List<IDataPersistance<PlayerData>>(dataPersistancesObjects);
    }

    private List<IDataPersistance<LeaderboardData>> FindAllLeaderBoardDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance<LeaderboardData>> dataPersistancesObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance<LeaderboardData>>();

        return new List<IDataPersistance<LeaderboardData>>(dataPersistancesObjects);
    }
}