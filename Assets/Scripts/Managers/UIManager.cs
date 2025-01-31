using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : Subject, IObserver
{
    [SerializeField] Subject gameManagerSubject;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI scoreText;

    [Header("Overlays")]
    [SerializeField] GameObject InGameOverlay;
    [SerializeField] GameObject diedOverlay;

    [Header("Buttons")]
    [SerializeField] Button retry;
    [SerializeField] Button menu;

    int highScore;

    void Start()
    {
        SetOverlay(diedOverlay, false);
        SetOverlay(InGameOverlay, true);
    }

    public void OnNotify(Events @event, int value)
    {
        Debug.Log($"UIManager received event: {@event} with value: {value}");
        if (@event == Events.PassedPipe)
        {
            UpdateScore(value);
        }

        if (@event == Events.Die)
        {
            SetOverlay(InGameOverlay, true);
        }

        if (@event == Events.NewHighScore)
        {

        }
    }



    void UpdateScore(int value)
    {

        scoreText.text = value.ToString();
    }

    void SetOverlay(GameObject overlay, bool show)
    {
        overlay.SetActive(show);
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