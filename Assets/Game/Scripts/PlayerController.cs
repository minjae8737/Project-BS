using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerAnimationController animationController;
    

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        animationController = GetComponent<PlayerAnimationController>();
    }

    public void Move(Vector3 moveInput)
    {
        movement.SetMoveDirection(moveInput);
        animationController.SetSpeed(moveInput.magnitude);
    }

    public void Jump()
    {
        movement.Jump();
    }

    private void LateUpdate()
    {
        animationController.SetGrounded(movement.IsGrounded);
        animationController.SetVerticalSpeed(movement.VerticalVelocity);
    }
}
