using System;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private MonsterMovement movement;
    private MonsterAnimationController animationController;
    private Collider col;
    private UnitStat stat;

    private PlayerController target;
    private float attackTimer;

    [SerializeField, Min(0)] private float separationRadius;
    [SerializeField, Min(0)] private float attackCooldown;

    public float SeparationRadius => separationRadius;
    public bool IsDead => stat.IsDead;

    public event Action<MonsterController> EndDeathClip;

    private void Awake()
    {
        movement = GetComponent<MonsterMovement>();
        animationController = GetComponent<MonsterAnimationController>();
        col = GetComponent<Collider>();
        stat = GetComponent<UnitStat>();
    }

    private void OnEnable()
    {
        stat.Died += HandleDeath;
        animationController.DiedClipEnd += HandleDeathClipEndEvent;
    }

    private void OnDisable()
    {
        stat.Died -= HandleDeath;
        animationController.DiedClipEnd -= HandleDeathClipEndEvent;
    }

    public void Init(StatRulesData statRulesData, PlayerController playerController)
    {
        stat.Init(statRulesData);
        target = playerController;

        col.enabled = true;
        movement.ChangeIsKinematic(false);
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

    private void HandleDeath()
    {
        movement.Tick(Vector3.zero, 0f);
        
        col.enabled = false;
        movement.ChangeIsKinematic(true);
        animationController.PlayDeathClip();
    }

    private void HandleDeathClipEndEvent()
    {
        EndDeathClip?.Invoke(this);
    }
}