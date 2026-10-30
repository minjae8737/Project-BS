using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Transform modelTr;
    private float damage;
    private Vector3 moveDir;
    private float speed;
    private float maxDistance;
    private float moveDistance;
    private int penetration;
    private LayerMask targetLayer;
    private LayerMask obstacleLayer;
    private bool isEnded;

    public bool IsReached => moveDistance > maxDistance || penetration < 0 || isEnded;

    public void Init(
        float damage,
        Vector3 startPos,
        Vector3 moveDir,
        float speed,
        float maxDistance,
        int penetration,
        LayerMask targetLayer,
        LayerMask obstacleLayer)
    {
        transform.position = startPos;
        modelTr.rotation = Quaternion.LookRotation(moveDir);

        this.damage = damage;
        this.moveDir = moveDir;
        this.speed = speed;
        this.maxDistance = maxDistance;
        this.penetration = penetration;
        this.targetLayer = targetLayer;
        this.obstacleLayer = obstacleLayer;
        moveDistance = 0f;
        isEnded = false;
    }

    public void Tick(float deltaTime)
    {
        float deltaDistance = speed * deltaTime;
        moveDistance += deltaDistance;

        transform.position += moveDir * deltaDistance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsReached) return;

        int mask = 1 << other.gameObject.layer;
        
        if ((obstacleLayer.value & mask) != 0)
        {
            isEnded = true;
            return;
        }

        if ((targetLayer.value & mask) != 0)
        {
            if (other.TryGetComponent(out SkillTarget skillTarget))
            {
                skillTarget.TakeDamage(damage);
                penetration--;
            }
            else
            {
                Debug.LogWarning($"{other.gameObject.name} 에 SkillTarget이 없습니다.", this);
            }
        }
    }
}