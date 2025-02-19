using UnityEngine;

public class PlayerAudio : MonoBehaviour, IObserver
{
    [SerializeField] Subject gameManagerSubject;

    [SerializeField] SoundData BackgroundMusic;
    [SerializeField] SoundData DeathSounds;
    [SerializeField] SoundData FlapSounds;
    [SerializeField] SoundData PassedPipeSounds;
    [SerializeField] SoundData NewLevelSounds;

    private void Start()
    {
        SoundManager.Instance.PlayRandomSound(BackgroundMusic);
    }

    public void OnNotify(Events action, int value = 0)
    {
        if (action == Events.Die)
        {
            SoundManager.Instance.PlayRandomSound(DeathSounds);
        }

        if (action == Events.Jump)
        {
            SoundManager.Instance.PlayRandomSound(FlapSounds);
        }

        if (action == Events.PassedPipe)
        {
            SoundManager.Instance.PlayRandomSound(PassedPipeSounds);
        }

        if (action == Events.Level2 || action == Events.Level3)
        {
            SoundManager.Instance.PlayRandomSound(NewLevelSounds);
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
