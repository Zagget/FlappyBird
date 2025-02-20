using System.Collections.Generic;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour, IDataPersistance<LeaderboardData>
{
    private LeaderboardData leaderboardData = new LeaderboardData();

    public void AddScore(string playerName, int score)
    {
        leaderboardData.LeaderBoardEntries.Add(new LeaderboardEntry(playerName, score));
        leaderboardData.LeaderBoardEntries.Sort((a, b) => b.score.CompareTo(a.score));

        if (leaderboardData.LeaderBoardEntries.Count > 10)
        {
            Debug.Log("More than 10 entries removed last entry");
            leaderboardData.LeaderBoardEntries.RemoveAt(leaderboardData.LeaderBoardEntries.Count - 1);
        }
    }

    public List<LeaderboardEntry> GetLeaderBoard()
    {
        return leaderboardData.LeaderBoardEntries;
    }

    public void LoadData(LeaderboardData data)
    {
        leaderboardData = data;
    }

    public void SaveData(LeaderboardData data)
    {
        data.LeaderBoardEntries = new List<LeaderboardEntry>(leaderboardData.LeaderBoardEntries);
    }
}