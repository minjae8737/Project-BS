using System;
using UnityEngine;
using UnityEngine.Assertions;

public class UnitManager : Singleton<UnitManager>
{
    [SerializeField] private StatRulesData statRulesData;
    private float[] statMinValueArr;

    //FIXME 임시 코드 
    [SerializeField] private PlayerStat playerStat;

    protected override void Awake()
    {
        base.Awake();
        
        Assert.IsNotNull(statRulesData, $"{gameObject.name} 에 MinStatData가 할당되어야 합니다.");
    }

    private void Start()
    {
        //FIXME 임시 코드
        if (playerStat != null)
        {
            playerStat.Init(statRulesData);
        }
    }
}