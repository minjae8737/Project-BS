using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

public class StageManager : Singleton<StageManager>
{
    [SerializeField] private StageData stageData;
    [SerializeField] private UnitManager unitManager;
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private ExpItemManager expItemManager;

    [SerializeField] private PlayerController playerController;
    
    private float stageElapsed;

    protected override void Awake()
    {
        base.Awake();
        
        Assert.IsNotNull(stageData, "[StageManager] stageData가 null 입니다.");
    }

    private void OnEnable()
    {
        unitManager.MonsterDeathFinished += HandleMonsterDeathFinished;
    }

    private void OnDisable()
    {
        unitManager.MonsterDeathFinished -= HandleMonsterDeathFinished;
    }

    public void Init()
    {
        unitManager.Init(playerController);
        waveManager.Init(unitManager);
        expItemManager.Init(playerController);
        
        //FIXME Test 코드
        StageRoutine(destroyCancellationToken).Forget();
    }

    public async UniTaskVoid StageRoutine(CancellationToken token)
    {
        CancellationTokenSource stageCts = CancellationTokenSource.CreateLinkedTokenSource(token);

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
        //FIXME 
        int exp = 1;
        
        expItemManager.Spawn(monster.transform.position, exp);
    }
}