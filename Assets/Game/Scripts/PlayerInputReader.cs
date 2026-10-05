using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    private PlayerInputActions input;
    
    private Vector2 moveInput = Vector2.zero;

    public Vector2 MoveInput => moveInput;

    public event Action JumpPressed;
    
    private void Awake()
    {
        input = new PlayerInputActions();
    }
    
    private void OnEnable()
    {
        input.Player.Move.performed += OnMove;
        input.Player.Move.canceled += OnMove;
        
        input.Player.Jump.performed += OnJump;
        
        input.Enable();
    }

    private void OnDisable()
    {
        input.Player.Move.performed -= OnMove;
        input.Player.Move.canceled -= OnMove;
        
        input.Player.Jump.performed -= OnJump;
        
        input.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }    

    private void OnJump(InputAction.CallbackContext context)
    {
        JumpPressed?.Invoke();
    }
}