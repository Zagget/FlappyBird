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
    [SerializeField] private List<TextMeshProUGUI> names;
    [SerializeField] private List<TextMeshProUGUI> scores;

    [Header("Overlays")]
    [SerializeField] private GameObject inGameOverlay;
    [SerializeField] private GameObject diedOverlay;
    [SerializeField] private GameObject leaderBoardOverlay;
    [SerializeField] private GameObject addLeaderboardOverlay;
    [SerializeField] private GameObject promptOverlay;

    [Header("Buttons")]
    [SerializeField] private Button retry;
    [SerializeField] private Button menu;
    [SerializeField] private Button enter;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scorePromptText;
    [SerializeField] private TextMeshProUGUI promptText;

    [Header("InputFields")]
    [SerializeField] private TMP_InputField nameField;

    [Header("Prompt Config")]
    [SerializeField] private float durationBeforeFade = 5f;
    [SerializeField] private float fadeDuration = 1.5f;

    private struct PromptUI
    {
        public Image img;
        public TextMeshProUGUI text;
    }
    private PromptUI prompt;

    Coroutine ShowPrompt;
    private int score;

    void Start()
    {
        leaderboardManager = FindAnyObjectByType<LeaderboardManager>();
        if (leaderboardManager == null) Debug.LogError("leaderboardManager is null");

        prompt = new PromptUI { img = promptOverlay.GetComponent<Image>(), text = promptText };
        if (prompt.img == null) Debug.LogError("Prompt image is null");

        InGameOverlay();
        retry.onClick.AddListener(OnRetryClicked);
        menu.onClick.AddListener(OnMenuClicked);
        enter.onClick.AddListener(OnEnterClicked);
    }

    public void InGameOverlay()
    {
        SetOverlays(true, false, false, false);
        score = 0;
    }

    public void GameOverOverlay()
    {
        SetOverlays(false, true, true, true);
        scorePromptText.text = $"Score {score}";
    }

    private void SetOverlays(bool inGame, bool died, bool leaderboard, bool addLeaderboard)
    {
        inGameOverlay.SetActive(inGame);
        diedOverlay.SetActive(died);
        leaderBoardOverlay.SetActive(leaderboard);
        addLeaderboardOverlay.SetActive(addLeaderboard);
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

        for (int i = 0; i < leaderboard.Count && i < names.Count; i++)
        {
            names[i].text = leaderboard[i].playerName;
            scores[i].text = leaderboard[i].score.ToString();
        }
    }

    void OnEnterClicked()
    {
        string playerName = nameField.text.Trim();

        if (string.IsNullOrWhiteSpace(playerName))
        {
            ShowPromptMessage("Please enter a name.");
            return;
        }

        if (playerName.Length > 5)
        {
            ShowPromptMessage("Name must be 5 characters or fewer.");
            return;
        }

        leaderboardManager.AddScore(playerName, score);
        UpdateLeaderboard();
        DataPersistanceManager.Instance.SaveLeaderboardData();
        addLeaderboardOverlay.SetActive(false);
    }

    void ShowPromptMessage(string msg)
    {
        if (ShowPrompt != null)
        {
            StopCoroutine(ShowPrompt);
        }

        ShowPrompt = StartCoroutine(ShowPromptForDuration(durationBeforeFade, msg));
    }


    IEnumerator ShowPromptForDuration(float duration, string msg)
    {
        promptText.text = msg;
        SetPromptUIOpacitiy(prompt, 1f);

        yield return new WaitForSeconds(duration);

        yield return StartCoroutine(FadeOutPromptUI(prompt, fadeDuration));
    }

    private void SetPromptUIOpacitiy(PromptUI ui, float alpha)
    {
        Color imgColor = ui.img.color;
        imgColor.a = alpha;
        ui.img.color = imgColor;

        Color textColor = ui.text.color;
        textColor.a = alpha;
        ui.text.color = textColor;
    }


    private IEnumerator FadeOutPromptUI(PromptUI ui, float fadeDuration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            SetPromptUIOpacitiy(prompt, alpha);
            yield return null;
        }
    }

    void OnRetryClicked() => SceneLoader.Instance.LoadScene(1);
    void OnMenuClicked() => SceneLoader.Instance.LoadScene(0);
    void OnEnable() => gameManagerSubject.AddObserver(this);
    void OnDisable() => gameManagerSubject.RemoveObserver(this);
}