using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Stage")]
public class StageData :ScriptableObject
{
    [SerializeField] private float remitTime;
    [SerializeField] private List<Wave> waves;
    [SerializeField] private List<SpawnEvent> events;
}