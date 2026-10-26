using System;
using UnityEngine;

[Serializable]
public struct Wave
{
    [SerializeField] private float startTime;
    [SerializeField] private WaveData data;

    public float StartTime => startTime;
    public WaveData Data => data;
}