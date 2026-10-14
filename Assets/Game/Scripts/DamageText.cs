using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private float height = 1f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float duration = 0.7f;
    private float elapsed;
    private Vector3 targetPos;
    public bool IsEnd => elapsed > duration;

    public void SetDamageText(int damage, Vector3 startPos)
    {
        transform.position = startPos;
        
        startPos.y += height;

        text.SetText("{0}", damage);
        targetPos = startPos;

        elapsed = 0f;
    }

    public bool Tick(float deltaTime, Quaternion rotation)
    {
        elapsed += deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * deltaTime);
        transform.rotation = rotation;

        return IsEnd;
    }
}