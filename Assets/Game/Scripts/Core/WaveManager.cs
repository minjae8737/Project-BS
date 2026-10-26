using System.Threading;
using Cysharp.Threading.Tasks;

public class WaveManager
{
    private StageData stageData;
    private int waveIndex;
    private float stageElapsed;

    private const int MaxMonsterCount = 200;
    
    public async UniTask WaveRoutine(CancellationToken token, StageData stageData)
    {
        bool canSpawn = await CheckCanSpawn(token);

        if (canSpawn)
        {
            await CheckWaveIndex();
            
        }
    }

    private async UniTask<bool> CheckCanSpawn(CancellationToken token)
    {
        return UnitManager.Instance.MonsterCount > MaxMonsterCount;
    }

    private async UniTask CheckWaveIndex()
    {
        for (int i = stageData.Waves.Count - 1; i >= 0; i--)
        {
            if (stageData.Waves[i].StartTime < stageElapsed)
            {
                waveIndex = i;
            }
        }
    }

    public void SetStageElapsed(float elapsed)
    {
        stageElapsed = elapsed;
    }
}