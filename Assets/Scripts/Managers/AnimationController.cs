using UnityEngine;

public class AnimationController : MonoBehaviour, IObserver
{
    [SerializeField] Subject Player;

    [SerializeField] Animator birdController;

    public void OnNotify(Events action, int value = 0)
    {
        if (action == Events.Jump)
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
