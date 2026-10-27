using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Data/Stage/Wave")]
public class WaveData : ScriptableObject
{
    [SerializeField] private List<MonsterType> types;
    [SerializeField, Min(0)] private float spawnDelay;
    [SerializeField, Min(1)] private int minMonsterCount;

    public IReadOnlyList<MonsterType> Types => types;
    public float SpawnDelay => spawnDelay;
    public int MinMonsterCount => minMonsterCount;
}