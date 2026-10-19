using System;
using UnityEngine;
using UnityEngine.Assertions;

public class MonsterController : MonoBehaviour, IDamageable
{
    private MonsterMovement movement;
    private MonsterAnimationController animationController;
    private UnitStat stat;
    private Collider col;
    private HitFlash hitFlash;
    private SkillTarget skillTarget;

    private PlayerController target;
    private float attackTimer;

    [SerializeField, Min(0)] private float separationRadius;
    [SerializeField, Min(0)] private float attackCooldown;
    [SerializeField, Min(0)] private float height; 


    public float SeparationRadius => separationRadius;
    public bool IsDead => stat.IsDead;

    public event Action<MonsterController> DeathFinished;

    private void Awake()
    {
        movement = GetComponent<MonsterMovement>();
        animationController = GetComponent<MonsterAnimationController>();
        hitFlash = GetComponentInChildren<HitFlash>();
        col = GetComponent<Collider>();
        stat = GetComponent<UnitStat>();
        skillTarget = GetComponentInChildren<SkillTarget>();
        
        Assert.IsNotNull(skillTarget, $"{gameObject.name} 에 skillTarget이 할당 되어야합니다.");
    }

    private void OnEnable()
    {
        stat.Died += HandleDeath;
        stat.Damaged += HandleDamaged;
        animationController.DiedClipEnd += HandleDeathClipEndEvent;
    }

    private void OnDisable()
    {
        stat.Died -= HandleDeath;
        stat.Damaged -= HandleDamaged;
        animationController.DiedClipEnd -= HandleDeathClipEndEvent;
    }

    public void Init(StatRulesData statRulesData, PlayerController playerController)
    {
        stat.Init(statRulesData);
        skillTarget.SetActive(true);
        skillTarget.Init(this);
        hitFlash.Stop();
        target = playerController;

        col.enabled = true;
        movement.SetKinematic(false);
        attackTimer = 0f;
    }

    public void Tick(Vector3 moveDir, float distance, float targetRadius, float deltaTime)
    {
        TryAttack(distance, targetRadius, deltaTime);

        movement.Tick(moveDir, stat.MoveSpeed);
    }

    private void TryAttack(float distance, float targetRadius, float deltaTime)
    {
        attackTimer += deltaTime;
        
        if (attackCooldown < attackTimer)
        {
            bool canAttack = distance - targetRadius - separationRadius < 0;

            if (canAttack)
            {
                target.TakeDamage(stat.AttackPower);
                attackTimer = 0f;
            }
        }
    }

    public void TakeDamage(float rawDamage)
    {
        stat.TakeDamage(rawDamage);
    }
    
    private void HandleDamaged(int damage)
    {
        Vector3 position = transform.position;
        position.y += height;
        
        hitFlash.Play();
        DamageTextSpawner.Instance.Show(position, damage);
    }

    private void HandleDeath()
    {
        movement.Stop();
        
        col.enabled = false;
        skillTarget.SetActive(false);
        movement.SetKinematic(true);
        animationController.PlayDeathClip();
    }

    private void HandleDeathClipEndEvent()
    {
        DeathFinished?.Invoke(this);
    }
}