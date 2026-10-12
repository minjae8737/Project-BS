using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class UnitManager : Singleton<UnitManager>
{
    [SerializeField] private StatRulesData statRulesData;
    private MonsterManager monsterManager;

    //FIXME 임시 코드 
    [SerializeField] private PlayerController playerController;
    [SerializeField] private List<MonsterController> monsters;

    protected override void Awake()
    {
        base.Awake();
        
        monsterManager = GetComponent<MonsterManager>();
        
        Assert.IsNotNull(monsterManager, $"{gameObject.name} 에 MonsterManager가 할당되어야 합니다.");
        Assert.IsNotNull(statRulesData, $"{gameObject.name} 에 MinStatData가 할당되어야 합니다.");
        Assert.IsNotNull(playerController, $"{gameObject.name} 에 PlayerController가 할당되어야 합니다.");
    }

    private void Start()
    { 
        //FIXME 임시 코드
        monsterManager.Init(playerController);

        playerController.Init(statRulesData);

        foreach (var monster in monsters)
        {
            monster.Init(statRulesData, playerController);
            monsterManager.AddMonster(monster);
        }
    }
}