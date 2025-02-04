using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Name")]
    [SerializeField] private string fileName;

    private PlayerData data;
    private List<IDataPersistance> dataPersistancesObjects;
    private FileDataHandler dataHandler;

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
        this.dataPersistancesObjects = FindAllDataPersistanceObjects();

        LoadGame();
    }

    public void NewGame()
    {
        this.data = new PlayerData();
    }

    public void LoadGame()
    {
        this.data = dataHandler.Load();

        if (this.data == null)
        {
            Debug.Log("No data found, creating new");
            NewGame();
        }

        foreach (var obj in dataPersistancesObjects)
        {
            obj.LoadData(data);
        }
    }

    public void SaveGame()
    {
        foreach (var obj in dataPersistancesObjects)
        {
            obj.SaveData(data);
        }
        dataHandler.Save(data);
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistancesObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistancesObjects);
    }
}