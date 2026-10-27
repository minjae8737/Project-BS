using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

public class StageManager : Singleton<StageManager>
{
    [SerializeField] private StageData stageData;
    [SerializeField] private WaveManager waveManager;

    private float stageElapsed;

    protected override void Awake()
    {
        base.Awake();
        Assert.IsNotNull(stageData, "[StageManager] stageData가 null 입니다.");
    }

    public void Init()
    {
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
}