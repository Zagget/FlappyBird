using UnityEngine;

public class AnimationController : MonoBehaviour, IObserver
{
    [SerializeField] private Subject gameManagerSubject;
    [SerializeField] private Animator birdController;

    public void OnNotify(Events @event, int value = 0)
    {
        if (@event == Events.Jump)
        {
            PlayAnimation(birdController, "Flap");
        }
    }

    public void PlayAnimation(Animator anim, string animatonName)
    {
        anim.Play(animatonName);
    }

    void OnEnable() => gameManagerSubject.AddObserver(this);
    void OnDisable() => gameManagerSubject.RemoveObserver(this);
}