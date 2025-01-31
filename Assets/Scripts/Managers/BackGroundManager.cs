using System.Collections.Generic;
using UnityEngine;

public class BackGroundManager : MonoBehaviour, IObserver
{
    [Header("BackgroundImages")]
    [SerializeField] List<GameObject> ImagePrefabs = new List<GameObject>();
    List<BackgroundImage> backgroundImages = new List<BackgroundImage>();

    [SerializeField] bool scrollLeft;
    [SerializeField] Subject gameManagerSubject;

    bool move = true;

    void Start()
    {
        SetUpImages();
    }

    void SetUpImages()
    {
        for (int i = 0; i < ImagePrefabs.Count; i++)
        {
            GameObject background = Instantiate(ImagePrefabs[i]);
            background.transform.SetParent(transform);
            BackgroundImage bgImage = background.GetComponent<BackgroundImage>();
            if (bgImage != null)
            {
                bgImage.SetScrollDirection(scrollLeft);
                backgroundImages.Add(bgImage);
            }
        }
    }

    public void OnNotify(Events @event, int value = 0)
    {
        if (@event == Events.Die)
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


    private void OnEnable()
    {
        gameManagerSubject.AddObserver(this);
    }
    private void OnDisable()
    {
        gameManagerSubject.RemoveObserver(this);
    }
}