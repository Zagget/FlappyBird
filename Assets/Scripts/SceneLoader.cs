using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader instance;
    public static SceneLoader Instance { get { return instance; } }


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        // If running in the Unity editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // If running as a standalone build
            Application.Quit();
#endif
    }
}
