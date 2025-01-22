using UnityEngine;

public class PlayerAudio : MonoBehaviour, IObserver
{
    [SerializeField] Subject playerSubject;
    [SerializeField] SoundData DeathSounds;
    [SerializeField] SoundData JumpSounds;


    public void OnNotify(PlayerAction action)
    {
        if (action == PlayerAction.Die)
        {
            SoundManager.Instance.PlayRandomSound(DeathSounds);
            Debug.Log("play death sound");
        }

        if (action == PlayerAction.Jump)
        {
            SoundManager.Instance.PlayRandomSound(JumpSounds);
            Debug.Log("play jump sound");
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
