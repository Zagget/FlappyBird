[System.Serializable]
public class PlayerData
{
    public int totalPassedPipes;
    public int totalJumps;
    public int distance;

    public PlayerData()
    {
        totalPassedPipes = 0;
        totalJumps = 0;
        distance = 0;
    }
}