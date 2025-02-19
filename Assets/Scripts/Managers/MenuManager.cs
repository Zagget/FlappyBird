using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour, IDataPersistance<PlayerData>
{
    [SerializeField] private GameObject statsOverlay;
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private SoundData MenuMusic;


    private int jumps;
    private int distance;
    private int passedPipes;

    private bool showStats = false;

    private void Start()
    {
        SoundManager.Instance.PlayBackgroundMusic(MenuMusic);
        statsOverlay.SetActive(showStats);
        DataPersistanceManager.Instance.UpdateAndLoad();
    }

    public void Play()
    {
        SceneLoader.Instance.LoadScene(1);
    }

    public void Stats()
    {
        showStats = !showStats;

        statsOverlay.SetActive(showStats);
        statsText.text = $"Total Jumps:\n{jumps} \nTotal Distance:\n{distance} meter\nPassed Pipes:\n{passedPipes}";
    }

    public void LoadData(PlayerData data)
    {
        jumps = data.totalJumps;
        passedPipes = data.totalPassedPipes;
        distance = data.distance;
    }

    public void SaveData(PlayerData data)
    {
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}