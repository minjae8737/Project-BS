using System;
using UnityEngine;

[Serializable]
public struct Wave
{
    [SerializeField] private float startTime;
    [SerializeField] private WaveData data;
}