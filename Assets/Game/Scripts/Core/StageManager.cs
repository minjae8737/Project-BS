using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

public class StageManager : Singleton<StageManager>
{
    [SerializeField] private StageData stageData;
    private WaveManager waveManager;

    private float stageElapsed;

    protected override void Awake()
    {
        base.Awake();

        waveManager = new WaveManager();

        Assert.IsNotNull(stageData, "[StageManager] stageData가 null 입니다.");
    }

    private void Start()
    {
        //FIXME Test 코드
        StageRoutine(destroyCancellationToken).Forget();
    }

    public async UniTaskVoid StageRoutine(CancellationToken token)
    {
        CancellationTokenSource timerCts = CancellationTokenSource.CreateLinkedTokenSource(token);

        TickTimer(timerCts.Token).Forget();

        await waveManager.WaveRoutine(token, stageData);
    }

    private async UniTask TickTimer(CancellationToken token)
    {
        stageElapsed = 0f;

        while (true)
        {
            await UniTask.Yield(PlayerLoopTiming.Update, token);
            stageElapsed += Time.deltaTime;
            waveManager.SetStageElapsed(stageElapsed);
        }
    }
}