using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveData
{
    [SerializeField] private List<MonsterType> types;
    [SerializeField] private float spawnDelay;
    [SerializeField] private int minMonsterCount;
}