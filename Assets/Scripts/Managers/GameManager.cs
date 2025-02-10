using System.Collections.Generic;
using UnityEngine;

public class GameManager : Subject, IObserver, IDataPersistance<PlayerData>
{
    [SerializeField] Subject playerSubject;
    [SerializeField] int currentScore;
    [SerializeField] int highScore;
    [SerializeField] int currentJump;

    [SerializeField] List<int> highScoreList = new List<int>();

    void Start()
    {
        ResetValues();
    }

    void ResetValues()
    {
        currentScore = 0;
        currentJump = 0;
    }

    public void OnNotify(Events @event, int value = 0)
    {
        if (@event == Events.PassedPipe)
        {
            currentScore++;
            NotifyObservers(Events.PassedPipe, currentScore);
        }
        if (@event == Events.Jump)
        {
            currentJump++;
            NotifyObservers(Events.Jump);
        }
        if (@event == Events.Die)
        {
            NotifyObservers(Events.Die, currentScore);
            DataPersistanceManager.Instance.SaveGame();
        }
    }


    public void LoadData(PlayerData data)
    {
        this.currentJump = data.totalJumps;
        this.currentScore = data.highScore;
    }

    public void SaveData(PlayerData data)
    {
        data.highScore = this.currentScore;
        data.totalJumps += this.currentJump;
    }

    void OnEnable()
    {
        playerSubject.AddObserver(this);
    }

    void OnDisable()
    {
        playerSubject.RemoveObserver(this);
    }
}