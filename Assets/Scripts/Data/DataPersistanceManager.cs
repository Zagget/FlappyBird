using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Name")]
    [SerializeField] private string fileName;

    private PlayerData playerData;
    private FileDataHandler dataHandler;
    private List<IDataPersistance<PlayerData>> dataPersistancesObjects;

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
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistancesObjects = FindAllPlayerDataPersistanceObjects();

        LoadGame();
    }

    public void NewGame()
    {
        this.playerData = new PlayerData();
    }

    public void LoadGame()
    {
        this.playerData = dataHandler.Load<PlayerData>();

        if (this.playerData == null)
        {
            Debug.Log("No data found, creating new");
            NewGame();
        }

        foreach (var obj in dataPersistancesObjects)
        {
            obj.LoadData(playerData);
        }
    }

    public void SaveGame()
    {
        foreach (var obj in dataPersistancesObjects)
        {
            obj.SaveData(playerData);
        }
        dataHandler.Save(playerData);
    }

    private List<IDataPersistance<PlayerData>> FindAllPlayerDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance<PlayerData>> dataPersistancesObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance<PlayerData>>();

        return new List<IDataPersistance<PlayerData>>(dataPersistancesObjects);
    }
}