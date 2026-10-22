using UnityEngine;
using UnityEngine.Assertions;

public class UnitManager : Singleton<UnitManager>
{
    [SerializeField] private StatRulesData statRulesData;
    [SerializeField] private SpawnPositionCalculator posCalculator;
    [SerializeField, Min(0)] private float spawnRange = 10f;
    private MonsterManager monsterManager;

    //FIXME 임시 코드 
    [SerializeField] private PlayerController playerController;

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
        monsterManager.Init(statRulesData, playerController);
        playerController.Init(statRulesData);
        
        //FIXME 임시 코드
        Test();
    }

    public void SpawnMonster(MonsterType type)
    {
        if (posCalculator.TryGetMonsterPosition(playerController.transform.position, spawnRange, out var position))
        {
            monsterManager.Spawn(type, position);
        }
        else
        {
            Debug.LogWarning("[UnitManager] 몬스터 소환에 실패했습니다.");
        }
    }

    public void Test()
    {
        for (int i = 0; i < 10; i++)
        {
            SpawnMonster(MonsterType.PinkBlob);
        }
    }
}