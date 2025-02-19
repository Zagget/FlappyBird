using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BackGroundManager : MonoBehaviour, IObserver
{
    [Header("BackgroundPrefabs")]
    [SerializeField] private List<GameObject> levelOneImagePrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> levelTwoImagePrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> levelThreeImagePrefabs = new List<GameObject>();

    [Header("Background config")]
    [SerializeField] private bool scrollLeft;
    [SerializeField] private bool move = true;
    [SerializeField] private float fadeDuration;

    [SerializeField] private Subject gameManagerSubject;

    private List<GameObject> currentBackgrounds = new List<GameObject>();
    private List<ImageMovement> imageMovements = new List<ImageMovement>();

    private void Start()
    {
        SetUpBackground(levelOneImagePrefabs);
    }

    private void SetUpBackground(List<GameObject> bgPrefabs)
    {
        ClearBackground();

        for (int i = 0; i < bgPrefabs.Count; i++)
        {
            GameObject spawnedBackground = Instantiate(bgPrefabs[i]);
            spawnedBackground.transform.SetParent(transform);

            ImageMovement imageMov = spawnedBackground.GetComponent<ImageMovement>();
            if (imageMov == null)
            {
                Debug.LogError($"imageMov is null in {bgPrefabs}, ListNumber: {i}");
                continue;
            }

            imageMov.SetScrollDirection(scrollLeft);
            imageMovements.Add(imageMov);
            currentBackgrounds.Add(spawnedBackground);
        }
    }

    private void ClearBackground()
    {
        if (currentBackgrounds.Count > 0)
        {
            foreach (var obj in currentBackgrounds)
            {
                Destroy(obj);
            }
            foreach (var obj in imageMovements)
            {
                Destroy(obj);
            }

            currentBackgrounds.Clear();
            imageMovements.Clear();
        }
    }

    public void OnNotify(Events @event, int value = 0)
    {
        if (@event == Events.Die)
        {
            move = false;
        }

        if (@event == Events.Level2)
        {
            ChangeBackground(currentBackgrounds, levelTwoImagePrefabs);
        }

        if (@event == Events.Level3)
        {
            ChangeBackground(currentBackgrounds, levelThreeImagePrefabs);
        }
    }

    void Update()
    {
        if (move)
        {
            MoveBackground();
        }
    }

    private void MoveBackground()
    {
        foreach (var mov in imageMovements)
        {
            mov.Scroll();
            mov.CheckReset();
        }
    }

    private void ChangeBackground(List<GameObject> currentBackground, List<GameObject> toBackground)
    {
        StartCoroutine(ChangeBackgroundRoutine(currentBackground, toBackground));
    }

    private IEnumerator ChangeBackgroundRoutine(List<GameObject> currentBackground, List<GameObject> toBackground)
    {
        foreach (var obj in currentBackground)
        {
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            if (sr == null)
            {
                Debug.LogError("SpriteRender null");
                continue;
            }

            StartCoroutine(Fade.FadeInOrOut(sr, fadeDuration, 1, 0));
        }
        yield return new WaitForSeconds(fadeDuration);

        SetUpBackground(toBackground);

        foreach (var obj in currentBackgrounds)
        {
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            if (sr == null)
            {
                Debug.LogError("SpriteRender null");
                continue;
            }
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
            StartCoroutine(Fade.FadeInOrOut(sr, fadeDuration, 0, 1));
        }
        //yield return new WaitForSeconds(fadeDuration);
    }

    // private IEnumerator Fade(SpriteRenderer sr, float startAlpha, float endAlpha)
    // {
    //     sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, startAlpha);

    //     float elapsedTime = 0f;
    //     while (elapsedTime < fadeDuration)
    //     {
    //         elapsedTime += Time.deltaTime;
    //         float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);

    //         sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

    //         yield return null;
    //     }
    //     sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, endAlpha);
    // }

    private void OnEnable()
    {
        if (gameManagerSubject == null) return;

        gameManagerSubject.AddObserver(this);
    }

    private void OnDisable()
    {
        if (gameManagerSubject == null) return;

        gameManagerSubject.RemoveObserver(this);
    }
}