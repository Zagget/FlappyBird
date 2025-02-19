using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath;
    private string dataFileName;

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public void Save<T>(T data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string jsonData = JsonUtility.ToJson(data, true);
            File.WriteAllText(fullPath, jsonData);
            Debug.Log($"Saved data  to {fullPath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving data to {fullPath}: {e}");
        }
    }

    public T Load<T>()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        if (File.Exists(fullPath))
        {
            try
            {
                string jsonData = File.ReadAllText(fullPath);
                Debug.Log($"Loaded file from {fullPath}");
                T result = JsonUtility.FromJson<T>(jsonData);
                if (result == null)
                {
                    Debug.LogError($"Failed to parse the JSON data into {typeof(T)} from file: {fullPath}");
                }
                return result;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading data from {fullPath}: {e}");
            }
        }
        return default;
    }
}