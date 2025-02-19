using UnityEngine;

public class AnimationController : MonoBehaviour, IObserver
{
    [SerializeField] private Subject Player;
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

    private void OnEnable()
    {
        Player.AddObserver(this);
    }

    private void OnDisable()
    {
        Player.RemoveObserver(this);
    }
}