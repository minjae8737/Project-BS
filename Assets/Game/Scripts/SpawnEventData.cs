using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Data/Stage/SpawnEvent")]
public class SpawnEventData : ScriptableObject
{
    [SerializeField] private MonsterType type;
    [SerializeField] private int spawnCount;
    [SerializeField] private int interval;

    public MonsterType Type => type;
    public int SpawnCount => spawnCount;
    public int Interval => interval;
}