using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public PlayerData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        PlayerData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<PlayerData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error when loading data from file {fullPath} /n {e}");
            }
        }
        return loadedData;
    }

    public void Save(PlayerData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter write = new StreamWriter(stream))
                {
                    write.Write(dataToStore);
                }
            }
            Debug.Log("Saving data");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error when trying to save data to file {fullPath} +/n {e}");
        }
    }
}
