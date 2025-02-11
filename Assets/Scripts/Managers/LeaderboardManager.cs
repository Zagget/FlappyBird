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
        Debug.Log($"Added Score for {playerName}, score: {score}");

        Debug.Log("Current Leaderboard Data:");
        for (int i = 0; i < leaderboard.Count; i++)
        {
            Debug.Log($"Name: {leaderboard[i].playerName} Score: {leaderboard[i].score.ToString()}");
        }
    }

    public List<LeaderboardEntry> GetLeaderBoard()
    {
        return leaderboard;
    }

    public void LoadData(LeaderboardData data)
    {
        Debug.Log("Loading leaderboard data");
        data.LeaderBoardEntries = this.leaderboard;
    }

    public void SaveData(LeaderboardData data)
    {
        Debug.Log("Saving leaderboard data");
        this.leaderboard = data.LeaderBoardEntries;
    }
}