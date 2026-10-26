using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Data/Stage/Wave")]
public class WaveData : ScriptableObject
{
    [SerializeField] private List<MonsterType> types;
    [SerializeField] private float spawnDelay;
    [SerializeField] private int minMonsterCount;

    public IReadOnlyList<MonsterType> Types => types;
    public float SpawnDelay => spawnDelay;
    public int MinMonsterCount => minMonsterCount;
}