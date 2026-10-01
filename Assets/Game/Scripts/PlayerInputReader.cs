using UnityEngine;

public class PlayerInputReader : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    
    [SerializeField] private Transform cameraTransform;
    private PlayerInputActions input;
    
    private void Awake()
    {
        input = new PlayerInputActions();
    }
    
    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Update()
    {
        Vector2 moveInput = input.Player.Move.ReadValue<Vector2>();
        
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDir = moveInput.x * cameraRight + moveInput.y * cameraForward;
        
        controller.Move(moveDir);
    }
}