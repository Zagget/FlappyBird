using System.Collections.Generic;
using UnityEngine;

public class GameManager : Subject, IObserver, IDataPersistance<PlayerData>
{
    [SerializeField] private Subject playerSubject;
    [SerializeField] private int currentScore;
    [SerializeField] private int highScore;
    [SerializeField] private int currentJump;
    [SerializeField] private int distance;

    [SerializeField] private List<int> highScoreList = new List<int>();

    private void Start()
    {
        ResetValues();
    }

    private void ResetValues()
    {
        currentScore = 0;
        currentJump = 0;
        distance = 0;
    }

    public void OnNotify(Events @event, int value = 0)
    {
        if (@event == Events.PassedPipe)
        {
            currentScore++;
            distance += 5;
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
            DataPersistanceManager.Instance.SavePlayerData();
        }
    }

    public void LoadData(PlayerData data)
    {
        this.currentJump = data.totalJumps;
        this.currentScore = data.highScore;
        this.distance = data.distance;
    }

    public void SaveData(PlayerData data)
    {
        data.highScore = this.currentScore;
        data.totalJumps += this.currentJump;
        data.distance += this.distance;
    }

    private void OnEnable()
    {
        playerSubject.AddObserver(this);
    }

    private void OnDisable()
    {
        playerSubject.RemoveObserver(this);
    }
}