using UnityEngine;
using UnityEngine.Assertions;

public class UnitManager : Singleton<UnitManager>
{
    [SerializeField] private StatRulesData statRulesData;
    [SerializeField] private SpawnPositionCalculator posCalculator;
    [SerializeField, Min(0)] private float spawnRange = 20f;
    [SerializeField, Min(0)] private float relocateRange = 50f;
    private MonsterManager monsterManager;

    public int MonsterCount => monsterManager.MonsterCount;

    //FIXME 임시 코드 
    [SerializeField] private PlayerController playerController;

    protected override void Awake()
    {
        base.Awake();
        
        monsterManager = GetComponent<MonsterManager>();
        
        Assert.IsNotNull(monsterManager, $"{gameObject.name} 에 MonsterManager가 할당되어야 합니다.");
        Assert.IsNotNull(statRulesData, $"{gameObject.name} 에 MinStatData가 할당되어야 합니다.");
        Assert.IsNotNull(playerController, $"{gameObject.name} 에 PlayerController가 할당되어야 합니다.");
        Assert.IsTrue(spawnRange + posCalculator.MinSpawnDistance + 10f <= relocateRange, $"{gameObject.name} relocateRange가 (spawnRange + MinSpawnDistance) 보다 짧습니다.");
    }

    public void Init()
    { 
        monsterManager.Init(statRulesData, playerController, relocateRange);
        playerController.Init(statRulesData);
    }

    private void OnEnable()
    {
        monsterManager.MonsterOutOfRange += HandleMonsterOutOfRange;
    }

    private void OnDisable()
    {
        monsterManager.MonsterOutOfRange -= HandleMonsterOutOfRange;
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

    private void HandleMonsterOutOfRange(MonsterController monster)
    {
        Vector3 playerMoveDir = playerController.MoveDir;
        
        if (posCalculator.TryGetMonsterRelocatePosition(
                playerController.transform.position,
                playerMoveDir, 
                spawnRange,
                out var position))
        {
            monsterManager.Relocate(monster, position);
        }
    }
}