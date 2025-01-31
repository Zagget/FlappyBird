using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : Subject, IObserver
{
    [SerializeField] Subject gameManagerSubject;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject InGameOverlay;
    [SerializeField] GameObject diedOverlay;

    int highScore;

    void Start()
    {
        diedOverlay.SetActive(false);
        InGameOverlay.SetActive(true);
    }

    public void OnNotify(Events action, int value)
    {
        Debug.Log($"UIManager received event: {action} with value: {value}");
        if (action == Events.PassedPipe)
        {
            UpdateScore(value);
        }

        if (action == Events.Die)
        {

        }

        if (action == Events.NewHighScore)
        {

        }
    }

    void UpdateScore(int value)
    {

        scoreText.text = value.ToString();
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