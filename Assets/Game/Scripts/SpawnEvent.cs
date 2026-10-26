using System;
using UnityEngine;

[Serializable]
public struct SpawnEvent
{
    [SerializeField] private float startTime;
    [SerializeField] private float endTime;
    [SerializeField] private SpawnEventData data;
    
    public float StartTime => startTime;
    public float EndTime => endTime;
    public SpawnEventData Data => data;
}