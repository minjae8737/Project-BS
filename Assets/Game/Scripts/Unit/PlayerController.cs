using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField, Min(0)] private float height;     
    
    private PlayerInputReader inputReader;
    private PlayerMovement movement;
    private PlayerAnimationController animationController;
    private UnitStat stat;
    private PlayerSkills skills;
    
    public float Radius => movement.Radius;
    public bool IsDead => stat.IsDead;
    
    private void Awake()
    { 
        Assert.IsNotNull(cameraTransform, $"{gameObject.name} 에 cameraTransform이 할당 되어야합니다.");
        
        inputReader = GetComponent<PlayerInputReader>();
        movement = GetComponent<PlayerMovement>();
        animationController = GetComponent<PlayerAnimationController>();
        stat = GetComponent<UnitStat>();
        skills = GetComponent<PlayerSkills>();
    }

    public void Init(StatRulesData statRulesData)
    {
        stat.Init(statRulesData);
        skills.Init(); //TODO 스킬은 얻어서 초기화해야하므로 초기화 구조가 바뀔것 
    }

    private void OnEnable()
    {
        inputReader.JumpPressed += Jump;
        stat.Died += HandleDeath;
        stat.Damaged += HandleDamaged;
    }

    private void OnDisable()
    {
        inputReader.JumpPressed -= Jump;
        stat.Died -= HandleDeath;
        stat.Damaged -= HandleDamaged;
    }

    private void Jump()
    {
        movement.Jump();
    }

    private void Update()
    {
        if (IsDead) return;
        
        float deltaTime = Time.deltaTime;
        
        // 스킬
        skills.Tick(transform.position, deltaTime);
        
        // 이동
        Vector3 moveDir = ToWorldDirection(inputReader.MoveInput);

        movement.Tick(moveDir, stat.MoveSpeed, deltaTime);

        // 애니메이션
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
    
    public void TakeDamage(float rawDamage)
    {
        stat.TakeDamage(rawDamage);
    }
    
    private void HandleDamaged(int damage)
    {
        Vector3 position = transform.position;
        position.y += height;

        DamageTextSpawner.Instance.Show(position, damage);
    }
    
    private void HandleDeath()
    {
        animationController.PlayDeathClip();
    }

    private void HandleDeathClipEndEvent()
    {
    }
}