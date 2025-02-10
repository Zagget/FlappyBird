using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : Subject, IObserver
{
    [SerializeField] Subject gameManagerSubject;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI noLeaderboardText;

    [Header("Overlays")]
    [SerializeField] GameObject inGameOverlay;
    [SerializeField] GameObject diedOverlay;

    [Header("Buttons")]
    [SerializeField] Button retry;
    [SerializeField] Button menu;
    [SerializeField] Button enter;

    void Start()
    {
        InGameOverlay();

        retry.onClick.AddListener(OnRetryClicked);
        menu.onClick.AddListener(OnMenuClicked);
    }

    public void InGameOverlay()
    {
        diedOverlay.SetActive(false);
        inGameOverlay.SetActive(true);
    }

    public void GameOverOverlay()
    {
        inGameOverlay.SetActive(false);
        diedOverlay.SetActive(true);
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
            ShowHighScores();

        }
    }

    void ShowHighScores()
    {

    }

    void UpdateScore(int value)
    {
        scoreText.text = value.ToString();
    }

    void OnRetryClicked()
    {
        Debug.Log("Retry button clicked!");
        SceneLoader.Instance.LoadScene(1);
    }

    void OnMenuClicked()
    {
        Debug.Log("Menu clicked");
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