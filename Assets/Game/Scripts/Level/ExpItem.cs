using UnityEngine;

public enum DropItemState
{
    Idle,
    PickUp,
}

public class ExpItem : MonoBehaviour
{
    [SerializeField] private float duration;
    
    private int exp;
    private DropItemState state;
    private bool isReached;
    private Vector3 startPos;
    private AnimationCurve curve;
    private float elapsed;

    public DropItemState State => state;
    public bool IsReached => isReached;

    public void Init(Vector3 position, int exp, AnimationCurve curve, float duration)
    {
        transform.position = position;
        startPos = position;
        this.exp = exp;
        if (this.curve == null) this.curve = curve;
        this.duration = duration;

        state = DropItemState.Idle;
        isReached = false;
        elapsed = 0f;
    }

    public void Tick(Vector3 playerPos, float deltaTime)
    {
        switch (state)
        {
            case DropItemState.Idle:
                break;
            case DropItemState.PickUp:
                MoveToTarget(playerPos, deltaTime);
                break;
        }
    }

    public void SetStatePickUp()
    {
        state = DropItemState.PickUp;
    }

    private void MoveToTarget(Vector3 targetPos, float deltaTime)
    {
        elapsed += deltaTime;

        float t = Mathf.Clamp01(elapsed / duration);
        transform.position = Vector3.LerpUnclamped(startPos, targetPos, curve.Evaluate(t));
        isReached = t >= 1f;
    }
}