using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subject : MonoBehaviour
{
    private List<IObserver> observers = new List<IObserver>();

    public void AddObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    // Remove an observer from the list
    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    // Notify all observers about an event
    public void NotifyObservers(Events action, int value = 0)
    {
        foreach (IObserver observer in observers)
        {
            observer.OnNotify(action, value);
        }
    }
}