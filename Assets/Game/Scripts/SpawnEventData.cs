using System;
using UnityEngine;

[Serializable]
public class SpawnEventData
{
    [SerializeField] private MonsterType type;
    [SerializeField] private int spawnCount;
    [SerializeField] private int interval;
}