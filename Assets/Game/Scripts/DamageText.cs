using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    private float speed;
    private float duration;
    private float elapsed;
    private Vector3 targetPos;

    public bool IsEnd => elapsed < duration;

    public void SetDamageText(int damage, Vector3 startPos, float speed = 5f, float duration = 0.7f)
    {
        transform.position = startPos;
        startPos.y += 5f;

        text.SetText(damage + "");
        targetPos = startPos;
        this.speed = speed;
        this.duration = duration;

        elapsed = 0f;
    }

    public bool Tick(float deltaTime, Quaternion rotation)
    {
        elapsed += deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * deltaTime);

        return IsEnd;
    }
}