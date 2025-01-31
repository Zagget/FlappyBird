using UnityEngine;

public class PlayerAudio : MonoBehaviour, IObserver
{
    [SerializeField] Subject gameManagerSubject;
    [SerializeField] SoundData DeathSounds;
    [SerializeField] SoundData FlapSounds;
    [SerializeField] SoundData PassedPipeSounds;


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
