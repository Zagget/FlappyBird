using System.Collections.Generic;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour, IDataPersistance<LeaderboardData>
{
    private List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry>();

    public void AddScore(string playerName, int score)
    {
        leaderboard.Add(new LeaderboardEntry(playerName, score));
        leaderboard.Sort((a, b) => b.score.CompareTo(a.score));

        if (leaderboard.Count > 10)
        {
            Debug.Log("More than 10 entries removed last entry");
            leaderboard.RemoveAt(leaderboard.Count - 1);
        }
    }

    public List<LeaderboardEntry> GetLeaderBoard()
    {
        return leaderboard;
    }

    public void LoadData(LeaderboardData data)
    {
        leaderboard = new List<LeaderboardEntry>(data.LeaderBoardEntries);
    }

    public void SaveData(LeaderboardData data)
    {
        data.LeaderBoardEntries = new List<LeaderboardEntry>(leaderboard);
    }
}