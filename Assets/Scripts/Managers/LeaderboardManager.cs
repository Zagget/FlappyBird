using System.Collections.Generic;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour, IObserver, IDataPersistance<LeaderboardData>
{
    [SerializeField] private Subject gameManagerSubject;
    private List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry>();

    public void OnNotify(Events @event, int value = 0)
    {
        if (@event == Events.Die)
        {

        }
    }

    public void AddScore(string playerName, int score)
    {
        leaderboard.Add(new LeaderboardEntry(playerName, score));
        leaderboard.Sort((a, b) => b.score.CompareTo(a.score));

        if (leaderboard.Count > 10)
        {
            leaderboard.RemoveAt(leaderboard.Count - 1);
        }
    }

    public void LoadData(LeaderboardData data)
    {

    }

    public void SaveData(LeaderboardData data)
    {

    }

    private void OnEnable()
    {
        gameManagerSubject.AddObserver(this);
    }

    private void OnDisable()
    {
        gameManagerSubject.RemoveObserver(this);
    }
}