using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerStat : MonoBehaviour
{
    [SerializeField] private StatRulesData statRulesData;
    [SerializeField] private StatData statData;
    private Dictionary<StatType, StatSlot> stats;

    public int Hp { get; private set; }
    // public float MoveSpeed => curStat.MoveSpeed;
    // public float BaseSpeed => data.MoveSpeed;

    private void Awake()
    {
        Assert.IsNotNull(statRulesData, $"{gameObject.name} 에 MinStatData가 할당되어야 합니다.");
        Assert.IsNotNull(statData, $"{gameObject.name} 에 StatData가 할당되어야 합니다.");

        stats = new();
        
        // curStat = data.CreateStat();
        // Hp = curStat.MaxHp;
    }
    
    public void Init()
    {
        
    }
}