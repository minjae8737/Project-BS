using UnityEngine;

public enum DropItemState
{
    Idle,
    Attracting,
}

public class ExpItem : MonoBehaviour
{
    private int exp;
    private DropItemState state;
    private bool isArrived;
    private Vector3 attractStartPos;
    private AnimationCurve attractCurve;
    private float attractDuration;
    private float attractElapsed;

    public DropItemState State => state;
    public bool IsArrived => isArrived;

    public void Init(Vector3 position, int exp, AnimationCurve curve, float duration)
    {
        transform.position = position;
        this.exp = exp;
        attractCurve = curve;
        attractDuration = duration;

        state = DropItemState.Idle;
        isArrived = false;
        attractElapsed = 0f;
    }

    public void Tick(Vector3 playerPos, float deltaTime)
    {
        if (state == DropItemState.Idle) return;
        
        MoveToTarget(playerPos, deltaTime);
    }

    public void StartAttract(Vector3 targetPos)
    {
        state = DropItemState.Attracting;
        attractStartPos = targetPos;
    }

    private void MoveToTarget(Vector3 targetPos, float deltaTime)
    {
        attractElapsed += deltaTime;

        float t = Mathf.Clamp01(attractElapsed / attractDuration);
        transform.position = Vector3.LerpUnclamped(attractStartPos, targetPos, attractCurve.Evaluate(t));
        isArrived = t >= 1f;
    }
}