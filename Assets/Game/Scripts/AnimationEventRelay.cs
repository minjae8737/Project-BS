using System;
using UnityEngine;

public class AnimationEventRelay : MonoBehaviour, IStateTimeEventReceiver
{
    public event Action<string> EventRaised;

    public void OnStateTimeEvent(string eventName) => EventRaised?.Invoke(eventName);
}