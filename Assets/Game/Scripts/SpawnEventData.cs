using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Data/Stage/SpawnEvent")]
public class SpawnEventData : ScriptableObject
{
    [SerializeField] private MonsterType type;
    [SerializeField, Min(0)] private int spawnCount;
    [SerializeField, Min(0)] private float interval;

    public MonsterType Type => type;
    public int SpawnCount => spawnCount;
    public float Interval => interval;
}
