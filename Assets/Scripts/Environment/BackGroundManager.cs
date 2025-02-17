using System.Collections.Generic;
using UnityEngine;

public class BackGroundManager : MonoBehaviour, IObserver
{
    [Header("BackgroundImages")]
    [SerializeField] private List<GameObject> ImagePrefabs = new List<GameObject>();

    [SerializeField] private bool scrollLeft;
    [SerializeField] private bool move = true;

    [SerializeField] private Subject gameManagerSubject;
    private List<BackgroundImage> backgroundImages = new List<BackgroundImage>();

    void Start()
    {
        SetUpImages();
    }

    void SetUpImages()
    {
        backgroundImages.Clear();
        for (int i = 0; i < ImagePrefabs.Count; i++)
        {
            GameObject background = Instantiate(ImagePrefabs[i]);
            background.transform.SetParent(transform);

            BackgroundImage bgImage = background.GetComponent<BackgroundImage>();
            if (bgImage == null)
            {
                Debug.LogError($"bgImage is null in imageprefabs, ListNumber: {i}");
                continue;
            }

            bgImage.SetScrollDirection(scrollLeft);
            backgroundImages.Add(bgImage);
        }
    }

    public void OnNotify(PlayerActions @event, int value = 0)
    {
        if (@event == PlayerActions.Die)
        {
            move = false;
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
        foreach (var backgroundImage in backgroundImages)
        {
            backgroundImage.Scroll();
            backgroundImage.CheckReset();
        }
    }

    private void ChangeBackground()
    {

    }

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