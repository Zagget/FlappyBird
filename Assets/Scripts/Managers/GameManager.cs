using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : Subject, IObserver
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
            Debug.Log($"GameManager - Updating score: {currentScore}");
            NotifyObservers(Events.PassedPipe, currentScore);
        }
        if (@event == Events.Jump)
        {
            currentJump++;
            NotifyObservers(Events.Jump);
        }
        if (@event == Events.Die)
        {
            NotifyObservers(Events.Die);
            CheckScore();
        }
    }

    void CheckScore()
    {
        for (int i = 0; i < highScoreList.Count; i++)
        {
            if (highScoreList[i] < currentScore)
            {
                highScoreList.Add(currentScore);
            }
        }
    }

    void Save()
    {
        PlayerSaveData saveData = new PlayerSaveData();
        saveData.Name = "player";
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