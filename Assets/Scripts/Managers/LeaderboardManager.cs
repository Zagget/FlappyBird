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

        Debug.Log("Current Leaderboard Data: \n----------------");
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
        leaderboard = new List<LeaderboardEntry>(data.LeaderBoardEntries);
        Debug.Log($"Loading Data LeaderboardManager data entries count: {data.LeaderBoardEntries.Count} leaderboard entries count:{leaderboard.Count}");

    }

    public void SaveData(LeaderboardData data)
    {
        data.LeaderBoardEntries = new List<LeaderboardEntry>(leaderboard);
        Debug.Log($"Saving Data LeaderboardManager entries count: {data.LeaderBoardEntries.Count}");
    }
}