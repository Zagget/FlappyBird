[System.Serializable]
public class PlayerData
{
    public string Name;
    public int highScore;
    public int totalJumps;

    public PlayerData()
    {
        Name = "name";
        highScore = 0;
        totalJumps = 0;
    }
}