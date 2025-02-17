using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class UIManager : MonoBehaviour, IObserver
{
    [SerializeField] private Subject gameManagerSubject;
    private LeaderboardManager leaderboardManager;

    [Header("LeaderBoard")]
    [SerializeField] List<TextMeshProUGUI> names;
    [SerializeField] List<TextMeshProUGUI> scores;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI scorePromptText;
    [SerializeField] TextMeshProUGUI enterPromptText;

    [Header("Overlays")]
    [SerializeField] GameObject inGameOverlay;
    [SerializeField] GameObject diedOverlay;
    [SerializeField] GameObject leaderBoardOverlay;
    [SerializeField] GameObject addLeaderboardOverlay;
    [SerializeField] GameObject enterPromptOverlay;

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
        enterPromptOverlay.SetActive(false);
        score = 0;
    }

    public void GameOverOverlay()
    {
        inGameOverlay.SetActive(false);
        enterPromptOverlay.SetActive(false);
        diedOverlay.SetActive(true);
        leaderBoardOverlay.SetActive(true);
        addLeaderboardOverlay.SetActive(true);
        scorePromptText.text = "Score   " + score.ToString();

    }

    public void OnNotify(PlayerActions action, int value)
    {
        if (action == PlayerActions.PassedPipe)
        {
            UpdateScore(value);
        }

        if (action == PlayerActions.Die)
        {
            GameOverOverlay();
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
        //List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry>();
        List<LeaderboardEntry> leaderboard = leaderboardManager.GetLeaderBoard();

        for (int i = 0; i < names.Count; i++)
        {
            names[i].text = "";
            scores[i].text = "";
        }

        for (int i = 0; i < leaderboard.Count; i++)
        {
            names[i].text = leaderboard[i].playerName;
            scores[i].text = leaderboard[i].score.ToString();
        }
    }

    void onEnterClicked()
    {
        string name = nameField.text;
        int length = name.Length;

        if (string.IsNullOrWhiteSpace(name))
        {
            StartCoroutine(ShowPromptForDuration(5f, "Please Enter a name."));
            return;
        }

        if (length > 5)
        {
            StartCoroutine(ShowPromptForDuration(5f, "Please enter a name with 5 or fewer characters."));
            return;
        }

        leaderboardManager.AddScore(name, score);
        UpdateLeaderboard();
        DataPersistanceManager.Instance.SaveLeaderboardData();
        addLeaderboardOverlay.SetActive(false);
    }

    IEnumerator ShowPromptForDuration(float duration, string prompt)
    {
        enterPromptOverlay.SetActive(true);
        enterPromptText.text = prompt;

        Image overlayImage = enterPromptOverlay.GetComponent<Image>();
        Color textColor = enterPromptText.color;
        Color overlayColor = overlayImage.color;

        textColor.a = 1f;
        overlayColor.a = 1f;
        enterPromptText.color = textColor;
        overlayImage.color = overlayColor;

        yield return new WaitForSeconds(duration);

        float fadeDuration = 1.5f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

            textColor.a = alpha;
            overlayColor.a = alpha;

            enterPromptText.color = textColor;
            overlayImage.color = overlayColor;

            yield return null;
        }

        enterPromptOverlay.SetActive(false);
    }

    void OnRetryClicked()
    {
        SceneLoader.Instance.LoadScene(1);
    }

    void OnMenuClicked()
    {
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