using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    private Vector2 moveInput = Vector2.zero;
    private InputAction moveAction;
    private InputAction jumpAction;

    public Vector2 MoveInput => moveInput;

    public event Action JumpPressed;

    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction(InputActionNames.PlayerMoveName, throwIfNotFound: true);
        jumpAction = InputSystem.actions.FindAction(InputActionNames.PlayerJumpName, throwIfNotFound: true);
    }

    private void OnEnable()
    {
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;

        jumpAction.performed += OnJump;
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;

        jumpAction.performed -= OnJump;
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