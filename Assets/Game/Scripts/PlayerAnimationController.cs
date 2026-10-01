using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator anim;
    
    private static readonly int Speed = Animator.StringToHash("Speed");
    
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void SetSpeed(float value)
    {
        anim.SetFloat(Speed, value);
    }
}
