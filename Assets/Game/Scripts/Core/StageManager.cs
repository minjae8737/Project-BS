using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class StageManager : Singleton<StageManager>
{
    [SerializeField] private StageData stageData;
    [SerializeField] private UnitManager unitManager;
    [SerializeField] private ExpItemManager expItemManager;
    [SerializeField] private PlayerController playerController;

    [Header("## WaveManager")] [SerializeField]
    private WaveManager waveManager;

    [Header("## LevelManager")] [SerializeField]
    private LevelManager levelManager;

    [Header("## LevelUpRewardManager")] [SerializeField]
    private LevelUpRewardManager levelUpRewardManager;

    private CancellationTokenSource stageCts;
    private float stageElapsed;

    protected override void Awake()
    {
        base.Awake();

        Assert.IsNotNull(stageData, "[StageManager] stageData가 null 입니다.");
    }

    private void OnEnable()
    {
        unitManager.MonsterDeathFinished += HandleMonsterDeathFinished;
        expItemManager.ExpCollected += HandleExpCollected;
        levelManager.LeveledUp += HandleLeveledUp;
    }

    private void OnDisable()
    {
        unitManager.MonsterDeathFinished -= HandleMonsterDeathFinished;
        expItemManager.ExpCollected -= HandleExpCollected;
        levelManager.LeveledUp -= HandleLeveledUp;
    }

    public void Init()
    {
        stageCts = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);

        unitManager.Init(playerController);
        waveManager.Init(unitManager);
        expItemManager.Init(playerController);
        levelManager.Init();
        levelUpRewardManager.Init(stageCts.Token, playerController);

        //FIXME Test 코드
        StageRoutine(stageCts.Token).Forget();
    }

    public async UniTaskVoid StageRoutine(CancellationToken token)
    {
        waveManager.WaveRoutine(stageCts.Token, stageData).Forget();
        waveManager.SpawnEventRoutine(stageCts.Token, stageData);

        try
        {
            await TickTimer(stageCts.Token);
        }
        finally
        {
            stageCts.Cancel();
            stageCts.Dispose();
        }
    }

    private async UniTask TickTimer(CancellationToken token)
    {
        stageElapsed = 0f;
        float stageLimitTime = stageData.LimitTime;

        while (true)
        {
            await UniTask.Yield(PlayerLoopTiming.Update, token);
            stageElapsed += Time.deltaTime;
            waveManager.SetStageElapsed(stageElapsed);

            if (stageLimitTime < stageElapsed) break;
        }
    }

    private void HandleMonsterDeathFinished(MonsterController monster)
    {
        expItemManager.Spawn(monster.transform.position, monster.DropExp);
    }

    private void HandleExpCollected(int exp)
    {
        levelManager.AddExp(exp);
    }

    private void HandleLeveledUp(int level)
    {
        levelUpRewardManager.AddPendingReward();
    }
}