using UnityEngine;

public class PlayerAudio : MonoBehaviour, IObserver
{
    [SerializeField] Subject gameManagerSubject;
    [SerializeField] SoundData DeathSounds;
    [SerializeField] SoundData FlapSounds;
    [SerializeField] SoundData PassedPipeSounds;


    public void OnNotify(PlayerActions action, int value = 0)
    {
        if (action == PlayerActions.Die)
        {
            SoundManager.Instance.PlayRandomSound(DeathSounds);
        }

        if (action == PlayerActions.Jump)
        {
            SoundManager.Instance.PlayRandomSound(FlapSounds);
        }

        if (action == PlayerActions.PassedPipe)
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
