using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    private Rigidbody rigid;
    private Vector3 moveDir;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void SetMoveDirection(Vector3 input)
    {
        input.y = 0;
        moveDir = input.normalized;
    }
    
    public void Tick(Vector3 input, float moveSpeed)
    {
        SetMoveDirection(input);

        // 회전
        // Rotate();

        // 이동
        Move(moveSpeed);
    }

    private void Move(float moveSpeed)
    {
        Vector3 velocity = moveDir * moveSpeed;

        velocity.y = rigid.linearVelocity.y;

        rigid.linearVelocity = velocity;
    }
}