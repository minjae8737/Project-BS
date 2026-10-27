using System;
using UnityEngine;

[Serializable]
public struct SpawnEvent
{
    [SerializeField, Min(0)] private float startTime;
    [SerializeField, Min(0)] private float endTime;
    [SerializeField] private SpawnEventData data;
    
    public float StartTime => startTime;
    public float EndTime => endTime;
    public SpawnEventData Data => data;
}