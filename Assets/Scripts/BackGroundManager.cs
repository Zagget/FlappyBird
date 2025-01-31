using System.Collections.Generic;
using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    [SerializeField] List<BackgroundImage> imageList = new List<BackgroundImage>();


    void Start()
    {
        // for (int i = 0; i < imageList.Count; i++)
        // {
        //     imageList[i].GetComponent<BackgroundImage>();
        // }
    }

    void Update()
    {
        // Update all background images in the list
        for (int i = 0; i < imageList.Count; i++)
        {
            imageList[i].Scroll();
            imageList[i].CheckReset();
        }
    }
}