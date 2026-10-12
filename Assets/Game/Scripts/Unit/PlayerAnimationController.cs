using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator anim;
    
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int VerticalSpeed = Animator.StringToHash("VerticalSpeed");
    private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    private static readonly int LocomotionSpeedMultiplier = Animator.StringToHash("LocomotionSpeedMultiplier");
    
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
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
}