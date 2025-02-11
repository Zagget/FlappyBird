[System.Serializable]
public class PlayerData
{
    public string name;
    public int highScore;
    public int totalJumps;
    public int distance;

    public PlayerData()
    {
        name = "name";
        highScore = 0;
        totalJumps = 0;
        distance = 0;
    }
}