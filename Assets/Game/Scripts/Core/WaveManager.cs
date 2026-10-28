using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class WaveManager
{
    private StageData stageData;
    private float stageElapsed;

    [SerializeField] private int maxMonsterCount = 200;
    [SerializeField] private int maxSpawnPerFrame = 5;

    public async UniTask WaveRoutine(CancellationToken token, StageData stageData)
    {
        CheckWave(stageData);

        this.stageData = stageData;

        while (true)
        {
            int waveIndex = FindWaveIndex();
            Wave wave = stageData.Waves[waveIndex];
            int minMonsterCount = wave.Data.MinMonsterCount;

            if (CanSpawn())
            {
                if (minMonsterCount > UnitManager.Instance.MonsterCount)
                {
                    await SpawnMinCount(wave, token);
                }
                else
                {
                    SpawnWave(wave);
                }
            }

            await UniTask.Delay(TimeSpan.FromSeconds(wave.Data.SpawnDelay), cancellationToken: token);
        }
    }

    public void SpawnEventRoutine(CancellationToken token, StageData stageData)
    {
        if (stageData.Events.Count <= 0) return;

        CheckEvent(stageData);

        for (int i = 0; i < stageData.Events.Count; i++)
        {
            RunSpawnEvent(stageData.Events[i], token).Forget();
        }
    }

    private void CheckWave(StageData stageData)
    {
        Assert.IsNotNull(stageData.Waves, "[WaveManager] Wave 목록이 할당되지 않았습니다.");
        Assert.IsTrue(0 < stageData.Waves.Count, "[WaveManager] Wave 목록이 비어있습니다.");

        float preWaveStartTime = 0;

        foreach (Wave wave in stageData.Waves)
        {
            Assert.IsTrue(preWaveStartTime <= wave.StartTime, "[WaveManager] Wave의 StartTime 순서가 맞지않습니다.");
            Assert.IsNotNull(wave.Data, "[WaveManager] Wave 목록이 할당되지 않았습니다.");

            Assert.IsTrue(0 < wave.Data.SpawnDelay, "[WaveManager] Wave의 SpawnDelay 가 0입니다.");
            Assert.IsTrue(0 < wave.Data.Types.Count, "[WaveManager] Wave의 몬스터 타입이 0입니다.");

            preWaveStartTime = wave.StartTime;
        }
    }

    private void CheckEvent(StageData stageData)
    {
        foreach (SpawnEvent spawnEvent in stageData.Events)
        {
            Assert.IsNotNull(spawnEvent.Data, "[WaveManager] SpawnEvent Data가 할당되지 않았습니다.");
            Assert.IsTrue(spawnEvent.StartTime <= spawnEvent.EndTime,
                "[WaveManager] SpawnEvent 의 StartTime이 EndTime보다 늦습니다.");
            Assert.IsTrue(0f <= spawnEvent.Data.Interval, "[WaveManager] SpawnEvent 의 Interval이 0보다 작습니다.");
        }
    }

    private bool CanSpawn()
    {
        return UnitManager.Instance.MonsterCount < maxMonsterCount;
    }

    private int FindWaveIndex()
    {
        for (int i = stageData.Waves.Count - 1; i >= 0; i--)
        {
            if (stageData.Waves[i].StartTime < stageElapsed)
                return i;
        }

        return 0;
    }

    private void SpawnWave(Wave wave)
    {
        foreach (MonsterType type in wave.Data.Types)
        {
            if (UnitManager.Instance.MonsterCount >= maxMonsterCount) break;

            UnitManager.Instance.SpawnMonster(type);
        }
    }

    private async UniTask SpawnMinCount(Wave wave, CancellationToken token)
    {
        int minMonsterCount = wave.Data.MinMonsterCount;
        int spawnCount = minMonsterCount - UnitManager.Instance.MonsterCount;

        int frameSpawnCount = 0;

        for (int i = 0; i < spawnCount; i++)
        {
            if (minMonsterCount <= UnitManager.Instance.MonsterCount) break;

            int typeIndex = i % wave.Data.Types.Count;

            UnitManager.Instance.SpawnMonster(wave.Data.Types[typeIndex]);
            frameSpawnCount++;

            if (frameSpawnCount >= maxSpawnPerFrame)
            {
                frameSpawnCount = 0;
                await UniTask.NextFrame(token);
            }
        }
    }

    private async UniTask RunSpawnEvent(SpawnEvent spawnEvent, CancellationToken token)
    {
        int frameSpawnCount = 0;

        await UniTask.WaitUntil(() => stageElapsed > spawnEvent.StartTime, cancellationToken: token);

        while (true)
        {
            for (int i = 0; i < spawnEvent.Data.SpawnCount; i++)
            {
                UnitManager.Instance.SpawnMonster(spawnEvent.Data.Type);
                frameSpawnCount++;

                if (frameSpawnCount >= maxSpawnPerFrame)
                {
                    frameSpawnCount = 0;
                    await UniTask.NextFrame(token);
                }
            }

            if (spawnEvent.Data.Interval <= 0f) break;

            await UniTask.Delay(TimeSpan.FromSeconds(spawnEvent.Data.Interval), cancellationToken: token);

            if (stageElapsed > spawnEvent.EndTime) break;
        }
    }

    public void SetStageElapsed(float elapsed)
    {
        stageElapsed = elapsed;
    }
}