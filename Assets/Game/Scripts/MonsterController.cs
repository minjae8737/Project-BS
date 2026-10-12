using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private MonsterMovement movement;
    private UnitStat stat;

    private PlayerController target;
    private float attackTimer;

    [SerializeField, Min(0)] private float separationRadius;
    [SerializeField, Min(0)] private float attackCooldown;

    public float SeparationRadius => separationRadius;

    private void Awake()
    {
        movement = GetComponent<MonsterMovement>();
        stat = GetComponent<UnitStat>();
    }

    public void Init(StatRulesData statRulesData, PlayerController playerController)
    {
        stat.Init(statRulesData);
        target = playerController;

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
}