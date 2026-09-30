using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float gravity = -9.81f;
    
    [SerializeField] private Transform cameraTransform;
    
    private CharacterController controller;
    private PlayerInputActions input;
    private float verticalVelocity;
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
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
        
        // 이동
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDir = moveInput.x * cameraRight + moveInput.y * cameraForward;

        if (moveDir.sqrMagnitude > 1f)
        {
            moveDir.Normalize();
        }
        
        moveDir *= moveSpeed;

        // 회전
        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotateSpeed * Time.deltaTime);
        }
        
        // 중력
        verticalVelocity += gravity * Time.deltaTime;
        
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        moveDir.y = verticalVelocity;
        
        controller.Move(moveDir * Time.deltaTime);
    }
    
}
