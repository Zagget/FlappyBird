using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.PlasticSCM.Editor.WebApi;
using System.Collections.Generic;

public class UIManager : MonoBehaviour, IObserver
{
    [SerializeField] private Subject gameManagerSubject;
    private LeaderboardManager leaderboardManager;

    [Header("LeaderBoard")]
    [SerializeField] List<TextMeshProUGUI> names;
    [SerializeField] List<TextMeshProUGUI> scores;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI leaderboardText;

    [Header("Overlays")]
    [SerializeField] GameObject inGameOverlay;
    [SerializeField] GameObject diedOverlay;
    [SerializeField] GameObject leaderBoardOverlay;
    [SerializeField] GameObject AddleaderboardOverlay;

    [Header("Buttons")]
    [SerializeField] Button retry;
    [SerializeField] Button menu;
    [SerializeField] Button enter;
    [Header("InputFields")]
    [SerializeField] TMP_InputField nameField;

    private int score;

    void Start()
    {
        leaderboardManager = FindAnyObjectByType<LeaderboardManager>();

        InGameOverlay();

        retry.onClick.AddListener(OnRetryClicked);
        menu.onClick.AddListener(OnMenuClicked);
        enter.onClick.AddListener(onEnterClicked);
    }

    public void InGameOverlay()
    {
        diedOverlay.SetActive(false);
        leaderBoardOverlay.SetActive(false);
        inGameOverlay.SetActive(true);
    }

    public void GameOverOverlay()
    {
        inGameOverlay.SetActive(false);
        diedOverlay.SetActive(true);
        leaderBoardOverlay.SetActive(true);
        AddleaderboardOverlay.SetActive(true);
    }

    public void OnNotify(Events @event, int value)
    {
        if (@event == Events.PassedPipe)
        {
            UpdateScore(value);
        }

        if (@event == Events.Die)
        {
            GameOverOverlay();
            score = value;
            UpdateLeaderboard();
        }
    }

    void UpdateScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
    }

    void UpdateLeaderboard()
    {
        List<LeaderboardEntry> leaderboard = leaderboardManager.GetLeaderBoard();

        for (int i = 0; i < names.Count; i++)
        {
            names[i].text = "";
            scores[i].text = "";
        }

        for (int i = 0; i < leaderboard.Count; i++)
        {
            if (leaderboard[i].playerName == null)
            {
                continue;
            }
            names[i].text = leaderboard[i].playerName;
            scores[i].text = leaderboard[i].score.ToString();
        }
    }

    void onEnterClicked()
    {
        string name = nameField.text;
        leaderboardManager.AddScore(name, score);
        UpdateLeaderboard();
        DataPersistanceManager.Instance.SaveLeaderboardData();
        AddleaderboardOverlay.SetActive(false);
    }


    void OnRetryClicked()
    {
        Debug.Log("Loading main game");
        SceneLoader.Instance.LoadScene(1);
    }

    void OnMenuClicked()
    {
        Debug.Log("Loading menu");
        SceneLoader.Instance.LoadScene(0);
    }

    void OnEnable()
    {
        gameManagerSubject.AddObserver(this);
    }
    void OnDisable()
    {
        gameManagerSubject.RemoveObserver(this);
    }
}