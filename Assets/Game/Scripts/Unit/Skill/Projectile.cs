using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Transform modelTr;
    private Vector3 moveDir;
    private float speed;
    private float maxDistance;
    private float moveDistance;
    private int penetration;
    private LayerMask targetLayer;

    public bool IsReached => moveDistance > maxDistance || penetration < 0;

    public void Init(Vector3 caster, Vector3 moveDir, float speed, float maxDistance, int penetration, LayerMask targetLayer)
    {
        transform.position = caster;
        modelTr.rotation = Quaternion.LookRotation(moveDir);

        this.moveDir = moveDir;
        this.speed = speed;
        this.maxDistance = maxDistance;
        this.penetration = penetration;
        this.targetLayer = targetLayer;
        moveDistance = 0f;
    }

    public void Tick(float deltaTime)
    {
        float deltaDistance = speed * deltaTime;
        moveDistance += deltaDistance;

        transform.position += moveDir * deltaDistance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (penetration < 0) return;

        if ((targetLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            penetration--;
            // 데미지 로직
            // SkillEffect
        }
    }
}