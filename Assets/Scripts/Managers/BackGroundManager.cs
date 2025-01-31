using System.Collections.Generic;
using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    [SerializeField] List<GameObject> ImagePrefabs = new List<GameObject>();
    List<BackgroundImage> backgroundImages = new List<BackgroundImage>();

    [SerializeField] bool scrollLeft;

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

    void Update()
    {
        // Iterate through all background images and update them
        foreach (var backgroundImage in backgroundImages)
        {
            backgroundImage.Scroll();
            backgroundImage.CheckReset();
        }
    }
}