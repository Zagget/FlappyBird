using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIManager : MonoBehaviour, IObserver
{
    [SerializeField] Subject playerSubject;
    [SerializeField] int passedPipe;
    [SerializeField] TextMeshPro passedPipeText;


    public void OnNotify(PlayerAction action)
    {
        if (action == PlayerAction.PassedPipe)
        {
            Debug.Log("Passed a pipe");
            passedPipe++;
            UpdateText(passedPipeText);
        }
    }

    void UpdateText(TextMeshPro text)
    {
        passedPipeText.text = passedPipe.ToString();
    }

    void OnEnable()
    {
        playerSubject.AddObserver(this);
    }
    void OnDisable()
    {
        playerSubject.RemoveObserver(this);
    }
}