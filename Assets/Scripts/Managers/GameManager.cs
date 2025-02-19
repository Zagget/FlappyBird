using UnityEngine;

public class GameManager : Subject, IObserver, IDataPersistance<PlayerData>
{
    [SerializeField] private Subject playerSubject;
    [SerializeField] private int level2;
    [SerializeField] private int level3;
    private int currentScore;
    private int currentJump;
    private int distance;

    private void Start()
    {
        ResetValues();
        Debug.Log("GM Start: Setup files");
        DataPersistanceManager.Instance.UpdateAndLoad();
    }

    private void ResetValues()
    {
        currentScore = 0;
        currentJump = 0;
        distance = 0;
    }

    public void OnNotify(Events action, int value = 0)
    {
        if (action == Events.PassedPipe)
        {
            currentScore++;
            distance += 5;
            NotifyObservers(Events.PassedPipe, currentScore);

            if (currentScore == level2) NotifyObservers(Events.Level2);
            if (currentScore == level3) NotifyObservers(Events.Level3);
        }
        if (action == Events.Jump)
        {
            currentJump++;
            NotifyObservers(Events.Jump);
        }
        if (action == Events.Die)
        {
            NotifyObservers(Events.Die, currentScore);
            DataPersistanceManager.Instance.SavePlayerData();
        }
    }

    public void LoadData(PlayerData data)
    {

    }

    public void SaveData(PlayerData data)
    {
        data.totalPassedPipes += this.currentScore;
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