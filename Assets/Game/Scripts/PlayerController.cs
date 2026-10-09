using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    private PlayerInputReader inputReader;
    private PlayerMovement movement;
    private PlayerAnimationController animationController;
    private UnitStat stat;
    
    private void Awake()
    { 
        Assert.IsNotNull(cameraTransform, $"{gameObject.name} 에 cameraTransform이 할당 되어야합니다.");
        
        inputReader = GetComponent<PlayerInputReader>();
        movement = GetComponent<PlayerMovement>();
        animationController = GetComponent<PlayerAnimationController>();
        stat = GetComponent<UnitStat>();
    }

    public void Init(StatRulesData statRulesData)
    {
        stat.Init(statRulesData);
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

        movement.Tick(moveDir, stat.MoveSpeed, deltaTime);

        animationController.UpdateLocomotion(movement.HorizontalVelocity, stat.BaseSpeed);
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