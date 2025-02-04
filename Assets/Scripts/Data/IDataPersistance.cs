using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistance
{
    void LoadData(PlayerData data);
    void SaveData(PlayerData data);
}