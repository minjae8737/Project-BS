using System;
using UnityEngine;

[Serializable]
public struct SpawnEvent
{
    [SerializeField]private float startTime;
    [SerializeField]private float endTime;
    [SerializeField]private SpawnEventData data;
}