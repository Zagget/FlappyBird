using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour, IDataPersistance<PlayerData>
{
    [SerializeField] private GameObject statsOverlay;
    [SerializeField] private TextMeshProUGUI statsText;


    private int jumps;
    private int distance;
    private int passedPipes;

    private bool showStats = false;

    private void Start()
    {
        statsOverlay.SetActive(showStats);
        DataPersistanceManager.Instance.UpdateAndLoad();
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
        // Nothing to save here
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