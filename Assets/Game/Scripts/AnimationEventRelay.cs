using System;
using UnityEngine;

public class AnimationEventRelay : MonoBehaviour, IStateTimeEventReceiver
{
    public event Action<AniClipEventName> EventRaised;

    public void OnStateTimeEvent(AniClipEventName eventName) => EventRaised?.Invoke(eventName);
}