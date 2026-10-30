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
        moveDir = Vector3.ClampMagnitude(input, 1f);
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

    public void Stop()
    {
        SetMoveDirection(Vector3.zero);
        Move(0);
    }
    
    public void SetKinematic(bool isKinematic)
    {
        rigid.isKinematic = isKinematic;
    }
}