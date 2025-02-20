public interface IObserver
{
    void OnNotify(Events action, int value = 0);
}