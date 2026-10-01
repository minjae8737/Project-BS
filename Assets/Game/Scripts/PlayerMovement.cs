using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 moveDir;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void SetMoveDirection(Vector3 input)
    {
        moveDir = input;
    }
    
    private void Update()
    {
        // 중력
        UpdateVerticalVelocity();
        
        // 이동
        Vector3 moveVelocity = CalculateMoveVelocity();
        
        // 회전
        Rotate();
        
        moveVelocity.y = verticalVelocity;
        
        controller.Move(moveVelocity * Time.deltaTime);
    }

    private Vector3 CalculateMoveVelocity()
    {
        if (moveDir.sqrMagnitude > 1f)
        {
            moveDir.Normalize();
        }

        return moveDir * moveSpeed;
    }

    private void UpdateVerticalVelocity()
    {
        verticalVelocity += gravity * Time.deltaTime;

        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }
    }

    private void Rotate()
    {
        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotateSpeed * Time.deltaTime);
        }
    }
}