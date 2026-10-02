using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float jumpHeight = 4f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 moveDir;
    private float verticalVelocity;
    private const float GroundedVerticalVelocity = -2f;

    public bool IsGrounded => controller.isGrounded;
    public float VerticalVelocity => verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void SetMoveDirection(Vector3 input)
    {
        moveDir = Vector3.ClampMagnitude(input, 1f);
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        // 중력
        ApplyGravity(deltaTime);

        // 회전
        Rotate(deltaTime);

        // 이동
        Move(deltaTime);
    }

    private void ApplyGravity(float deltaTime)
    {
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = GroundedVerticalVelocity;
        }
        else
        {
            verticalVelocity += gravity * deltaTime;
        }
    }

    private void Rotate(float deltaTime)
    {
        if (moveDir.sqrMagnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(moveDir);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotateSpeed * deltaTime);
    }

    private void Move(float deltaTime)
    {
        Vector3 velocity = moveDir * moveSpeed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * deltaTime);
    }

    public void Jump()
    {
        if (controller.isGrounded)
        {
            verticalVelocity += Mathf.Sqrt(jumpHeight * gravity * -2f);
        }
    }
}