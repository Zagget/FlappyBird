using UnityEngine;

public class PlayerAudio : MonoBehaviour, IObserver
{
    [SerializeField] Subject playerSubject;
    [SerializeField] SoundData DeathSounds;
    [SerializeField] SoundData FlapSounds;
    [SerializeField] SoundData PassedPipeSounds;


    public void OnNotify(PlayerAction action)
    {
        if (action == PlayerAction.Die)
        {
            SoundManager.Instance.PlayRandomSound(DeathSounds);
        }

        if (action == PlayerAction.Jump)
        {
            SoundManager.Instance.PlayRandomSound(FlapSounds);
        }
        if (action == PlayerAction.PassedPipe)
        {
            SoundManager.Instance.PlayRandomSound(PassedPipeSounds);
        }
    }

    private void OnEnable()
    {
        playerSubject.AddObserver(this);
    }

    private void OnDisable()
    {
        playerSubject.RemoveObserver(this);
    }
}
