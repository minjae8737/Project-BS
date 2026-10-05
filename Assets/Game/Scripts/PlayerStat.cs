using UnityEngine;
using UnityEngine.Assertions;

public class PlayerStat : MonoBehaviour
{
    [SerializeField] private StatData data;
    private Stat curStat;
    
    public int Hp { get; private set; }
    public float MoveSpeed => curStat.MoveSpeed;
    public float BaseSpeed => data.MoveSpeed;

    private void Awake()
    {
        if (data == null)
            Assert.IsNotNull(data, $"{gameObject.name} 에 StatData가 할당되어야 합니다.");
        
        curStat = data.CreateStat();
        Hp = curStat.MaxHp;
    }
}