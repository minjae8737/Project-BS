using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private MonsterMovement movement;
    private UnitStat stat;

    //FIXME 임시코드
    [SerializeField] private Transform target;
    
    private void Awake()
    {
        movement = GetComponent<MonsterMovement>();
        stat = GetComponent<UnitStat>();
    }

    private void FixedUpdate()
    {
        Vector3 dir = target.position - transform.position;
        dir.y = 0;

        movement.Tick(dir, 2);
    }
}