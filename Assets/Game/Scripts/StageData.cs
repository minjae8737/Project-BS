using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Stage")]
public class StageData :ScriptableObject
{
    [SerializeField] private float limitTime;
    [SerializeField] private List<Wave> waves;
    [SerializeField] private List<SpawnEvent> events;
    
    public float LimitTime => limitTime;
    public IReadOnlyList<Wave> Waves => waves;
    public IReadOnlyList<SpawnEvent> Events => events;
}