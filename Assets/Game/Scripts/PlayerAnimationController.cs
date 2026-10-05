using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator anim;
    
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int VerticalSpeed = Animator.StringToHash("VerticalSpeed");
    private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void SetSpeed(float moveSpeed,float baseSpeed)
    {
        float value = 0;
        
        if (baseSpeed != 0) 
            value = moveSpeed / baseSpeed;
        
        anim.SetFloat(Speed, value);
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