using UnityEngine;

public class SkillTarget : MonoBehaviour
{
    private IDamageable damageable;
    private Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();
    }

    public void Init(IDamageable damageable)
    {
        this.damageable = damageable;
        col.enabled = true;
    }
    
    public void TakeDamage(float rawDamage)
    {
        damageable.TakeDamage(rawDamage);
    }

    public void SetActive(bool active)
    {
        col.enabled = active;
    }
}