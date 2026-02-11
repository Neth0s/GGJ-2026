using UnityEngine.Events;
using UnityEngine;

public static class GameEventsManager
{
    public static UnityEvent OnMaskChange = new();

    public static void TriggerMaskChange()
    {
        OnMaskChange.Invoke();
    }
}