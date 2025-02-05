using System.Collections.Generic;

[System.Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntry> LeaderBoardEntries;

    public LeaderboardData()
    {
        LeaderBoardEntries = new List<LeaderboardEntry>();
    }
}

[System.Serializable]
public class LeaderboardEntry
{
    public string playerName;
    public int score;

    public LeaderboardEntry(string playerName, int score)
    {
        this.playerName = playerName;
        this.score = score;
    }
}