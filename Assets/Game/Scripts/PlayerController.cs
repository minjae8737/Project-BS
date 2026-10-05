using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    private PlayerInputReader inputReader;
    private PlayerMovement movement;
    private PlayerAnimationController animationController;
    
    private void Awake()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
        
        inputReader = GetComponent<PlayerInputReader>();
        movement = GetComponent<PlayerMovement>();
        animationController = GetComponent<PlayerAnimationController>();
    }

    private void OnEnable()
    {
        inputReader.JumpPressed += Jump;
    }

    private void OnDisable()
    {
        inputReader.JumpPressed -= Jump;
    }

    private void Jump()
    {
        movement.Jump();
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        Vector3 moveDir = ToWorldDirection(inputReader.MoveInput);
        
        movement.Tick(moveDir, deltaTime);
        
        animationController.SetSpeed(moveDir.magnitude);
        animationController.SetGrounded(movement.IsGrounded);
        animationController.SetVerticalSpeed(movement.VerticalVelocity);
    }
    
    private Vector3 ToWorldDirection(Vector2 moveInput)
    {
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        
        cameraForward.Normalize();
        cameraRight.Normalize();
        
        return  moveInput.x * cameraRight + moveInput.y * cameraForward;
    }
}