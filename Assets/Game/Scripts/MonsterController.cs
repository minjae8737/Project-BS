using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private MonsterMovement movement;
    private UnitStat stat;

    [SerializeField, Min(0)] private float separationRadius;

    public float SeparationRadius => separationRadius;
    
    private void Awake()
    {
        movement = GetComponent<MonsterMovement>();
        stat = GetComponent<UnitStat>();
    }

    public void Init(StatRulesData statRulesData)
    {
        stat.Init(statRulesData);
    }

    public void Tick(Vector3 moveDir)
    {
        movement.Tick(moveDir, stat.MoveSpeed);
    }
}