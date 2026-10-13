using System;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator anim;
    private AnimationEventRelay eventRelay;
    
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int VerticalSpeed = Animator.StringToHash("VerticalSpeed");
    private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    private static readonly int LocomotionSpeedMultiplier = Animator.StringToHash("LocomotionSpeedMultiplier");
    private static readonly int Die = Animator.StringToHash("Die");

    public event Action DiedClipEnd;
    
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        eventRelay = GetComponentInChildren<AnimationEventRelay>();
        
        Assert.IsNotNull(eventRelay,$"{name}에 AnimationEventRelay을 할당해야합니다.");
    }

    private void OnEnable()
    {
        eventRelay.EventRaised += HandleAnimationEvent;
    }

    private void OnDisable()
    {
        eventRelay.EventRaised -= HandleAnimationEvent;
    }
    
    public void UpdateLocomotion(float moveSpeed, float baseSpeed)
    {
        float value = 0;
        
        if (baseSpeed != 0) 
            value = moveSpeed / baseSpeed;

        anim.SetFloat(Speed, Mathf.Clamp01(value));
        anim.SetFloat(LocomotionSpeedMultiplier, Mathf.Max(value, 1f));
    }

    public void SetGrounded(bool value)
    {
        anim.SetBool(IsGrounded, value);
    }

    public void SetVerticalSpeed(float value)
    {
        anim.SetFloat(VerticalSpeed, value);
    }
    
    public void PlayDeathClip()
    {
        anim.SetTrigger(Die);
    }
    
    private void HandleAnimationEvent(AniClipEventName eventName)
    {
        if (eventName == AniClipEventName.Die)
        {
            DiedClipEnd?.Invoke();
        }
    }
}