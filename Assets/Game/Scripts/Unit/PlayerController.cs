using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField, Min(0)] private float height;

    //FIXME 임시 시작 스킬
    [SerializeField] private List<AbilityData> startingAbilities;

    private PlayerInputReader inputReader;
    private PlayerMovement movement;
    private PlayerAnimationController animationController;
    private PlayerAbilities abilities;
    private UnitStat stat;
    private PlayerSkills skills;
    private SkillTarget skillTarget;

    public Vector3 MoveDir => movement.MoveDir;
    public float Radius => movement.Radius;
    public bool IsDead => stat.IsDead;
    public float PickupRadius => stat.PickupRadius;
    public Vector3 TargetPos => skillTarget.transform.position;
    public Vector3 Pos => transform.position;
    public IReadOnlyPlayerAbilities Abilities => abilities;

    private void Awake()
    {
        inputReader = GetComponent<PlayerInputReader>();
        movement = GetComponent<PlayerMovement>();
        animationController = GetComponent<PlayerAnimationController>();
        stat = GetComponent<UnitStat>();
        skills = GetComponent<PlayerSkills>();
        skillTarget = GetComponentInChildren<SkillTarget>();
        abilities = new();

        Assert.IsNotNull(cameraTransform, $"{gameObject.name} 에 cameraTransform이 할당 되어야합니다.");
        Assert.IsNotNull(skillTarget, $"{gameObject.name} 에 skillTarget이 할당 되어야합니다.");
    }

    public void Init(StatRulesData statRulesData)
    {
        abilities.Init();
        stat.Init(statRulesData);
        skills.Init();
        skillTarget.SetActive(true);
        skillTarget.Init(this);

        //FIXME 임시 코드
        foreach (AbilityData data in startingAbilities)
        {
            ApplyAbility(data);
        }
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
        skills.Tick(transform.position, stat.AttackPower, deltaTime);

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

        return moveInput.x * cameraRight + moveInput.y * cameraForward;
    }

    public void TakeDamage(float rawDamage)
    {
        stat.TakeDamage(rawDamage);
    }

    public void ApplyAbility(AbilityData abilityData)
    {
        bool leveledUp = abilities.TryLevelUp(abilityData, out int newLevel);

        if (!leveledUp)
        {
            bool acquired = abilities.TryAcquire(abilityData);

            if (acquired)
            {
                newLevel = 1;
            }
            else
            {
                Debug.LogError($"[PlayerController] ApplyAbility {abilityData.DisplayName} 실패.\n", this);
                return;
            }
        }
        
        switch (abilityData)
        {
            case SkillData skillData:
                if (leveledUp)
                    skills.SetSkillLevel(skillData, newLevel);
                else
                    skills.AddSkill(skillData);
                break;
            case PassiveData passiveData:
                if (passiveData.TryGetLevelBonus(newLevel, out var statModifiers))
                    stat.AddModifiers(statModifiers);
                else 
                    Debug.LogError($"[PlayerController] statModifiers의 데이터가 비어있습니다.", this);
                break;
            default:
                Debug.LogError($"[PlayerController] ApplyAbility {abilityData.DisplayName} 잘못된 데이터 타입입니다.", this);
                break;
        }
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
        skillTarget.SetActive(false);
    }

    private void HandleDeathClipEndEvent()
    {
    }
}